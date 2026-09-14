namespace SubastaYa.Application.UseCases.LedgerTransactions.GetTransactions
{
    public class GetTransactionsQuery
    {
        public int WalletId { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}