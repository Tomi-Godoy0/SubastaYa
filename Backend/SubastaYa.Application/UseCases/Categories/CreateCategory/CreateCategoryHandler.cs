using SubastaYa.Application.Interfaces;
using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Service.Categories;
using SubastaYa.Domain.Entities;
using SubastaYa.Domain.Exceptions;

namespace SubastaYa.Application.UseCases.Categories.CreateCategory
{
    public class CreateCategoryHandler : ICreateCategoryHandler
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategoryHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CategoryResponse> HandleAsync(CreateCategoryCommand command)
        {
            var existingCategory = await _categoryRepository.GetByNameAsync(command.Name);

            if (existingCategory is not null)
                throw new ConflictException("Ya existe una categoría con ese nombre.");

            var category = new Category
            {
                Name = command.Name.Trim(),
                IconUrl = command.IconUrl?.Trim() ?? string.Empty
            };

            await _categoryRepository.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name.Trim(),
                IconUrl = category.IconUrl
            };
        }
    }
}