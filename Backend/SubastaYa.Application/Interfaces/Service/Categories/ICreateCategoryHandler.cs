using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.UseCases.Categories.CreateCategory;

namespace SubastaYa.Application.Interfaces.Service.Categories
{
    public interface ICreateCategoryHandler
    {
        public Task<CategoryResponse> HandleAsync(CreateCategoryCommand command);
    }
}
