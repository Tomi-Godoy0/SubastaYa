using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.UseCases.Categories.CreateCategory
{
    public class CreateCategoryHandler
    {
        private readonly ICategoryRepository _categoryRepository;

        public CreateCategoryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        internal async Task<Category> HandleAsync(CreateCategoryCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Name))
                throw new ArgumentException("El nombre de la categoría es obligatorio.");

            var categoriaExistente =
                await _categoryRepository.GetByNameAsync(command.Name);

            if (categoriaExistente is not null)
                throw new InvalidOperationException("Ya existe una categoría con ese nombre.");

            var categoria = new Category
            {
                Name = command.Name.Trim(),
                IconUrl = command.IconUrl?.Trim() ?? string.Empty
            };

            return await _categoryRepository.AddAsync(categoria);
        }
    }
}