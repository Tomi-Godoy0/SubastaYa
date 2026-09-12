using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.Interfaces.DTOs
{
    public class AuctionSummaryResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string CategoryName { get; set; }
        public decimal CurrentBidAmount { get; set; }
        public int TotalBids { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }

    }
}
