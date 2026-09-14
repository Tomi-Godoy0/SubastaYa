using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.Interfaces.Repositories
{
    public interface ITransactionLedgerRepository
    {
        Task<TransactionLedger> AddAsync(TransactionLedger transaction);

        Task<IEnumerable<TransactionLedger>> GetByWalletIdAsync(int walletId);

        Task<TransactionLedger?> GetByIdAsync(int id);
    }
}