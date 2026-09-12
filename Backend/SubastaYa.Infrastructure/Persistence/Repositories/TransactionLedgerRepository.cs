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
            await _context.SaveChangesAsync();

            return transaction;
        }

        public async Task<IEnumerable<TransactionLedger>> GetByWalletIdAsync(
            int walletId)
        {
            return await _context.TransactionLedgers
                .Where(t => t.WalletId == walletId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<TransactionLedger?> GetByIdAsync(int id)
        {
            return await _context.TransactionLedgers
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}