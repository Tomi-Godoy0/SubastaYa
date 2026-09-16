using SubastaYa.Application.Common;
using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.UseCases.Auctions.GetAuctions;

namespace SubastaYa.Application.Interfaces.Service.Auctions
{
    public interface IGetAuctionsHandler
    {
        public Task<PagedResult<AuctionSummaryResponse>> HandleAsync(GetAuctionsQuery query);
    }
}