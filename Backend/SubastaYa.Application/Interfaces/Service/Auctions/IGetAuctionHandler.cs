using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.UseCases.Auctions.GetAuction;

namespace SubastaYa.Application.Interfaces.Service.Auctions
{
    public interface IGetAuctionHandler
    {
        public Task<AuctionResponse> HandleAsync(GetAuctionQuery query);
    }
}