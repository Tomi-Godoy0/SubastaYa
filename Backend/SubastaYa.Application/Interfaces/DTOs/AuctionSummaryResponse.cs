namespace SubastaYa.Application.Interfaces.DTOs
{
    public class AuctionSummaryResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public decimal CurrentBidAmount { get; set; }
        public int TotalBids { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;

    }
}