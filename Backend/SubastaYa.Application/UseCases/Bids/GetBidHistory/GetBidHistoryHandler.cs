using SubastaYa.Application.Common;
using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Service.Bids;

namespace SubastaYa.Application.UseCases.Bids.GetBidHistory
{
    public class GetBidHistoryHandler : IGetBidHistoryHandler
    {
        private readonly IBidRepository _bidRepository;

        public GetBidHistoryHandler(IBidRepository bidRepository)
        {
            _bidRepository = bidRepository;
        }

        public async Task<PagedResult<BidHistoryResponse>> HandleAsync(GetBidHistoryQuery query)
        {
            var (bids, totalCount) = await _bidRepository.GetByAuctionIdAsync(query.AuctionId, query.PageNumber, query.PageSize);

            var items = bids.Select(b => new BidHistoryResponse
            {
                Alias = AliasGenerator.Generate(b.AuctionId, b.BuyerId),
                Amount = b.Amount,
                CreatedAt = b.CreatedAt
            }).ToList();

            return new PagedResult<BidHistoryResponse>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
        }
    }
}
