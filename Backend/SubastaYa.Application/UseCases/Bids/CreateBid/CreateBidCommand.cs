using System;

namespace SubastaYa.Application.UseCases.Bids.CreateBid
{
    public class CreateBidCommand
    {
        public int AuctionId { get; set; }
        public int BuyerId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}