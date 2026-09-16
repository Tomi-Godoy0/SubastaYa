namespace SubastaYa.Application.Interfaces.DTOs
{
    public class WalletResponse
    {
        public decimal TotalBalance { get; set; }
        public decimal HeldBalance { get; set; }
        public decimal AvailableBalance { get; set; }
    }
}
