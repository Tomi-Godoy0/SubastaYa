using SubastaYa.Application.Common;
using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.UseCases.Bids.GetBidHistory;

namespace SubastaYa.Application.Interfaces.Service.Bids
{
    public interface IGetBidHistoryHandler
    {
        public Task<PagedResult<BidHistoryResponse>> HandleAsync(GetBidHistoryQuery query);
    }
}
