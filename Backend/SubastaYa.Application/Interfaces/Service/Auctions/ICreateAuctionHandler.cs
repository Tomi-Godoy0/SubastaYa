using SubastaYa.Application.UseCases.Auctions.CreateAuction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.Interfaces.Service.Auctions
{
    public interface ICreateAuctionHandler
    {
        public Task<int> HandleAsync(CreateAuctionCommand command);
    }
}
