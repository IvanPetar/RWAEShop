using AutoMapper;
using RWAEshopDAL.Models;
using RWAEShopMVC.ViewModels;

namespace RWAEShopMVC.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<User, UserRegisterVM>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName))
                .ReverseMap();
            CreateMap<User, UserLoginVM>().ReverseMap();
            CreateMap<UserRegisterVM, User>()
                .ForMember(dest => dest.RoleId, opt => opt.Ignore());


            CreateMap<Order, OrderVM>().ReverseMap();
            CreateMap<OrderItem, OrderItemVM>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name));

            CreateMap<Product, ProductVM>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.CountryNames, opt => opt.MapFrom(src =>
                 src.CountryProducts.Select(cp => cp.Country.Name).ToList()))
                .ReverseMap()
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt =>  opt.Ignore());

            CreateMap<ProductCategory, CategoryVM>().ReverseMap();

            CreateMap<Country, CountryVM>().ReverseMap();

        }
    }
}
