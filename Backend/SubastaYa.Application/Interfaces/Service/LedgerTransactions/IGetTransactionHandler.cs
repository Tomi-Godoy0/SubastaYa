using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.UseCases.LedgerTransactions.GetTransaction;

namespace SubastaYa.Application.Interfaces.Service.LedgerTransactions
{
    public interface IGetTransactionHandler
    {
        Task<TransactionResponse> HandleAsync(GetTransactionQuery query);
    }
}