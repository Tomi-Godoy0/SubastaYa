using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }

        //-------
        public Billetera? Billetera { get; set; }
        public ICollection<Subasta> Subastas { get; set; } = [];
        public ICollection<Puja> Pujas { get; set; } = [];
        public ICollection<AuditoriaLog> Auditorias { get; set; } = [];
    }
}