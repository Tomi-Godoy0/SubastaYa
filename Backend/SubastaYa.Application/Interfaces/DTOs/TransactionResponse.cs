namespace SubastaYa.Application.Interfaces.DTOs
{
    public class TransactionResponse
    {
        public int Id { get; set; }

        public int WalletId { get; set; }

        public string Type { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? AuctionId { get; set; }
    }
}