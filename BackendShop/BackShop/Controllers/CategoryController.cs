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
    }
}
//using AutoMapper;
//using AutoMapper.QueryableExtensions;
//using BackendShop.Core.Dto.Category;
//using BackendShop.Core.Interfaces;
//using BackendShop.Services;
//using BackendShop.Data.Data;
//using BackendShop.Data.Entities;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace BackendShop.BackShop.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CategoryController(ShopDbContext _context, IMapper mapper, IImageHulk imageHulk, IConfiguration configuration) : ControllerBase
//    {
//        [HttpGet]
//        public IActionResult GetList()
//        {
//            Console.WriteLine("Запит отримано на GetList");
//            Thread.Sleep(1000);
//            var list = _context.Categories
//                .ProjectTo<CategoryDto>(mapper.ConfigurationProvider)
//                .ToList();
//            return Ok(list);
//        }

//        [HttpPost("create")]
//        public async Task<IActionResult> Create([FromForm] CategoryCreateDto model)
//        {
//            var imageName = await imageHulk.Save(model.ImageCategory);
//            var entity = mapper.Map<Category>(model);
//            entity.ImageCategoryPath = imageName;
//            _context.Categories.Add(entity);
//            _context.SaveChanges();
//            return Ok();
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var entity = _context.Categories.SingleOrDefault(x => x.CategoryId == id);
//            if (entity == null)
//                return NotFound();
//            if (!string.IsNullOrEmpty(entity.ImageCategoryPath))
//                imageHulk.Delete(entity.ImageCategoryPath);
//            _context.Categories.Remove(entity);
//            _context.SaveChanges();
//            return Ok();
//        }

//        [HttpPut("edit")]
//        public async Task<IActionResult> Edit([FromForm] CategoryEditDto model)
//        {
//            var entity = await _context.Categories.SingleOrDefaultAsync(x => x.CategoryId == model.Id);
//            if (entity == null)
//                return NotFound();

//            // Update basic fields
//            entity.Name = model.Name;

//            // Handle image update
//            if (model.ImageCategory != null && model.ImageCategory.Length > 0)
//            {
//                // Delete the old image if it exists
//                if (!string.IsNullOrEmpty(entity.ImageCategoryPath))
//                {
//                    imageHulk.Delete(entity.ImageCategoryPath);
//                }

//                // Save the new image
//                var newImageName = await imageHulk.Save(model.ImageCategory);
//                entity.ImageCategoryPath = newImageName;
//            }

//            _context.Categories.Update(entity);
//            await _context.SaveChangesAsync();

//            return Ok();
//        }

//        [HttpGet("{id}")]
//        public IActionResult GetById(int id)
//        {
//            var item = _context.Categories
//                .ProjectTo<CategoryDto>(mapper.ConfigurationProvider)
//                .SingleOrDefault(x => x.Id == id);

//            if (item == null)
//                return NotFound(new { message = "Категорію не знайдено" });

//            return Ok(item);
//        }

//        //[HttpGet("{id}")]
//        //public IActionResult GetById(int id)
//        //{
//        //    var item = _context.Categories
//        //        .ProjectTo<CategoryDto>(mapper.ConfigurationProvider)
//        //        .SingleOrDefault(x => x.Id == id);
//        //    return Ok(item);
//        //}

//        //[HttpGet("names")]
//        //public async Task<IActionResult> GetCategoriesNames()
//        //{
//        //    var result = await context.Categories
//        //    .ProjectTo<SelectItemViewModel>(mapper.ConfigurationProvider)
//        //    .ToListAsync();
//        //    return Ok(result);
//        //}
//    }
//}
