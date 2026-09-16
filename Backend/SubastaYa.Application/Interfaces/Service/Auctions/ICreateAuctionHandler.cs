using SubastaYa.Application.UseCases.Auctions.CreateAuction;

namespace SubastaYa.Application.Interfaces.Service.Auctions
{
    public interface ICreateAuctionHandler
    {
        public Task<int> HandleAsync(CreateAuctionCommand command);
    }
}