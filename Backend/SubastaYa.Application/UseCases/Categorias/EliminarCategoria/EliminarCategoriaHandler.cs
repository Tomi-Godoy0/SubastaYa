using SubastaYa.Application.Interfaces.Repositories;

namespace SubastaYa.Application.UseCases.Categorias.EliminarCategoria
{
    public class EliminarCategoriaHandler
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public EliminarCategoriaHandler(
            ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task HandleAsync(
            EliminarCategoriaCommand command)
        {
            if (command.Id <= 0)
                throw new ArgumentException(
                    "El Id de la categoría no es válido.");

            var categoria = await _categoriaRepository
                .ObtenerPorIdAsync(command.Id);

            if (categoria is null)
                throw new KeyNotFoundException(
                    "La categoría no existe.");

            await _categoriaRepository.EliminarAsync(categoria);
        }
    }
}