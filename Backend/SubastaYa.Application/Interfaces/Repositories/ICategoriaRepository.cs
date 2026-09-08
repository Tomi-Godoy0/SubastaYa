using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.Interfaces.Repositories
{
    public interface ICategoriaRepository
    {
        Task<Categoria?> ObtenerPorNombreAsync(string nombre);

        Task<Categoria?> ObtenerPorIdAsync(int id);

        Task<Categoria> CrearAsync(Categoria categoria);

        Task<Categoria> ActualizarAsync(Categoria categoria);

        Task<IEnumerable<Categoria>> ObtenerTodasAsync();

        Task EliminarAsync(Categoria categoria);
    }
}