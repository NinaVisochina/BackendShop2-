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
    }
}
