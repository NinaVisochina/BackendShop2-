using AutoMapper;
using BackendShop.Core.Dto;
using BackendShop.Core.Dto.Category;
using BackendShop.Core.Dto.Product;
using BackendShop.Core.Dto.SubCategory;
using BackendShop.Data.Entities;

namespace BackendShop.Core.MapperProfiles
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<Category, CategoryDto>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoryId))
                    .ForMember(dest => dest.ImageCategory, opt => opt.MapFrom(src =>
                        string.IsNullOrEmpty(src.ImageCategoryPath)
                        ? "noimage.jpg"  // Файл-заглушка
                        : src.ImageCategoryPath));
            CreateMap<CategoryCreateDto, Category>()
                .ForMember(x => x.ImageCategoryPath, opt => opt.Ignore());
            CreateMap<CategoryEditDto, Category>()
                    .ForMember(x => x.ImageCategoryPath, opt => opt.Ignore());
            CreateMap<ProductEntity, ProductItemViewModel>()
                .ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(src => src.SubCategory.Name))
                .ForMember(x => x.Images, opt => opt.MapFrom(x => x.ProductImages.OrderBy(x => x.Priority)
                    .Select(p => p.Image).ToArray()));

            CreateMap<ProductCreateViewModel, ProductEntity>();

            CreateMap<ProductEditViewModel, ProductEntity>()
                .ForMember(x => x.ProductImages, opt => opt.Ignore());

            CreateMap<ProductDescImageEntity, ProductDescImageIdViewModel>();
            CreateMap<SubCategory, SubCategoryDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SubCategoryId))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.ImageSubCategory, opt => opt.MapFrom(src => src.ImageSubCategoryPath));

            CreateMap<CreateSubCategoryDto, SubCategory>()
            .ForMember(x => x.ImageSubCategoryPath, opt => opt.Ignore());

            CreateMap<EditSubCategoryDto, SubCategory>()
                .ForMember(x => x.ImageSubCategoryPath, opt => opt.Ignore());
            CreateMap<RegisterDto, User>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(src => src.Email));
        }
    }
}
