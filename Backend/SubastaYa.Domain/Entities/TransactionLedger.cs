using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Domain.Entities
{
    public class TransactionLedger
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public string Type { get; set; } = string.Empty; // Deposito, Retención, Liberación, Pago, Cobro
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? AuctionId { get; set; } // Referencia a la subasta asociada 

        //------
        public Wallet Wallet { get; set; } = null!;
        public Auction? Auction { get; set; }
    }
}
