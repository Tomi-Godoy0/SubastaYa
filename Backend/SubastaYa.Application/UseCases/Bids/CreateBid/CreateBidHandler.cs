using SubastaYa.Application.Interfaces;
using SubastaYa.Domain.Exceptions;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Service.Bids;
using SubastaYa.Domain.Entities;
using SubastaYa.Domain.Constants;
using System.Data;

namespace SubastaYa.Application.UseCases.Bids.CreateBid
{
    public class CreateBidHandler : ICreateBidHandler
    {
        private readonly IBidRepository _bidRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAuctionRepository _aucRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBidHandler(IBidRepository bidRepository, IAuctionRepository aucRepository, IWalletRepository walletRepository, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _bidRepository = bidRepository;
            _aucRepository = aucRepository;
            _walletRepository = walletRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task<int> HandleAsync(CreateBidCommand command)
        {
            var auction = await _aucRepository.GetByIdTrackingAsync(command.AuctionId)
                ?? throw new NotFoundException("La subasta no existe.");

            if (auction.Status != AuctionConstants.Active)
                throw new ValidationException("La subasta no esta activa.");

            if (auction.EndDate <= DateTime.UtcNow)
                throw new ConflictException("La subasta ya finalizó");

            if(!await _userRepository.ExistsByIdAsync(command.BuyerId))
                throw new NotFoundException("El comprador no existe");

            var highestBid = await _bidRepository.GetHighestBidByAuctionIdAsync(auction.Id);
            var currentAmount = highestBid?.Amount ?? auction.BasePrice; // si no hubo pujas, vamos a usar el precio base
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

                    await _walletRepository.UpdateAsync(lastWallet);
                }

                wallet.HeldBalance += command.Amount;
                await _walletRepository.UpdateAsync(wallet);


                var newBid = new Bid
                {
                    AuctionId = command.AuctionId,
                    BuyerId = command.BuyerId,
                    Amount = command.Amount,
                    CreatedAt = DateTime.UtcNow
                };

                await _bidRepository.AddAsync(newBid);

                //Pendiente registrar movimientos en Transaction.

                //Anti-sniping
                var timeRemaining = auction.EndDate - DateTime.UtcNow; // Es un intervalo de tiempo.
                if (timeRemaining.TotalSeconds <= 60)
                {
                    auction.EndDate = auction.EndDate.AddMinutes(2);
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                return newBid.Id;

            }
            catch (DBConcurrencyException ex)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}