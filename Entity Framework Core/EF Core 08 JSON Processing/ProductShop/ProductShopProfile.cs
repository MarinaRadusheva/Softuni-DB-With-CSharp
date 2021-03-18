using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ProductShop.DataTransferObjects;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<UserJsonModel, User>();
            CreateMap<ProductJsonModel, Product>();
            CreateMap<CategoryJsonModel, Category>();
            CreateMap<CategoryProductsJsonModel, CategoryProduct>();
            CreateMap<Product, ProductJsonModel>();
            CreateMap<Product, ProductsInRangeModel>()
                .ForMember(x => x.SellerFullName, options => options.MapFrom(p => (p.Seller.FirstName + " " + p.Seller.LastName)));
            CreateMap<Product, ProductsSoldModel>();
            CreateMap<User, UsersWithSoldProductsModel>()
                .ForMember(x => x.SoldProducts, options => options.MapFrom(p => Mapper.Map<IEnumerable<ProductsSoldModel>>(p.ProductsSold)));
        }
    }
}
