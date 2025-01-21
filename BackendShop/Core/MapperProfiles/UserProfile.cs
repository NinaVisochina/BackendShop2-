using AutoMapper;
using BackendShop.Core.Dto;
using BackendShop.Data.Entities;

namespace BackendShop.Core.MapperProfiles
{
    public class UserProfile:Profile
    {
        public UserProfile()
        { 
        //CreateMap<RegisterDto, User>()
        //        .ForMember(x => x.UserName, opt => opt.MapFrom(src => src.Email));
        }
    }
}
