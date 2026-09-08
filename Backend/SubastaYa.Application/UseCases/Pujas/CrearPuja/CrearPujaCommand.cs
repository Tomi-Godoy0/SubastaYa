using System;

namespace SubastaYa.Application.UseCases.Pujas.CrearPuja
{
    public class CrearPujaCommand
    {
        public int SubastaId { get; set; }
        public int CompradorId { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPuja { get; set; }
    }
}