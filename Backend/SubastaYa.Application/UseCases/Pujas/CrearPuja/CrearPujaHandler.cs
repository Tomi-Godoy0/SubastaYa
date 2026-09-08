using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.UseCases.Pujas.CrearPuja
{
    public class CrearPujaHandler
    {
        public CrearPujaHandler()
        {
        }

        public async Task<Puja> HandleAsync(CrearPujaCommand command)
        {
            if (command.SubastaId <= 0)
                throw new ArgumentException(
                    "La subasta no es válida.");

            if (command.CompradorId <= 0)
                throw new ArgumentException(
                    "El comprador no es válido.");

            if (command.Monto <= 0)
                throw new ArgumentException(
                    "El monto de la puja debe ser mayor a cero.");

            if (command.FechaPuja == default)
                command.FechaPuja = DateTime.UtcNow;

            var puja = new Puja
            {
                SubastaId = command.SubastaId,
                CompradorId = command.CompradorId,
                Monto = command.Monto,
                FechaPuja = command.FechaPuja
            };

            return await Task.FromResult(puja);
        }
    }
}