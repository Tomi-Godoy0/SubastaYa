using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.UseCases.Auctions.GetAuctions
{
    public class GetAuctionsQuery
    {
        public string? Title { get; set; }
        public int? CategoryId {  get; set; }
        public string? Status { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? OrderBy { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "El número de página debe ser mayor a cero")]
        public int PageNumber { get; set; } = 1;
        [Range(1, 100, ErrorMessage = "El tamaño de página debe estar entre 1 y 100")]
        public int PageSize { get; set; } = 10;
    }
}
