using System.ComponentModel.DataAnnotations;

namespace SubastaYa.Application.UseCases.Bids.GetBidHistory
{
    public class GetBidHistoryQuery
    {
        public int AuctionId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "El número de página debe ser mayor a cero")]
        public int PageNumber { get; set; } = 1;
        [Range(1, 100, ErrorMessage = "El tamaño de página debe estar entre 1 y 100")]
        public int PageSize { get; set; } = 10;
    }
}
