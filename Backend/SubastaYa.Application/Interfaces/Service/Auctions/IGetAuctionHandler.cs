using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.UseCases.Auctions.GetAuction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.Interfaces.Service.Auctions
{
    public interface IGetAuctionHandler
    {
        public Task<AuctionResponse> HandleAsync(GetAuctionQuery query);
    }
}
