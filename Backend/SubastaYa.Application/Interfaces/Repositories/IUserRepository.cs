using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        // Command
        Task<User> AddAsync(User user);
        Task<bool> ExistsByIdAsync(int id);
        Task<bool> ExistsByEmailAsync(string email);

        //Queries
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
    }
}
