using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.UseCases.Categories.GetCategories;
using SubastaYa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.Interfaces.Service.Categories
{
    public interface IGetCategoriesHandler
    {
        public Task<List<CategoryResponse>> HandleAsync(GetCategoriesQuery query);
    }
}
