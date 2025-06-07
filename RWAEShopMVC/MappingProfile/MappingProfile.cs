using AutoMapper;
using RWAEshopDAL.Models;
using RWAEShopMVC.ViewModels;

namespace RWAEShopMVC.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<User, UserRegisterVM>().ReverseMap();
            CreateMap<User, UserLoginVM>().ReverseMap();
            CreateMap<UserRegisterVM, User>()
                .ForMember(dest => dest.RoleId, opt => opt.Ignore());
        }
    }
}
