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


            CreateMap<Order, OrderVM>()
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.IdOrder))
            .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.Name + " " + src.User.LastName))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<OrderItem, OrderItemVM>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Product.ImageUrl))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));

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
