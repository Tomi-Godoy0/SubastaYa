using Microsoft.EntityFrameworkCore;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Task<Wallet> UpdateAsync(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            

            return Task.FromResult(wallet);
        }

        public async Task<Wallet?> GetByIdAsync(int id)
        {
            return await _context.Wallets.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Wallet?> GetByUserIdAsync(int id)
        {
            return await _context.Wallets.FirstOrDefaultAsync(x => x.UserId == id);
        }
    }
}
