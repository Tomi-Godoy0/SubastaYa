using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Service.LedgerTransactions;
using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.UseCases.LedgerTransactions.GetTransactions
{
    public class GetTransactionsHandler : IGetTransactionsHandler
    {
        private readonly ITransactionLedgerRepository _transactionRepository;

        public GetTransactionsHandler(
            ITransactionLedgerRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<IEnumerable<TransactionLedger>> HandleAsync(
            GetTransactionsQuery query)
        {
            if (query.WalletId <= 0)
                throw new ArgumentException("La billetera no es válida.");

            return await _transactionRepository
                .GetByWalletIdAsync(query.WalletId);
        }
    }
}