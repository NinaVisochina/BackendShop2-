using BackendShop.Core.Dto.Category;
using BackendShop.Core.Dto.Product;


namespace BackendShop.Core.Interfaces
{
    public interface ICategoryService
    {
            Task<List<CategoryDto>> GetListAsync();
            Task<CategoryDto?> GetByIdAsync(int id);
            Task CreateAsync(CategoryCreateDto model);
            Task EditAsync(CategoryEditDto model);
            Task DeleteAsync(int id);
            Task<List<ProductItemViewModel>> GetProductsByCategoryAsync(int id);
            Task<CategoryDto> GetBySlugAsync(string slug);


    }

}

