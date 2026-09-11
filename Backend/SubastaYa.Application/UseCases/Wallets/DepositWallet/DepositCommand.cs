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

        [Required(ErrorMessage = "El monto a ingresar es obligatorio")]
        [Range(0.01, 9999999.99, ErrorMessage = "El monto debe ser mayor a cero")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "El monto no puede tener más de 2 decimales")]
        public decimal Amount { get; set; }
    }
}
