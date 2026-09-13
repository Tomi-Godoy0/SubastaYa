using SubastaYa.Application.UseCases.Bids.CreateBid;
using SubastaYa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.Interfaces.Service.Bids
{
    public interface ICreateBidHandler
    {
        public Task<int> HandleAsync(CreateBidCommand command);
    }
}
