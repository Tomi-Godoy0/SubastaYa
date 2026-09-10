using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.UseCases.LedgerTransactions.RegisterTransaction
{
    internal class RegisterTransactionCommand
    {
        public int WalletId { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? AuctionId { get; set; }
    }
}