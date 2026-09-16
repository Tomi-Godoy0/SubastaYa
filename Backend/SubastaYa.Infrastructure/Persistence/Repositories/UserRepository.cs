using Microsoft.EntityFrameworkCore;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Domain.Entities;

namespace SubastaYa.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);

            return user; 
        }
        public async Task<User?> GetByIdAsync(int id) => await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        
        public async Task<bool> ExistsByIdAsync(int id) => await _context.Users.AnyAsync(u => u.Id == id);

        public async Task<bool> ExistsByEmailAsync(string email) => await _context.Users.AnyAsync(u => u.Email == email);

        public async Task<User?> GetByEmailAsync(string email) => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}
