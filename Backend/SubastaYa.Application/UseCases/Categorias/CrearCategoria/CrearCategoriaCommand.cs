using System;

namespace SubastaYa.Application.UseCases.Categorias.CrearCategoria
{
    public class CrearCategoriaCommand
    {
        public string Nombre { get; set; } = string.Empty;
        public string UrlIcono { get; set; } = string.Empty;
    }
}