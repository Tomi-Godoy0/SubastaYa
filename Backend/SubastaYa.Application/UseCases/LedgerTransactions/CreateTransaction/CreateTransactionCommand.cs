using System;

namespace SubastaYa.Application.UseCases.LedgerTransactions.CreateTransaction
{
    public  class CreateTransactionCommand
    {
        public int WalletId { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? AuctionId { get; set; }
    }
}