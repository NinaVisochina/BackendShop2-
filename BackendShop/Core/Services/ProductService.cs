using AutoMapper;
using AutoMapper.QueryableExtensions;
using BackendShop.Core.Dto.Product;
using BackendShop.Core.Interfaces;
using BackendShop.Data.Data;
using BackendShop.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendShop.Services
{
    public class ProductService : IProductService
    {
        private readonly ShopDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageHulk _imageHulk;

        public ProductService(ShopDbContext context, IMapper mapper, IImageHulk imageHulk)
        {
            _context = context;
            _mapper = mapper;
            _imageHulk = imageHulk;
        }

        public async Task<List<ProductItemViewModel>> GetListAsync()
        {
            return await _context.Products
                .ProjectTo<ProductItemViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<ProductItemViewModel> GetByIdAsync(int id)
        {
            var product = await _context.Products
                .ProjectTo<ProductItemViewModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Id == id);

            if (product == null) throw new Exception("Продукт не знайдено");

            return product;
        }

        public async Task CreateAsync(ProductCreateViewModel model)
        {
            var entity = _mapper.Map<ProductEntity>(model);
            entity.GenerateSlug();
            _context.Products.Add(entity);
            await _context.SaveChangesAsync();

            if (model.ImagesDescIds.Any())
            {
                await _context.ProductDescImages
                    .Where(x => model.ImagesDescIds.Contains(x.Id))
                    .ForEachAsync(x => x.ProductId = entity.Id);
            }

            if (model.Images != null)
            {
                var p = 1;
                foreach (var image in model.Images)
                {
                    var pi = new ProductImageEntity
                    {
                        Image = await _imageHulk.Save(image),
                        Priority = p,
                        ProductId = entity.Id
                    };
                    p++;
                    _context.ProductImages.Add(pi);
                    await _context.SaveChangesAsync();
                }
            }
        }


        public async Task EditAsync(ProductEditViewModel model)
        {
            var product = await _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == model.Id);

            if (product == null) throw new Exception("Продукт не знайдено");

            // Перевіряємо, чи змінилась назва продукту
            if (product.Name != model.Name)
            {
                product.Name = model.Name;
                product.GenerateSlug(); // Генеруємо новий Slug

                // Перевіряємо, чи новий Slug вже існує
                var existingSlug = await _context.Products
                    .AnyAsync(p => p.Slug == product.Slug && p.Id != product.Id);

                if (existingSlug)
                {
                    throw new Exception("Slug вже використовується. Оберіть іншу назву.");
                }
            }

            // Оновлюємо інші поля продукту
            _mapper.Map(model, product);

            var oldNameImages = model.Images.Where(x => x.ContentType.Contains("old-image"))
                .Select(x => x.FileName) ?? [];

            var imgToDelete = product?.ProductImages?.Where(x => !oldNameImages.Contains(x.Image)) ?? [];
            foreach (var imgDel in imgToDelete)
            {
                _context.ProductImages.Remove(imgDel);
                _imageHulk.Delete(imgDel.Image);
            }

            if (model.Images is not null)
            {
                int index = 0;
                foreach (var image in model.Images)
                {
                    if (image.ContentType == "old-image")
                    {
                        var oldImage = product?.ProductImages?.FirstOrDefault(x => x.Image == image.FileName)!;
                        oldImage.Priority = index;
                    }
                    else
                    {
                        var imagePath = await _imageHulk.Save(image);
                        _context.ProductImages.Add(new ProductImageEntity
                        {
                            Image = imagePath,
                            Product = product,
                            Priority = index
                        });
                    }
                    index++;
                }
            }

            await _context.SaveChangesAsync();
        }


        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products
                .Include(x => x.ProductImages)
                .Include(x => x.ProductDescImages)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (product == null) throw new Exception("Продукт не знайдено");

            if (product.ProductImages != null)
                foreach (var p in product.ProductImages)
                    _imageHulk.Delete(p.Image);

            if (product.ProductDescImages != null)
                foreach (var p in product.ProductDescImages)
                    _imageHulk.Delete(p.Image);

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<ProductDescImageIdViewModel> UploadDescImageAsync(ProductDescImageUploadViewModel model)
        {
            if (model.Image != null)
            {
                var pdi = new ProductDescImageEntity
                {
                    Image = await _imageHulk.Save(model.Image),
                    DateCreate = DateTime.Now.ToUniversalTime(),
                };

                _context.ProductDescImages.Add(pdi);
                await _context.SaveChangesAsync();
                return _mapper.Map<ProductDescImageIdViewModel>(pdi);
            }

            throw new Exception("Зображення не завантажено");
        }
        public async Task<List<ProductItemViewModel>> GetBySubCategoryIdAsync(int subCategoryId)
        {
            var products = await _context.Products
                .Where(p => p.SubCategoryId == subCategoryId)
                .ProjectTo<ProductItemViewModel>(_mapper.ConfigurationProvider) // Автоматичне мапінг DTO через AutoMapper
                .ToListAsync();

            if (!products.Any())
                throw new Exception("Продуктів для цієї підкатегорії не знайдено");

            return products;
        }
        public async Task<List<ProductItemViewModel>> SearchProductsAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return new List<ProductItemViewModel>(); // Повертаємо пустий список замість викидання винятку
            }

            var products = await _context.Products
                .Where(p => p.Name.ToLower().Contains(query.ToLower()) ||
                            p.Description.ToLower().Contains(query.ToLower())) // Робимо пошук нечутливим до регістру
                .ProjectTo<ProductItemViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return products; // Просто повертаємо пустий список, якщо нічого не знайдено
        }

        public async Task<ProductItemViewModel> GetBySlugAsync(string slug)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Slug == slug);
            if (product == null) return null;
            return _mapper.Map<ProductItemViewModel>(product);
        }


    }
}
