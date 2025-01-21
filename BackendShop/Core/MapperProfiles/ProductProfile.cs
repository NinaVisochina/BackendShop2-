using AutoMapper;
using BackendShop.Core.Dto.Product;
using BackendShop.Data.Entities;

namespace BackendShop.Core.MapperProfiles
{
    public class ProductProfile: Profile
    {
        public ProductProfile()
        {
            //CreateMap<ProductEntity, ProductItemViewModel>()
            //    .ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(src => src.SubCategory.Name))
            //    .ForMember(x => x.Images, opt => opt.MapFrom(x => x.ProductImages.OrderBy(x => x.Priority)
            //        .Select(p => p.Image).ToArray()));

            //CreateMap<ProductCreateViewModel, ProductEntity>();

            //CreateMap<ProductEditViewModel, ProductEntity>()
            //    .ForMember(x => x.ProductImages, opt => opt.Ignore());

            //CreateMap<ProductDescImageEntity, ProductDescImageIdViewModel>();
        }
    }
}
