using BackendShop.Core.Dto.Product;
using BackendShop.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackendShop.BackShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var list = await _productService.GetListAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            return Ok(product);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] ProductCreateViewModel model)
        {
            await _productService.CreateAsync(model);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromForm] ProductEditViewModel model)
        {
            await _productService.EditAsync(model);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);
            return Ok();
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadDescImage([FromForm] ProductDescImageUploadViewModel model)
        {
            var result = await _productService.UploadDescImageAsync(model);
            return Ok(result);
        }
    }

    //[Route("api/[controller]")]
    //[ApiController]
    //public class ProductsController(
    //    ShopDbContext context, IImageHulk imageHulk,
    //    IMapper mapper) : ControllerBase
    //{
    //    [HttpGet]
    //    public IActionResult GetList()
    //    {
    //        var list = context.Products
    //            .ProjectTo<ProductItemViewModel>(mapper.ConfigurationProvider)
    //            .ToList();
    //        return Ok(list);
    //    }
    //    [HttpPost]
    //    public async Task<IActionResult> Create([FromForm] ProductCreateViewModel model)
    //    {
    //        var entity = mapper.Map<ProductEntity>(model);
    //        context.Products.Add(entity);
    //        context.SaveChanges();

    //        if (model.ImagesDescIds.Any())
    //        {
    //            await context.ProductDescImages
    //                .Where(x => model.ImagesDescIds.Contains(x.Id))
    //                .ForEachAsync(x => x.ProductId = entity.Id);
    //        }

    //        if (model.Images != null)
    //        {
    //            var p = 1;
    //            foreach (var image in model.Images)
    //            {
    //                var pi = new ProductImageEntity
    //                {
    //                    Image = await imageHulk.Save(image),
    //                    Priority = p,
    //                    ProductId = entity.Id
    //                };
    //                p++;
    //                context.ProductImages.Add(pi);
    //                await context.SaveChangesAsync();
    //            }
    //        }
    //        return Created();
    //    }

    //    [HttpDelete("{id}")]
    //    public IActionResult Delete(int id)
    //    {
    //        var product = context.Products
    //            .Include(x => x.ProductImages)
    //            .Include(x => x.ProductDescImages)
    //            .SingleOrDefault(x => x.Id == id);
    //        if (product == null) return NotFound();

    //        if (product.ProductImages != null)
    //            foreach (var p in product.ProductImages)
    //                imageHulk.Delete(p.Image);

    //        if (product.ProductDescImages != null)
    //            foreach (var p in product.ProductDescImages)
    //                imageHulk.Delete(p.Image);

    //        context.Products.Remove(product);
    //        context.SaveChanges();
    //        return Ok();
    //    }

    //    [HttpGet("{id}")]
    //    public async Task<IActionResult> GetProductById(int id)
    //    {
    //        var product = await context.Products
    //            .ProjectTo<ProductItemViewModel>(mapper.ConfigurationProvider)
    //            .SingleOrDefaultAsync(p => p.Id == id);
    //        if (product == null) return NotFound();
    //        return Ok(product);
    //    }

    //    [HttpPut]
    //    public async Task<IActionResult> Edit([FromForm] ProductEditViewModel model)
    //    {
    //        var request = this.Request;
    //        var product = await context.Products
    //            .Include(p => p.ProductImages)
    //            .FirstOrDefaultAsync(p => p.Id == model.Id);

    //        mapper.Map(model, product);

    //        var oldNameImages = model.Images.Where(x => x.ContentType.Contains("old-image"))
    //            .Select(x => x.FileName) ?? [];

    //        var imgToDelete = product?.ProductImages?.Where(x => !oldNameImages.Contains(x.Image)) ?? [];
    //        foreach (var imgDel in imgToDelete)
    //        {
    //            context.ProductImages.Remove(imgDel);
    //            imageHulk.Delete(imgDel.Image);
    //        }

    //        if (model.Images is not null)
    //        {
    //            int index = 0;
    //            foreach (var image in model.Images)
    //            {
    //                if (image.ContentType == "old-image")
    //                {
    //                    var oldImage = product?.ProductImages?.FirstOrDefault(x => x.Image == image.FileName)!;
    //                    oldImage.Priority = index;
    //                }
    //                else
    //                {
    //                    var imagePath = await imageHulk.Save(image);
    //                    context.ProductImages.Add(new ProductImageEntity
    //                    {
    //                        Image = imagePath,
    //                        Product = product,
    //                        Priority = index
    //                    });
    //                }
    //                index++;
    //            }
    //        }
    //        await context.SaveChangesAsync();

    //        return Ok();
    //    }


    //    [HttpPost("upload")]
    //    public async Task<IActionResult> UploadDescImage([FromForm] ProductDescImageUploadViewModel model)
    //    {
    //        if (model.Image != null)
    //        {
    //            var pdi = new ProductDescImageEntity
    //            {
    //                Image = await imageHulk.Save(model.Image),
    //                DateCreate = DateTime.Now.ToUniversalTime(),

    //            };
    //            context.ProductDescImages.Add(pdi);
    //            await context.SaveChangesAsync();
    //            return Ok(mapper.Map<ProductDescImageIdViewModel>(pdi));
    //        }
    //        return BadRequest();
    //    }
    //}
}
