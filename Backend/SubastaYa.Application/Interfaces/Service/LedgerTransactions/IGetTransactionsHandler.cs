using SubastaYa.Application.UseCases.LedgerTransactions.GetTransactions;
using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.Interfaces.Service.LedgerTransactions
{
    public interface IGetTransactionsHandler
    {
        Task<IEnumerable<TransactionLedger>> HandleAsync(
            GetTransactionsQuery query);
    }
}