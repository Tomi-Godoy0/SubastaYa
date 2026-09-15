using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Service.Categories;
using SubastaYa.Application.UseCases.Categories.CreateCategory;
using SubastaYa.Application.UseCases.Categories.GetCategories;

namespace SubastaYa.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICreateCategoryHandler _createCategoryHandler;
        private readonly IGetCategoriesHandler _getCategoriesHandler;

        public CategoryController(IGetCategoriesHandler getCategoriesHandler, ICreateCategoryHandler createCategoryHandler)
        {
            _getCategoriesHandler = getCategoriesHandler;
            _createCategoryHandler = createCategoryHandler;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryCommand command)
        {
            var categories = await _createCategoryHandler.HandleAsync(command);

            return CreatedAtAction(nameof(GetCategories), null, categories);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories() 
        {
            var categories = await _getCategoriesHandler.HandleAsync(new GetCategoriesQuery());

            return Ok(categories);
        }
    }
}
