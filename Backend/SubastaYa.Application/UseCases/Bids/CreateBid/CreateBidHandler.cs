using SubastaYa.Application.Interfaces;
using SubastaYa.Domain.Exceptions;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Service.Bids;
using SubastaYa.Domain.Entities;
using SubastaYa.Domain.Constants;
using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.Common;
using Microsoft.Extensions.Logging;

namespace SubastaYa.Application.UseCases.Bids.CreateBid
{
    public class CreateBidHandler : ICreateBidHandler
    {
        private readonly IBidRepository _bidRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAuctionRepository _aucRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuctionNotifier _auctionNotifier;
        private readonly ILogger<CreateBidHandler> _logger;
        private readonly ITransactionLedgerRepository _transactionLedgerRepository;
        private readonly IAuditLogRepository _auditLogRepository; 

        public CreateBidHandler(
            IBidRepository bidRepository, 
            IAuctionRepository aucRepository, 
            IWalletRepository walletRepository, 
            IUnitOfWork unitOfWork, 
            IUserRepository userRepository, 
            IAuctionNotifier auctionNotifier, 
            ILogger<CreateBidHandler> logger, 
            ITransactionLedgerRepository transactionLedgerRepository,
            IAuditLogRepository auditLogRepository)
        {
            _bidRepository = bidRepository;
            _aucRepository = aucRepository;
            _walletRepository = walletRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _auctionNotifier = auctionNotifier;
            _logger = logger;
            _transactionLedgerRepository = transactionLedgerRepository;
            _auditLogRepository = auditLogRepository;
        }

        public async Task<int> HandleAsync(CreateBidCommand command)
        {
            var auction = await _aucRepository.GetByIdTrackingAsync(command.AuctionId)
                ?? throw new NotFoundException("La subasta no existe.");

            if (auction.Status != AuctionConstants.Active)
                throw new ValidationException("La subasta no esta activa.");

            if (auction.EndDate <= DateTime.UtcNow)
                throw new ConflictException("La subasta ya finalizó");

            if (auction.SellerId == command.BuyerId)
                throw new ValidationException("El vendedor no puede pujar en su propia subasta");

            if(!await _userRepository.ExistsByIdAsync(command.BuyerId))
                throw new NotFoundException("El comprador no existe");

            var highestBid = await _bidRepository.GetHighestBidByAuctionIdAsync(auction.Id);
            var currentAmount = highestBid?.Amount ?? auction.BasePrice;
            var minimunValidAmount = currentAmount + auction.MinimumIncrement;

            if (command.Amount < minimunValidAmount)
                throw new ValidationException($"El monto debe ser al menos {minimunValidAmount}");

            var wallet = await _walletRepository.GetByUserIdTrackedAsync(command.BuyerId)
                ?? throw new NotFoundException("No se encontro la billetera");

            if (wallet.AvailableBalance < command.Amount)
                throw new InsufficientFundsException("Saldo insuficientes");

            await _unitOfWork.BeginTransactionAsync();

            try //escrow.
            {
                if(highestBid != null)
                {
                    var lastWallet = await _walletRepository.GetByUserIdTrackedAsync(highestBid.BuyerId)
                        ?? throw new NotFoundException("No se encontró la billetera");

                    lastWallet.HeldBalance -= highestBid.Amount;

                    var ledgerRelease = new TransactionLedger
                    {
                        WalletId = lastWallet.Id,
                        Type = TransactionConstants.Release,
                        Amount = highestBid.Amount,
                        CreatedAt = DateTime.UtcNow,
                        AuctionId = command.AuctionId,
                    };

                    await _transactionLedgerRepository.AddAsync(ledgerRelease);
                    await _walletRepository.UpdateAsync(lastWallet); //Sacar
                }

                wallet.HeldBalance += command.Amount;
                await _walletRepository.UpdateAsync(wallet); //Sacar


                var newBid = new Bid
                {
                    AuctionId = command.AuctionId,
                    BuyerId = command.BuyerId,
                    Amount = command.Amount,
                    CreatedAt = DateTime.UtcNow
                };

                await _bidRepository.AddAsync(newBid);

                auction.CurrentBidAmount = command.Amount;

                var ledgerRetention = new TransactionLedger
                {
                    WalletId = wallet.Id,
                    Type = TransactionConstants.Retention,
                    Amount = command.Amount,
                    CreatedAt = DateTime.UtcNow,
                    AuctionId = command.AuctionId
                };

                await _transactionLedgerRepository.AddAsync(ledgerRetention);

                //Anti-sniping
                var timeRemaining = auction.EndDate - DateTime.UtcNow; // Es un intervalo de tiempo.
                bool wasExtended = false;
                if (timeRemaining.TotalSeconds <= 60)
                {
                    wasExtended = true;
                    auction.EndDate = auction.EndDate.AddMinutes(2);

                    var auditAddTime = new AuditLog
                    {
                        Entity = "SUBASTA",
                        EntityId = auction.Id,
                        Action = "extension_tiempo",
                        UserId = null,
                        DetailJson = System.Text.Json.JsonSerializer.Serialize(new { newEndate = auction.EndDate, initialEndDate = auction.EndDate.AddMinutes(-2)}),
                        CreatedAt = DateTime.UtcNow
                    };
                    await _auditLogRepository.AddAsync(auditAddTime);
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                try
                {
                    if (wasExtended)
                        await _auctionNotifier.NotifyAuctionExtendedAsync(auction.Id, auction.EndDate);

                    var payload = new NewBidPayload
                    {
                        AuctionId = auction.Id,
                        Amount = command.Amount,
                        BuyerId = command.BuyerId,
                        Alias = AliasGenerator.Generate(auction.Id, command.BuyerId),
                        CreatedAt = DateTime.UtcNow,
                        TotalBids = auction.Bids.Count
                    };

                    await _auctionNotifier.NotifyNewBidAsync(auction.Id, payload);

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Hubo un error al notificar.");
                }

                return newBid.Id;

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                _unitOfWork.ClearTracking();

                try
                {
                    var auditBid = new AuditLog
                    {
                        Entity = "SUBASTA",
                        EntityId = auction.Id,
                        Action = "puja_rechazada",
                        UserId = command.BuyerId,
                        DetailJson = System.Text.Json.JsonSerializer.Serialize(new { detail = ex.Message }),
                        CreatedAt = DateTime.UtcNow
                    };

                    await _auditLogRepository.AddAsync(auditBid);
                    await _unitOfWork.SaveChangesAsync();
                }
                catch(Exception logEx)
                {
                    _logger.LogError(logEx, "No se pudo registrar el AuditLog de puja rechazada.");
                }
                throw;
            }
        }
    }
}