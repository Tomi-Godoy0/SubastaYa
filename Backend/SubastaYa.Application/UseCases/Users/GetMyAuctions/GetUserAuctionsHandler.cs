using SubastaYa.Application.Common;
using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Service.Users;
using SubastaYa.Domain.Exceptions;

namespace SubastaYa.Application.UseCases.Users.GetMyAuctions
{
    public class GetUserAuctionsHandler : IGetUserAuctionsHandler
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IUserRepository _userRepository;

        public GetUserAuctionsHandler(IAuctionRepository auctionRepository, IUserRepository userRepository)
        {
            _auctionRepository = auctionRepository;
            _userRepository = userRepository;
        }

        public async Task<PagedResult<UserAuctionsResponse>> HandleAsync(GetUserAuctionsQuery query)
        {
            var userExists = await _userRepository.ExistsByIdAsync(query.SellerId);

             if(!userExists)
                throw new NotFoundException("El vendedor solicitado no existe");
            
            var (auctions, totalCount) = await _auctionRepository.GetBySellerIdAsync(query.SellerId, query.PageNumber, query.PageSize);

            var items = auctions.Select(a => new UserAuctionsResponse
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                ImageUrl = a.ImageUrl,
                CategoryName = a.Category.Name,
                BasePrice = a.BasePrice,
                CurrentBidAmount = a.CurrentBidAmount,
                TotalBids = a.Bids.Count,
                EndDate = a.EndDate,
                Status = a.Status
            }).ToList();

            return new PagedResult<UserAuctionsResponse>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
        }
    }
}
