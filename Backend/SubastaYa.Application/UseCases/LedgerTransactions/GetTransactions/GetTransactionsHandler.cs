using SubastaYa.Application.Common;
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

        public async Task<PagedResult<TransactionResponse>> HandleAsync(GetTransactionsQuery query)
        {
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