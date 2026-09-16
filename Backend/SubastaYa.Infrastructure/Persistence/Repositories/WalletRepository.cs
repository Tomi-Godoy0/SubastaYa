using Microsoft.EntityFrameworkCore;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Domain.Entities;

namespace SubastaYa.Infrastructure.Persistence.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly AppDbContext _context;

        public WalletRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Wallet> AddAsync(Wallet wallet)
        {
            await _context.Wallets.AddAsync(wallet);

            return wallet;
        }

        public async Task<Wallet?> GetByUserIdAsync(int id)
        {
            return await _context.Wallets.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == id);
        }
        public async Task<Wallet?> GetByUserIdTrackedAsync(int id)
        {
            return await _context.Wallets.FirstOrDefaultAsync(x => x.UserId == id);
        }
    }
}
