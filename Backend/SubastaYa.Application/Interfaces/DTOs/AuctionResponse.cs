namespace SubastaYa.Application.Interfaces.DTOs
{
    public class AuctionResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public decimal MinimumIncrement { get; set; }
        public decimal CurrentBidAmount { get; set; }
        public int TotalBids { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string SellerName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
    }
}