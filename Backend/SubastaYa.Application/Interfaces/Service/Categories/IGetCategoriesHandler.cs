using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.UseCases.Categories.GetCategories;

namespace SubastaYa.Application.Interfaces.Service.Categories
{
    public interface IGetCategoriesHandler
    {
        public Task<List<CategoryResponse>> HandleAsync(GetCategoriesQuery query);
    }
}
