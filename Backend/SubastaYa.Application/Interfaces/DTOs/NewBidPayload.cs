namespace SubastaYa.Application.Interfaces.DTOs
{
    public class NewBidPayload
    {
        public int AuctionId { get; set; }
        public decimal Amount { get; set; }
        public int BuyerId { get; set; }
        public string Alias { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int TotalBids { get; set; }
    }
}
