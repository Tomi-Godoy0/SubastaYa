using System;
using System.ComponentModel.DataAnnotations;

namespace SubastaYa.Application.UseCases.LedgerTransactions.CreateTransaction
{
    public  class CreateTransactionCommand
    {
        [Range(1, int.MaxValue, ErrorMessage = "Debe especificar una billetera válida")]
        public int WalletId { get; set; }
        [Required]
        public string Type { get; set; } = string.Empty;
        [Range(0.01, 9999999.99)]
        public decimal Amount { get; set; }
        public int? AuctionId { get; set; }
    }
}