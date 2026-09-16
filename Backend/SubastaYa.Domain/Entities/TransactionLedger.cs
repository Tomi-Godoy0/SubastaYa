namespace SubastaYa.Domain.Entities
{
    public class TransactionLedger
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? AuctionId { get; set; }

        //------
        public Wallet Wallet { get; set; } = null!;
        public Auction? Auction { get; set; }
    }
}
