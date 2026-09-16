using System.ComponentModel.DataAnnotations;

namespace SubastaYa.Application.UseCases.Wallets.DepositWallet
{
    public class DepositCommand
    {
        [Range(1, int.MaxValue, ErrorMessage = "Debe especificar un usuario válido")]
        public int UserId { get; set; }
        [Range(1, 9999999.99)]
        public decimal Amount { get; set; }
    }
}
