using BackendShop.Core.Dto.Product;
using BackendShop.Core.Dto.SubCategory;

namespace BackendShop.Core.Interfaces
{
    public interface ISubCategoryService
    {
        Task<List<SubCategoryDto>> GetListAsync();
        Task<SubCategoryDto> GetByIdAsync(int id);
        Task<List<SubCategoryDto>> GetByCategoryIdAsync(int categoryId);
        Task CreateAsync(CreateSubCategoryDto model);
        Task EditAsync(EditSubCategoryDto model);
        Task DeleteAsync(int id);
        Task<SubCategoryDto> GetBySlugAsync(string slug);
    }
}
