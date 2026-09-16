using Microsoft.Extensions.Logging;
using SubastaYa.Application.Interfaces;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Service.Worker;
using SubastaYa.Domain.Constants;

namespace SubastaYa.Application.UseCases.Worker
{
    public class AuctionStartingService : IAuctionStartingService
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AuctionStartingService> _logger;

        public AuctionStartingService(IAuctionRepository auctionRepository, IUnitOfWork unitOfWork, ILogger<AuctionStartingService> logger)
        {
            _auctionRepository = auctionRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<int> HandleAsync()
        {
            var readyAuctions = await _auctionRepository.GetReadyToStartAsync(DateTime.UtcNow);

            var startedCount = 0;

            foreach (var auction in readyAuctions)
            {
                try
                {
                    auction.Status = AuctionConstants.Active;
                    await _unitOfWork.SaveChangesAsync();
                    startedCount++;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al activar la subasta {AuctionId}", auction.Id);
                }
            }

            return startedCount;
        }
    }
}
