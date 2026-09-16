namespace SubastaYa.Domain.Entities
{
    public class Wallet
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal HeldBalance { get; set; }
        public decimal AvailableBalance => TotalBalance - HeldBalance;
        public byte[] Version { get; set; } = null!;

        //------------------

        public User User { get; set; } = null!;
        public ICollection<TransactionLedger> TransactionLedgers { get; set; } = [];
    }
}
