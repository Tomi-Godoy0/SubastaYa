using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByNameAsync(string name);

        Task<Category?> GetByIdAsync(int id);

        Task<Category> AddAsync(Category category);

        Task<Category> UpdateAsync(Category category);

        Task<IEnumerable<Category>> GetAllAsync();

        Task DeleteAsync(Category category);
    }
}