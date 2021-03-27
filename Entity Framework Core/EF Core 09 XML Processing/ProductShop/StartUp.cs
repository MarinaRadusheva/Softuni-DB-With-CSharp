using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        private static IMapper Mapper;
        public static MapperConfiguration config = new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>());
      
        public static void Main(string[] args)
        {
            ProductShopContext context = new ProductShopContext();
            //context.Database.Migrate();
            //string usersXml = File.ReadAllText(@"C:\Users\Marina\Downloads\09 XML Processing ProductShop\ProductShop\Datasets\users.xml");
            //string productXml = File.ReadAllText(@"C:\Users\Marina\Downloads\09 XML Processing ProductShop\ProductShop\Datasets\products.xml");
            //string categoryXml = File.ReadAllText(@"C:\Users\Marina\Downloads\09 XML Processing ProductShop\ProductShop\Datasets\categories.xml");
            //string categoryProductXml = File.ReadAllText(@"C:\Users\Marina\Downloads\09 XML Processing ProductShop\ProductShop\Datasets\categories-products.xml");
            //System.Console.WriteLine(ImportUsers(context, usersXml));
            //System.Console.WriteLine(ImportProducts(context, productXml));
            //System.Console.WriteLine(ImportCategories(context, categoryXml));
            //System.Console.WriteLine(ImportCategoryProducts(context, categoryProductXml));
            //Console.WriteLine(GetProductsInRange(context));
            //Console.WriteLine(GetSoldProducts(context));
            //Console.WriteLine(GetCategoriesByProductsCount(context));
            Console.WriteLine(GetUsersWithProducts(context));
        }
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            //users with at least 1 sold product. Order by number of sold products (hi to lo). Select first and last name, age, count products and products - name and price sorted by price (descending). Take 10.
            var user = new UsersOutputDto()
            {
                Count = context.Users.Count(x=>x.ProductsSold.Any()),
                Users = context.Users.ToArray()
                .Where(x => x.ProductsSold.Count > 0)
                .OrderByDescending(x => x.ProductsSold.Count)
                .Take(10)
                .Select(x => new UserWithAgeAndSoldProductsDto()
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Age = x.Age,
                    SoldProducts = new SoldProductsDto()
                    {
                        Count = x.ProductsSold.Count,
                        Products = x.ProductsSold.ToArray()
                        .Select(p => new ProductNamePriceDto()
                        {
                            Name = p.Name,
                            Price = p.Price,
                        }).OrderByDescending(s => s.Price)
                        .ToArray(),
                    },

                })
                
                .ToArray()
            };
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            var serializer = new XmlSerializer(typeof(UsersOutputDto), new XmlRootAttribute("Users"));
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            serializer.Serialize(writer, user, namespaces);
            return sb.ToString().TrimEnd();
        }
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            //categories-select its name, the number of products, the average price of products and total revenue.Order by number of products (descending) then by total revenue.
            var categories = context.Categories
                .Select(x => new CategoriesByProductsDto
                {
                    Name = x.Name,
                    ProductsCount = x.CategoryProducts.Count(),
                    AveragePrice = x.CategoryProducts.Average(p => p.Product.Price).ToString(),
                    Revenue = x.CategoryProducts.Sum(p => p.Product.Price),
                })
                .OrderByDescending(x => x.ProductsCount)
                .ThenBy(x => x.Revenue)
                .ToList();
            var serializer = new XmlSerializer(typeof(List<CategoriesByProductsDto>), new XmlRootAttribute("Categories"));
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            serializer.Serialize(writer, categories, namespaces);
            return sb.ToString().TrimEnd();
        }
        public static string GetSoldProducts(ProductShopContext context)
        {
            //users who have at least 1 sold item; Order by last name, then by first name. Take 5;
            var users = context.Users
                .Where(x => x.ProductsSold.Count() > 0)
                .Select(x => new UserWithSoldProductDto
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Products = x.ProductsSold
                        .Select(p => new ProductNamePriceDto
                        {
                            Name = p.Name,
                            Price = p.Price,
                        })
                    .ToArray(),
                })
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Take(5)
                .ToList();
            StringBuilder sb = new StringBuilder();
            var serializer = new XmlSerializer(typeof(List<UserWithSoldProductDto>), new XmlRootAttribute("Users"));
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            StringWriter writer = new StringWriter(sb);
            using (writer)
            {
                serializer.Serialize(writer, users,namespaces);
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetProductsInRange(ProductShopContext context)
        {           
            //price range between 500 and 1000 (inclusive). Order them by price (from lowest to highest), take 10;

            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .OrderBy(x => x.Price)
                .Take(10)
                .ProjectTo<ProductsPriceDto>(config)
                .ToList();
            var serializer = new XmlSerializer(typeof(List<ProductsPriceDto>), new XmlRootAttribute("Products"));
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            using (writer)
            {

                serializer.Serialize(writer, products, namespaces);
            }
            return sb.ToString().TrimEnd();
        }
       
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            InitializeMapper();
            StringReader reader = new StringReader(inputXml);
            var serializer = new XmlSerializer(typeof(CategoryProductInputDto[]), new XmlRootAttribute("CategoryProducts"));
            var deserializedCatProd = (CategoryProductInputDto[])serializer.Deserialize(reader);
            var categoryProducts = Mapper.Map<IEnumerable<CategoryProduct>>(deserializedCatProd);
            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();
            return $"Successfully imported {categoryProducts.Count()}";
        }
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            InitializeMapper();
            var reader = new StringReader(inputXml);
            var serializer = new XmlSerializer(typeof(CategoryInputDto[]), new XmlRootAttribute("Categories"));
            CategoryInputDto[] deserializedCategories = (CategoryInputDto[])serializer.Deserialize(reader);
            var categories = Mapper.Map<IEnumerable<Category>>(deserializedCategories.Where(x => x.Name != null));
            context.Categories.AddRange(categories);
            context.SaveChanges();
            return $"Successfully imported {categories.Count()}";

        }
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            InitializeMapper();
            StringReader reader = new StringReader(inputXml);
            var serializer = new XmlSerializer(typeof(ProductInputDto[]), new XmlRootAttribute("Products"));
            ProductInputDto[] deserializedProducts = (ProductInputDto[])serializer.Deserialize(reader);
            var products = Mapper.Map<IEnumerable<Product>>(deserializedProducts);
            context.Products.AddRange(products);
            context.SaveChanges();
            return $"Successfully imported {products.Count()}";
        }
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            InitializeMapper();
            StringReader reader = new StringReader(inputXml);
            var serializer = new XmlSerializer(typeof(UserInputDto[]), new XmlRootAttribute("Users"));
            var deserializedUsers = (UserInputDto[])serializer.Deserialize(reader);
            var users = Mapper.Map<IEnumerable<User>>(deserializedUsers);
            context.AddRange(users);
            context.SaveChanges();
            return $"Successfully imported {users.Count()}";
        }
        private static void InitializeMapper()
        {
            //MapperConfiguration config = new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>());
            Mapper = config.CreateMapper();
        }


    }
}