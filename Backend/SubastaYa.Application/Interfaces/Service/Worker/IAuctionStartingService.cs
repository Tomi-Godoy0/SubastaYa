using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.Interfaces.Service.Worker
{
    public interface IAuctionStartingService
    {
        public Task<int> HandleAsync();
    }
}
