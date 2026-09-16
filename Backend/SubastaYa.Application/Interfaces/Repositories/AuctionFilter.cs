namespace SubastaYa.Application.Interfaces.Repositories
{
    public class AuctionFilter
    {
        public string? Title { get; set; }
        public int? CategoryId { get; set; }
        public string? Status { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? OrderBy { get; set; }
    }
}
