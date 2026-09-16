namespace SubastaYa.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        //-------
        public Wallet? Wallet { get; set; }
        public ICollection<Auction> Auctions { get; set; } = [];
        public ICollection<Bid> Bids { get; set; } = [];
        public ICollection<AuditLog> AuditLogs { get; set; } = [];
    }
}