using SubastaYa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.Interfaces.Repositories
{
    public interface IWalletRepository
    {
        public Task<Wallet> AddAsync(Wallet wallet);
        public Task<Wallet> UpdateAsync(Wallet wallet);

        //Queries
        public Task<Wallet?> GetByUserIdAsync(int id);
        public Task<Wallet?> GetByIdAsync(int id);
    }
}
