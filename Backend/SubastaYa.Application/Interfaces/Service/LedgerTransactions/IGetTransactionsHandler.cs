using SubastaYa.Application.Common;
using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.UseCases.LedgerTransactions.GetTransactions;

namespace SubastaYa.Application.Interfaces.Service.LedgerTransactions
{
    public interface IGetTransactionsHandler
    {
        Task<PagedResult<TransactionResponse>> HandleAsync(
            GetTransactionsQuery query);
    }
}