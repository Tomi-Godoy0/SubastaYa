namespace SubastaYa.Application.Interfaces.DTOs
{
    public class UserBidsResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal CurrentBidAmount { get; set; }
        public decimal MyBidAmount { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool Won { get; set; }
    }
}
