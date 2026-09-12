using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Service.Categories;
using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.UseCases.Categories.GetCategories
{
    public class GetCategoriesHandler : IGetCategoriesHandler
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoriesHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryResponse>> HandleAsync(GetCategoriesQuery query)
        {
            var categories = await _categoryRepository.GetAllAsync();

            return categories.Select(c => new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                IconUrl = c.IconUrl,
            }).ToList();
        }
    }
}