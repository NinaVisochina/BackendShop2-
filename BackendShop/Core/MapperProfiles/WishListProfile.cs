using AutoMapper;
using BackendShop.Core.Dto.WishList;
using BackendShop.Data.Entities;

namespace BackendShop.Core.MapperProfiles
{
    public class WishListProfile : Profile
    {
        public WishListProfile()
        {
            CreateMap<WishListItem, WishListItemDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.ProductImage, opt => opt.MapFrom(src =>
                    src.Product.ProductImages != null && src.Product.ProductImages.Any()
                        ? src.Product.ProductImages.First().Image
                        : null)); // Перевірка на null і вибір першого зображення
        }
    }
}
