using SubastaYa.Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.UseCases.Auctions.CreateAuction
{
    public class CreateAuctionCommand
    {
        [Required]
        public int SellerId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        [Range(1, 9999999.99)]
        public decimal BasePrice { get; set; }
        [Required]
        [Range(1, 9999999.99)]
        public decimal MinimumIncrement { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; }
    } 
}
