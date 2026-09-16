using SubastaYa.Application.Interfaces.DTOs;

namespace SubastaYa.Application.Interfaces.Service.Worker
{
    public interface IAuctionClosingService
    {
        public Task<AuctionResult> HandleAsync();
    }
}
