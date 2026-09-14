using Microsoft.EntityFrameworkCore;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Domain.Entities;

namespace SubastaYa.Infrastructure.Persistence.Repositories
{
    public class TransactionLedgerRepository : ITransactionLedgerRepository
    {
        private readonly AppDbContext _context;

        public TransactionLedgerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TransactionLedger> AddAsync(
            TransactionLedger transaction)
        {
            await _context.TransactionLedgers.AddAsync(transaction);

            return transaction;
        }

        public async Task<(
            IEnumerable<TransactionLedger> Transactions,
            int TotalCount)> GetByWalletIdAsync(
                int walletId,
                int pageNumber,
                int pageSize)
        {
            var query = _context.TransactionLedgers
                .AsNoTracking()
                .Where(t => t.WalletId == walletId)
                .OrderByDescending(t => t.CreatedAt);

            var totalCount = await query.CountAsync();

            var transactions = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (transactions, totalCount);
        }

        public async Task<TransactionLedger?> GetByIdAsync(int id)
        {
            return await _context.TransactionLedgers
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}