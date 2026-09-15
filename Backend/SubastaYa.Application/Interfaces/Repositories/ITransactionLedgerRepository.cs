using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.Interfaces.Repositories
{
    public interface ITransactionLedgerRepository
    {
        Task<TransactionLedger> AddAsync(TransactionLedger transaction);
        Task<(List<TransactionLedger> Transactions, int TotalCount)> GetByWalletIdAsync( int walletId, int pageNumber, int pageSize);
        Task<TransactionLedger?> GetByIdAsync(int id);
    }
}