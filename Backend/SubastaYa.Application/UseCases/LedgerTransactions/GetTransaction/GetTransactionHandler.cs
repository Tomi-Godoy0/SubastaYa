using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Service.LedgerTransactions;
using SubastaYa.Domain.Exceptions;

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

        public async Task<TransactionResponse> HandleAsync(
            GetTransactionQuery query)
        {

            var transaction = await _transactionRepository.GetByIdAsync(query.Id)
                ?? throw new NotFoundException($"No se encontro la transacción {query.Id}");

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