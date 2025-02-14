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
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price));
        }
    }
}
