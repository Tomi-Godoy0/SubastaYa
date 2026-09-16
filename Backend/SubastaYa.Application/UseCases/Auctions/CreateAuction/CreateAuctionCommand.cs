using System.ComponentModel.DataAnnotations;

namespace SubastaYa.Application.UseCases.Auctions.CreateAuction
{
    public class CreateAuctionCommand
    {
        [Range(1, int.MaxValue, ErrorMessage = "Debe especificar un vendedor válido")]
        public int SellerId { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Range(1, int.MaxValue, ErrorMessage = "Debe especificar una categoría válida")]
        public int CategoryId { get; set; }
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string ImageUrl { get; set; } = string.Empty;
        [Range(1, 9999999.99)]
        public decimal BasePrice { get; set; }
        [Range(1, 9999999.99)]
        public decimal MinimumIncrement { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; }
    } 
}
