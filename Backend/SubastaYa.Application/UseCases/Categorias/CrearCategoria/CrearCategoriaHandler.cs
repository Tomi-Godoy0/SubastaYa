using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.UseCases.Categorias.CrearCategoria
{
    public class CrearCategoriaHandler
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CrearCategoriaHandler(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        internal async Task<Categoria> HandleAsync(CrearCategoriaCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Nombre))
                throw new ArgumentException("El nombre de la categoría es obligatorio.");

            var categoriaExistente =
                await _categoriaRepository.ObtenerPorNombreAsync(command.Nombre);

            if (categoriaExistente is not null)
                throw new InvalidOperationException("Ya existe una categoría con ese nombre.");

            var categoria = new Categoria
            {
                Nombre = command.Nombre.Trim(),
                UrlIcono = command.UrlIcono?.Trim() ?? string.Empty
            };

            return await _categoriaRepository.CrearAsync(categoria);
        }
    }
}