using AutoMapper;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<UserInputDto, User>();
            CreateMap<ProductInputDto, Product>();
            CreateMap<CategoryInputDto, Category>();
            CreateMap<CategoryProductInputDto, CategoryProduct>();
            CreateMap<Product, ProductsPriceDto>().ForMember(x => x.Price, p => p.MapFrom(s => s.Price.ToString("f2"))).ForMember(x => x.BuyerName, b => b.MapFrom(n => n.Buyer.FirstName + " " + n.Buyer.LastName));
        }
    }
}
