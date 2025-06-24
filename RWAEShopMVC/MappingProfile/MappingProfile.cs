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


            CreateMap<Order, OrderVM>().ReverseMap();
            CreateMap<OrderItem, OrderItemVM>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name));

            CreateMap<Product, ProductVM>().ReverseMap();
            CreateMap<Product, ProductVM>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.CountryNames, opt => opt.MapFrom(src =>
                    src.CountryProducts.Select(cp => cp.Country.Name).ToList()));

            CreateMap<ProductVM, Product>()
                 .ForMember(dest => dest.Category, opt => opt.Ignore());
           




        }
    }
}
