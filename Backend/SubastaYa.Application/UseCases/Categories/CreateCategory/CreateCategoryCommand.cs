using System;

namespace SubastaYa.Application.UseCases.Categories.CreateCategory
{
    public class CreateCategoryCommand
    {
        public string Name { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
    }
}