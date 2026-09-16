using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.Interfaces.Repositories
{
    public interface IWalletRepository
    {
        public Task<Wallet> AddAsync(Wallet wallet);

        //Queries
        public Task<Wallet?> GetByUserIdAsync(int id);
        public Task<Wallet?> GetByUserIdTrackedAsync(int id);
    }
}
