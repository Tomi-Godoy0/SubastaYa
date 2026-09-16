namespace SubastaYa.Application.Interfaces.DTOs
{
    public class BidHistoryResponse
    {
        public decimal Amount { get; set; }
        public string Alias { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
