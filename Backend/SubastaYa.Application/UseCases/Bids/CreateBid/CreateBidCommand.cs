using System;
using System.ComponentModel.DataAnnotations;

namespace SubastaYa.Application.UseCases.Bids.CreateBid
{
    public class CreateBidCommand
    {
        public int AuctionId { get; set; }
        [Required]
        public int BuyerId { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        public decimal Amount { get; set; }
    }
}