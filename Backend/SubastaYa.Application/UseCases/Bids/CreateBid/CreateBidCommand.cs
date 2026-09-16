using System.ComponentModel.DataAnnotations;

namespace SubastaYa.Application.UseCases.Bids.CreateBid
{
    public class CreateBidCommand
    {
        [Range(1, int.MaxValue, ErrorMessage = "Debe especificar una subasta válida")]
        public int AuctionId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Debe especificar un comprador válido")]
        public int BuyerId { get; set; }
        [Range(1, 9999999.99, ErrorMessage = "El monto debe ser mayor a 0")]
        public decimal Amount { get; set; }
    }
}