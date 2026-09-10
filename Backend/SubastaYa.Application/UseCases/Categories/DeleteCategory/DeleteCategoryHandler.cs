using SubastaYa.Application.Interfaces.Repositories;

namespace SubastaYa.Application.UseCases.Categories.DeleteCategory
{
    public class DeleteCategoryHandler
    {
        private readonly ICategoryRepository _categoryRepository;

        public DeleteCategoryHandler(
            ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task HandleAsync(
            DeleteCategoryCommand command)
        {
            if (command.Id <= 0)
                throw new ArgumentException(
                    "El Id de la categoría no es válido.");

            var categoria = await _categoryRepository
                .GetByIdAsync(command.Id);

            if (categoria is null)
                throw new KeyNotFoundException(
                    "La categoría no existe.");

            await _categoryRepository.DeleteAsync(categoria);
        }
    }
}