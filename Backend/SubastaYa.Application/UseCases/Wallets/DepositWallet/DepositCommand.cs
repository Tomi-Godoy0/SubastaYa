using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.UseCases.Wallets.DepositWallet
{
    public class DepositCommand
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
    }
}
