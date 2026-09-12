using SubastaYa.Application.UseCases.Categories.CreateCategory;
using SubastaYa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.Interfaces.Service.Categories
{
    public interface ICreateCategoryHandler
    {
        public Task<Category> HandleAsync(CreateCategoryCommand command);
    }
}
