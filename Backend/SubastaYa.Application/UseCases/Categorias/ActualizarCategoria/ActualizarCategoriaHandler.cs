using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.UseCases.Categorias.ActualizarCategoria
{
    public class ActualizarCategoriaHandler
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public ActualizarCategoriaHandler(
            ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<Categoria> HandleAsync(
            ActualizarCategoriaCommand command)
        {
            if (command.Id <= 0)
                throw new ArgumentException("El Id de la categoría no es válido.");

            if (string.IsNullOrWhiteSpace(command.Nombre))
                throw new ArgumentException(
                    "El nombre de la categoría es obligatorio.");

            var categoria = await _categoriaRepository
                .ObtenerPorIdAsync(command.Id);

            if (categoria is null)
                throw new KeyNotFoundException(
                    "La categoría no existe.");

            var categoriaConMismoNombre = await _categoriaRepository
                .ObtenerPorNombreAsync(command.Nombre);

            if (categoriaConMismoNombre is not null &&
                categoriaConMismoNombre.Id != categoria.Id)
            {
                throw new InvalidOperationException(
                    "Ya existe otra categoría con ese nombre.");
            }

            categoria.Nombre = command.Nombre.Trim();
            categoria.UrlIcono = command.UrlIcono?.Trim() ?? string.Empty;

            return await _categoriaRepository.ActualizarAsync(categoria);
        }
    }
}