using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Service.LedgerTransactions;

namespace SubastaYa.Application.UseCases.LedgerTransactions.GetTransaction
{
    public class GetTransactionHandler : IGetTransactionHandler
    {
        private readonly ITransactionLedgerRepository _transactionRepository;

        public GetTransactionHandler(
            ITransactionLedgerRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<TransactionResponse?> HandleAsync(
            GetTransactionQuery query)
        {
            if (query.Id <= 0)
                throw new ArgumentException(
                    "El Id de transacción no es válido.");

            var transaction = await _transactionRepository
                .GetByIdAsync(query.Id);

            if (transaction == null)
                return null;

            return new TransactionResponse
            {
                Id = transaction.Id,
                WalletId = transaction.WalletId,
                Type = transaction.Type,
                Amount = transaction.Amount,
                CreatedAt = transaction.CreatedAt,
                AuctionId = transaction.AuctionId
            };
        }
    }
}