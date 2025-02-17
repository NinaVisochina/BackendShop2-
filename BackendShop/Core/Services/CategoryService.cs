using AutoMapper;
using AutoMapper.QueryableExtensions;
using BackendShop.Core.Dto.Category;
using BackendShop.Core.Dto.Product;
using BackendShop.Core.Interfaces;
using BackendShop.Data.Data;
using BackendShop.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendShop.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ShopDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageHulk _imageHulk;

        public CategoryService(ShopDbContext context, IMapper mapper, IImageHulk imageHulk)
        {
            _context = context;
            _mapper = mapper;
            _imageHulk = imageHulk;
        }

        public async Task<List<CategoryDto>> GetListAsync()
        {
            return await _context.Categories
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            return await _context.Categories
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(c => c.Id == id);
        }
        public async Task CreateAsync(CategoryCreateDto model)
        {
            // Зберігаємо зображення категорії
            var imageName = await _imageHulk.Save(model.ImageCategory);

            // Перетворюємо DTO в Entity
            var entity = _mapper.Map<Category>(model);
            entity.ImageCategoryPath = imageName;

            // Генеруємо Slug
            entity.GenerateSlug();

            // Додаємо в базу та зберігаємо
            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();
        }
        public async Task EditAsync(CategoryEditDto model)
        {
            var entity = await _context.Categories.SingleOrDefaultAsync(c => c.CategoryId == model.Id);
            if (entity == null)
                throw new Exception("Категорію не знайдено");

            // Перевіряємо, чи змінилась назва категорії
            if (entity.Name != model.Name)
            {
                entity.Name = model.Name;
                entity.GenerateSlug();

                // Перевіряємо, чи такий Slug вже існує в БД
                var existingSlug = await _context.Categories
                    .AnyAsync(c => c.Slug == entity.Slug && c.CategoryId != entity.CategoryId);

                if (existingSlug)
                {
                    throw new Exception("Slug вже використовується. Оберіть іншу назву.");
                }
            }

            // Оновлення зображення
            if (model.ImageCategory != null && model.ImageCategory.Length > 0)
            {
                if (!string.IsNullOrEmpty(entity.ImageCategoryPath))
                {
                    _imageHulk.Delete(entity.ImageCategoryPath);
                }

                var newImageName = await _imageHulk.Save(model.ImageCategory);
                entity.ImageCategoryPath = newImageName;
            }

            _context.Categories.Update(entity);
            await _context.SaveChangesAsync();
        }

        //public async Task EditAsync(CategoryEditDto model)
        //{
        //    var entity = await _context.Categories.SingleOrDefaultAsync(c => c.CategoryId == model.Id);
        //    if (entity == null)
        //        throw new Exception("Категорію не знайдено");

        //    // Update fields
        //    entity.Name = model.Name;

        //    // Handle image update
        //    if (model.ImageCategory != null && model.ImageCategory.Length > 0)
        //    {
        //        if (!string.IsNullOrEmpty(entity.ImageCategoryPath))
        //        {
        //            _imageHulk.Delete(entity.ImageCategoryPath);
        //        }

        //        var newImageName = await _imageHulk.Save(model.ImageCategory);
        //        entity.ImageCategoryPath = newImageName;
        //    }

        //    _context.Categories.Update(entity);
        //    await _context.SaveChangesAsync();
        //}

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Categories.SingleOrDefaultAsync(c => c.CategoryId == id);
            if (entity == null)
                throw new Exception("Категорію не знайдено");

            if (!string.IsNullOrEmpty(entity.ImageCategoryPath))
            {
                _imageHulk.Delete(entity.ImageCategoryPath);
            }

            _context.Categories.Remove(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<List<ProductItemViewModel>> GetProductsByCategoryAsync(int id)
        {
            var products = await _context.Products
                .Where(p => p.SubCategory.CategoryId == id) // Фільтрація за категорією через підкатегорії
                .ProjectTo<ProductItemViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return products;
        }
        public async Task<CategoryDto> GetBySlugAsync(string slug)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Slug == slug);
            if (category == null) return null;
            return _mapper.Map<CategoryDto>(category);
        }

    }
}

