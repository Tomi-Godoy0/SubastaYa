using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.UseCases.Auctions.GetAuctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.Interfaces.Service.Auctions
{
    public interface IGetAuctionsHandler
    {
        public Task<PagedResult<AuctionSummaryResponse>> HandleAsync(GetAuctionsQuery query);
    }
}
