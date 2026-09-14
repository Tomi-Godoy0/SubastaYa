using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Service.LedgerTransactions;

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

        public async Task<PagedResult<TransactionResponse>> HandleAsync(
            GetTransactionsQuery query)
        {
            if (query.WalletId <= 0)
                throw new ArgumentException(
                    "La billetera no es válida.");

            if (query.PageNumber <= 0)
                throw new ArgumentException(
                    "El número de página debe ser mayor a cero.");

            if (query.PageSize <= 0)
                throw new ArgumentException(
                    "El tamaño de página debe ser mayor a cero.");

            var (transactions, totalCount) =
                await _transactionRepository.GetByWalletIdAsync(
                    query.WalletId,
                    query.PageNumber,
                    query.PageSize);

            var items = transactions.Select(t => new TransactionResponse
            {
                Id = t.Id,
                WalletId = t.WalletId,
                Type = t.Type,
                Amount = t.Amount,
                CreatedAt = t.CreatedAt,
                AuctionId = t.AuctionId
            }).ToList();

            return new PagedResult<TransactionResponse>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
        }
    }
}