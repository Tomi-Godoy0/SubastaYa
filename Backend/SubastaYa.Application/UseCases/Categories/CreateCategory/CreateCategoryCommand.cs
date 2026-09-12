using System;
using System.ComponentModel.DataAnnotations;

namespace SubastaYa.Application.UseCases.Categories.CreateCategory
{
    public class CreateCategoryCommand
    {
        [Required(ErrorMessage = "El nombre de la catergoría es obligatorio")]
        public string Name { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
    }
}