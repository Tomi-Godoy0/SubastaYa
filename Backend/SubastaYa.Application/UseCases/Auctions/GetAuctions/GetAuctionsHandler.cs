using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Service.Auctions;
using SubastaYa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.UseCases.Auctions.GetAuctions
{
    public class GetAuctionsHandler : IGetAuctionsHandler
    {
        private readonly IAuctionRepository _auctionRepository;

        public GetAuctionsHandler(IAuctionRepository auctionRepository)
        {
            _auctionRepository = auctionRepository;
        }

        public async Task<PagedResult<AuctionSummaryResponse>> HandleAsync(GetAuctionsQuery query)
        {

            var filter = new AuctionFilter
            {
                Title = query.Title,
                CategoryId = query.CategoryId,
                Status = query.Status,
                MinPrice = query.MinPrice,
                MaxPrice = query.MaxPrice,
                OrderBy = query.OrderBy
            };

            var (auctions, totalCount) = await _auctionRepository.GetFilterAsync(filter, query.PageNumber, query.PageSize);

            var items = auctions.Select(a => new AuctionSummaryResponse
            {
                Id = a.Id,
                Title = a.Title,
                ImageUrl = a.ImageUrl,
                CategoryName = a.Category.Name,
                CurrentBidAmount = a.Bids.Count != 0 ? a.Bids.Max(b => b.Amount) : a.BasePrice,
                TotalBids = a.Bids.Count,
                EndDate = a.EndDate,
                Status = a.Status
            }).ToList();


            return new PagedResult<AuctionSummaryResponse>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
        }
    }
}
