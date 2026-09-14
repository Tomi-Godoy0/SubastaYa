using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.Interfaces.DTOs
{
    public class NewBidPayload
    {
        public int AuctionId { get; set; }
        public decimal Amount { get; set; }
        public int BuyerId { get; set; }
        public string? Alias { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TotalBids { get; set; }
    }
}
