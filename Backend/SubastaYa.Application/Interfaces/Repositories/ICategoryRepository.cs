using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByNameAsync(string name);

        Task<Category?> GetByIdAsync(int id);

        Task<Category> AddAsync(Category category);

        Task<Category> UpdateAsync(Category category);

        Task<List<Category>> GetAllAsync();

        Task<bool> ExistsAsync(int id);
        Task DeleteAsync(Category categoria);
    }
}