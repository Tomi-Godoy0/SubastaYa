using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.UseCases.Categorias.ActualizarCategoria
{
    public class ActualizarCategoriaCommand
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string UrlIcono { get; set; } = string.Empty;
    }
}