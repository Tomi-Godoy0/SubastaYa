namespace SubastaYa.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;

        //-------
        public ICollection<Auction> Auctions { get; set; } = [];
    }
}