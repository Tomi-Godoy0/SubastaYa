using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.UseCases.Categories.GetCategories
{
    public class GetCategoriesHandler
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoriesHandler(ICategoryRepository categoriaRepository)
        {
            _categoryRepository = categoriaRepository;
        }

        public async Task<IEnumerable<Category>> HandleAsync(
            GetCategoriesQuery query)
        {
            return await _categoryRepository.GetAllAsync();
        }
    }
}