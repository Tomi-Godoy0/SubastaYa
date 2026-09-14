using SubastaYa.Application.UseCases.LedgerTransactions.RegisterTransaction;
using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.Interfaces.Service.LedgerTransactions
{
    public interface IRegisterTransactionHandler
    {
        Task<TransactionLedger> HandleAsync(
            RegisterTransactionCommand command);
    }
}