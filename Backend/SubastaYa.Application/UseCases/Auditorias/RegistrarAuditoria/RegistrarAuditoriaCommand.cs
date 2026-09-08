using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.UseCases.Auditorias.RegistrarAuditoria
{
    public class RegistrarAuditoriaCommand
    {
        public string Entidad { get; set; } = string.Empty;
        public int EntidadId { get; set; }
        public string Accion { get; set; } = string.Empty;
        public int? UsuarioId { get; set; }
        public string? DetallesJson { get; set; }
        public DateTime Fecha { get; set; }
    }
}