
using AutoMapper;
using RWAEShop.DTOs;
using RWAEshopDAL.Models;

namespace RWAEShop.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<ProductCategory, CategoryResponseDto>().ReverseMap();
            CreateMap<ProductCategory, CategoryCreateDto>().ReverseMap();
            CreateMap<ProductCategory, CategoryUpdateDto>().ReverseMap();
            CreateMap<Country, CountryResponseDto>().ReverseMap();
            CreateMap<Country, CountryUpdateDto>().ReverseMap();
            CreateMap<Country, CreateCountryDto>().ReverseMap();
            CreateMap<Product, ProductResponseDto>().ReverseMap();
            CreateMap<Product, ProductCreateDto>().ReverseMap();
            CreateMap<Product, ProductUpdateDto>().ReverseMap();
            CreateMap<Order, OrderCreateDto>().ReverseMap();
            CreateMap<Order, OrderResponseDto>().ReverseMap();
            CreateMap<Order, OrderUpdateDto>().ReverseMap();
            CreateMap<OrderItem, UpdateOrderItemDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<OrderItem, CreateOrderItemDto>().ReverseMap();
        }
    }
}
