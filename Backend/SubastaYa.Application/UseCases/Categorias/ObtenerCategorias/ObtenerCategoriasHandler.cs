using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.UseCases.Categorias.ObtenerCategorias
{
    public class ObtenerCategoriasHandler
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public ObtenerCategoriasHandler(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<IEnumerable<Categoria>> HandleAsync(
            ObtenerCategoriasQuery query)
        {
            return await _categoriaRepository.ObtenerTodasAsync();
        }
    }
}