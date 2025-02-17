using BackendShop.Core.Dto.Category;
using BackendShop.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackendShop.BackShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var list = await _categoryService.GetListAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound(new { message = "Категорію не знайдено" });
            return Ok(category);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CategoryCreateDto model)
        {
            await _categoryService.CreateAsync(model);
            return Ok();
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Edit([FromForm] CategoryEditDto model)
        {
            await _categoryService.EditAsync(model);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteAsync(id);
            return Ok();
        }
        [HttpGet("{id}/products")]
        public async Task<IActionResult> GetProductsByCategory(int id)
        {
            var products = await _categoryService.GetProductsByCategoryAsync(id);

            if (!products.Any())
                return NotFound(new { message = "Продукти для цієї категорії не знайдені" });

            return Ok(products);
        }
        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var category = await _categoryService.GetBySlugAsync(slug);
            if (category == null) return NotFound();
            return Ok(category);
        }


    }
}
