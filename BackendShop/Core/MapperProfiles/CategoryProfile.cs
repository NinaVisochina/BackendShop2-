using AutoMapper;
using BackendShop.Core.Dto.Category;
using BackendShop.Data.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BackendShop.Core.MapperProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            //CreateMap<Category, CategoryDto>();
            //CreateMap<CategoryCreateDto, Category>();
            //CreateMap<CategoryEditDto, Category>();
            //CreateMap<Category, CategoryDto>()
            //        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            //        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoryId))
            //        .ForMember(dest => dest.ImageCategory, opt => opt.MapFrom(src =>
            //            string.IsNullOrEmpty(src.ImageCategoryPath)
            //            ? "noimage.jpg"  // Файл-заглушка
            //            : src.ImageCategoryPath));
            //CreateMap<CategoryCreateDto, Category>()
            //    .ForMember(x => x.ImageCategoryPath, opt => opt.Ignore());
            //CreateMap<CategoryEditDto, Category>()
            //    .ForMember(x => x.ImageCategoryPath, opt => opt.Ignore());
        }
    }
}
