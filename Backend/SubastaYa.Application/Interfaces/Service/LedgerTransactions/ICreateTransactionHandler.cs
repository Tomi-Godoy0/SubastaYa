using SubastaYa.Application.UseCases.LedgerTransactions.CreateTransaction;
using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.Interfaces.Service.LedgerTransactions
{
    public interface ICreateTransactionHandler
    {
        Task<TransactionLedger> HandleAsync(
            CreateTransactionCommand command);
    }
}