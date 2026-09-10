using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.UseCases.Categories.UpdateCategory
{
    public class UpdateCategoryHandler
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateCategoryHandler(
            ICategoryRepository categoriaRepository)
        {
            _categoryRepository = categoriaRepository;
        }

        public async Task<Category> HandleAsync(
            UpdateCategoryCommand command)
        {
            if (command.Id <= 0)
                throw new ArgumentException("El Id de la categoría no es válido.");

            if (string.IsNullOrWhiteSpace(command.Nombre))
                throw new ArgumentException(
                    "El nombre de la categoría es obligatorio.");

            var categoria = await _categoryRepository
                .GetByIdAsync(command.Id);

            if (categoria is null)
                throw new KeyNotFoundException(
                    "La categoría no existe.");

            var categoriaConMismoNombre = await _categoryRepository
                .GetByNameAsync(command.Nombre);

            if (categoriaConMismoNombre is not null &&
                categoriaConMismoNombre.Id != categoria.Id)
            {
                throw new InvalidOperationException(
                    "Ya existe otra categoría con ese nombre.");
            }

            categoria.Name = command.Nombre.Trim();
            categoria.IconUrl = command.UrlIcono?.Trim() ?? string.Empty;

            return await _categoryRepository.UpdateAsync(categoria);
        }
    }
}