using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using ProductShop.DataTransferObjects;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace ProductShop
{
    public class StartUp
    {
        public static IMapper mapper;
        static void Main(string[] args)
        {
            var context = new ProductShopContext();
            //CreateDatabase(context);
            //var jsonUsers = File.ReadAllText(@"C:\Users\Marina\source\repos\EF Core 08 JSON Processing\ProductShop\Datasets\users.json");
            //var jsonProducts = File.ReadAllText(@"C:\Users\Marina\source\repos\EF Core 08 JSON Processing\ProductShop\Datasets\products.json");
            //var jsonCategories = File.ReadAllText(@"C:\Users\Marina\source\repos\EF Core 08 JSON Processing\ProductShop\Datasets\categories.json");
            //var jsonCatProd = File.ReadAllText(@"C:\Users\Marina\source\repos\EF Core 08 JSON Processing\ProductShop\Datasets\categories-products.json");
            //Console.WriteLine(ImportUsers(context, jsonUsers));
            //Console.WriteLine(ImportProducts(context, jsonProducts));
            //Console.WriteLine(ImportCategories(context, jsonCategories));
            //Console.WriteLine(ImportCategoryProducts(context, jsonCatProd));
            //Console.WriteLine(GetProductsInRange(context));
            //Console.WriteLine(GetSoldProducts(context));
            Console.WriteLine(GetCategoriesByProductsCount(context));
        }

        private static void InitializeMapper()
        {
            var config = new MapperConfiguration(x => x.AddProfile<ProductShopProfile>());
            mapper = config.CreateMapper();
        }
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Include(x => x.CategoryProducts)
                .ThenInclude(x => x.Product)
                .Select(x => new CategoryRevenueModel
            {
                Name = x.Name,
                ProductCount = x.CategoryProducts.Count(),
                AvgeragePrice = x.CategoryProducts.Average(x => x.Product.Price),
                TotalRevenue = x.CategoryProducts.Sum(x => x.Product.Price)
            })
                .OrderByDescending(s => s.ProductCount)
                .ToList();
            return JsonConvert.SerializeObject(categories, Formatting.Indented);
        }
            
        public static string GetSoldProducts(ProductShopContext context)
        {
            InitializeMapper();
            var users = context.Users
                .Include(x=>x.ProductsSold)
                .ThenInclude(b=>b.Buyer)
                .Where(x => x.ProductsSold.Any(p => p.BuyerId != null))
                .Select(x=>new UsersWithSoldProductsModel
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SoldProducts =x.ProductsSold
                        .Select(p=>mapper.Map<ProductsSoldModel>(p)).ToList()
                })
                .OrderBy(x=>x.LastName)
                .ThenBy(x=>x.FirstName)
                .ToList();           
            return JsonConvert.SerializeObject(users, Formatting.Indented);
        }
        public static string GetProductsInRange(ProductShopContext context)
        {
            
            //Get all products in a specified price range:  500 to 1000 (inclusive)
            //Order them by price
            var productsInrange = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000).OrderBy(x=>x.Price)
                .ProjectTo<ProductsInRangeModel>(new MapperConfiguration(x => x.AddProfile<ProductShopProfile>()))
                .ToList();

            var productsJson = JsonConvert.SerializeObject(productsInrange, Formatting.Indented);
            return productsJson;
        }
        static void CreateDatabase(DbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            Console.WriteLine("Database successfully created");
        }
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            InitializeMapper();
            var users = JsonConvert.DeserializeObject<IEnumerable<UserJsonModel>>(inputJson);
            var usersComplete = mapper.Map<IEnumerable<User>>(users);
            context.AddRange(usersComplete);
            context.SaveChanges();
            return $"Successfully imported {usersComplete.Count()}";
        }
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            InitializeMapper();
            var productsJsonModel = JsonConvert.DeserializeObject<IEnumerable<ProductJsonModel>>(inputJson);
            var products = mapper.Map<IEnumerable<Product>>(productsJsonModel);
            context.Products.AddRange(products);
            context.SaveChanges();
            return $"Successfully imported {products.Count()}";
        }
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            InitializeMapper();
            var categoriesJsonModel = JsonConvert.DeserializeObject<IEnumerable<CategoryJsonModel>>(inputJson);
            var categories = mapper.Map<IEnumerable<Category>>(categoriesJsonModel.Where(x=>x.Name!=null));
            context.Categories.AddRange(categories);
            context.SaveChanges();
            return $"Successfully imported {categories.Count()}";
        }
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            InitializeMapper();
            var catProductsJsonModel = JsonConvert.DeserializeObject<IEnumerable<CategoryProductsJsonModel>>(inputJson);
            var categoryProducts = mapper.Map<IEnumerable<CategoryProduct>>(catProductsJsonModel);
            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();
            return $"Successfully imported {categoryProducts.Count()}";
        }
    }
}
