using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.Interfaces.DTOs
{
    public class AuctionResult
    {
        public int TotalProcessed { get; set; }
        public int AuctionsFinalized { get; set; }
        public int AuctionsDeserted { get; set; }
    }
}
