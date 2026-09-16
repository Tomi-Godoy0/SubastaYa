using System.ComponentModel.DataAnnotations;

namespace SubastaYa.Application.Interfaces.DTOs
{
    public class DepositRequest
    {
        [Required(ErrorMessage = "El monto a ingresar es obligatorio")]
        [Range(0.01, 9999999.99, ErrorMessage = "El monto debe ser mayor a cero")]
        public decimal Amount { get; set; }
    }
}
