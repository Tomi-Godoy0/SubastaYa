using SubastaYa.Application.Common;
using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Service.Users;
using SubastaYa.Domain.Constants;

namespace SubastaYa.Application.UseCases.Users.GetMyBids
{
    public class GetUserBidsHandler : IGetUserBidsHandler
    {
        private readonly IBidRepository _bidRepository;

        public GetUserBidsHandler(IBidRepository bidRepository)
        {
            _bidRepository = bidRepository;
        }

        public async Task<PagedResult<UserBidsResponse>> HandleAsync(GetUserBidsQuery query)
        {
            var (bid, totalCount) = await _bidRepository.GetByBuyerIdAsync(query.BuyerId, query.PageNumber, query.PageSize);

            var items = bid.Select(b => new UserBidsResponse
            {
                Id = b.AuctionId,
                Title = b.Auction.Title,
                CurrentBidAmount = b.Auction.CurrentBidAmount,
                MyBidAmount = b.Amount,
                EndDate = b.Auction.EndDate,
                Status = b.Auction.Status,
                Won = b.Auction.Status == AuctionConstants.Finished && b.Amount == b.Auction.CurrentBidAmount
            }).ToList();

            return new PagedResult<UserBidsResponse>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
        }
    }
}
