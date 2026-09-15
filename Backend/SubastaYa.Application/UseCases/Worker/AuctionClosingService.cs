using Microsoft.Extensions.Logging;
using SubastaYa.Application.Interfaces;
using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Service.Worker;
using SubastaYa.Domain.Constants;
using SubastaYa.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.UseCases.Worker
{
    public class AuctionClosingService : IAuctionClosingService
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IBidRepository _bidRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AuctionClosingService> _logger;
        //Falta el Ledger 
        //Falta audit

        public AuctionClosingService(IAuctionRepository auctionRepository, IBidRepository bidRepository, IWalletRepository walletRepository, IUnitOfWork unitOfWork, ILogger<AuctionClosingService> logger)
        {
            _auctionRepository = auctionRepository;
            _bidRepository = bidRepository;
            _walletRepository = walletRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<AuctionResult> HandleAsync()
        {
            var expiredAuctions = await _auctionRepository.GetExpiredAsync(DateTime.UtcNow);

            var result = new AuctionResult();

            foreach (var auction in expiredAuctions)
            {
                await _unitOfWork.BeginTransactionAsync();

                try
                {
                    var hasBids = await _bidRepository.ExistsByAuctionIdAsync(auction.Id);
                    result.TotalProcessed++;

                    if (hasBids)
                    {
                        result.AuctionsFinalized++;
                        auction.Status = AuctionConstants.Finished;

                        var highestBid = await _bidRepository.GetHighestBidByAuctionIdAsync(auction.Id)
                        ?? throw new InvalidOperationException($"Se esperaba una puja para la subasta {auction.Id} pero no se encontró.");

                        var buyerWallet = await _walletRepository.GetByUserIdTrackedAsync(highestBid.BuyerId)
                            ?? throw new NotFoundException("Billetera del comprador no encontrada.");

                        var sellerWallet = await _walletRepository.GetByUserIdTrackedAsync(auction.SellerId)
                            ?? throw new NotFoundException("Billetera del vendedor no encontrada.");

                        buyerWallet.TotalBalance -= highestBid.Amount;
                        buyerWallet.HeldBalance -= highestBid.Amount;

                        sellerWallet.TotalBalance += highestBid.Amount;

                    }else
                    {
                        result.AuctionsDeserted++;
                        auction.Status = AuctionConstants.Deserted;

                        //Recordatorio: Registrar en AuditLog (Action: "desierta_worker", UserId: null, EntityId: auction.Id)
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitAsync();

                }catch (Exception ex)
                {
                    await _unitOfWork.RollbackAsync();
                    _logger.LogError(ex, "Error al procesar el cierre de la subasta {AuctionId}", auction.Id);
                }
            }

            return result;
        }
    }
}
