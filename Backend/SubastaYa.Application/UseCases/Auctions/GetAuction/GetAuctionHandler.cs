using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Service.Auctions;
using SubastaYa.Domain.Exceptions;

namespace SubastaYa.Application.UseCases.Auctions.GetAuction
{
    public class GetAuctionHandler : IGetAuctionHandler
    {
        private readonly IAuctionRepository _auctionRepository;

        public GetAuctionHandler(IAuctionRepository auctionRepository)
        {
            _auctionRepository = auctionRepository;
        }

        public async Task<AuctionResponse> HandleAsync(GetAuctionQuery query)
        {
            var auction = await _auctionRepository.GetByIdWithBidsAsync(query.AuctionId)
                ?? throw new NotFoundException($"Subasta {query.AuctionId} no encontrada.");

            return new AuctionResponse
            {
                Id = auction.Id,
                Title = auction.Title,
                Description = auction.Description,
                ImageUrl = auction.ImageUrl,
                BasePrice = auction.BasePrice,
                MinimumIncrement = auction.MinimumIncrement,
                CurrentBidAmount = auction.CurrentBidAmount,
                TotalBids = auction.Bids.Count,
                StartDate = auction.StartDate,
                EndDate = auction.EndDate,
                Status = auction.Status,
                SellerName = auction.Seller.Name,
                CategoryName = auction.Category.Name
            };
        }
    }
}
