using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Service.Auctions;
using SubastaYa.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var auction = await _auctionRepository.GetByIdWithBidsAsync(query.AuctionId);

            if(auction == null)
                throw new NotFoundException($"Subasta {query.AuctionId} no encontrada.");

            var currentBid = auction.Bids.Count != 0 ? auction.Bids.Max(b => b.Amount) : auction.BasePrice;


            return new AuctionResponse
            {
                Id = auction.Id,
                Title = auction.Title,
                Description = auction.Description,
                ImageUrl = auction.ImageUrl,
                BasePrice = auction.BasePrice,
                MinimumIncrement = auction.MinimumIncrement,
                CurrentBidAmount = currentBid,
                TotalBids = auction.Bids.Count,
                EndDate = auction.EndDate,
                Status = auction.Status,
                SellerName = auction.Seller.Name,
                CategoryName = auction.Category.Name
            };
        }
    }
}
