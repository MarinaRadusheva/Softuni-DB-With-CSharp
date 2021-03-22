using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CarDealer
{
    public class StartUp
    {
        private static IMapper Mapper;
        static void Main(string[] args)
        {
            CarDealerContext dbContext = new CarDealerContext();
            //dbContext.Database.Migrate();
            //string suppliersJson = File.ReadAllText(@"C:\Users\Marina\source\repos\CarDealer\CarDealer\Datasets\suppliers.json");
            //string partsJson = File.ReadAllText(@"C:\Users\Marina\source\repos\CarDealer\CarDealer\Datasets\parts.json");
            //string carsJson = File.ReadAllText(@"C:\Users\Marina\source\repos\CarDealer\CarDealer\Datasets\cars.json");
            //string customersJson = File.ReadAllText(@"C:\Users\Marina\source\repos\CarDealer\CarDealer\Datasets\customers.json");
            //string salesJson = File.ReadAllText(@"C:\Users\Marina\source\repos\CarDealer\CarDealer\Datasets\sales.json");
            //Console.WriteLine(ImportSuppliers(dbContext, suppliersJson));
            //Console.WriteLine(ImportParts(dbContext, partsJson));
            //Console.WriteLine(ImportCars(dbContext, carsJson));
            //Console.WriteLine(ImportCustomers(dbContext, customersJson));
            //Console.WriteLine(ImportSales(dbContext, salesJson));
            //Console.WriteLine(GetOrderedCustomers(dbContext));
            //Console.WriteLine(GetCarsFromMakeToyota(dbContext));
            //Console.WriteLine(GetLocalSuppliers(dbContext));
            //Console.WriteLine(GetCarsWithTheirListOfParts(dbContext));
            //Console.WriteLine(GetTotalSalesByCustomer(dbContext));
            Console.WriteLine(GetSalesWithAppliedDiscount(dbContext));
        }
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Take(10)
                .Select(x => new SalesWithOrWithoutDiscountDto
                {
                    Car = new CarDto
                    {
                        Make = x.Car.Make,
                        Model = x.Car.Model,
                        TravelledDistance = x.Car.TravelledDistance,
                    },
                    Name = x.Customer.Name,
                    Discount = x.Discount.ToString("f2"),
                    Price = x.Car.Parts.Sum(x => x.Part.Price).ToString("f2"),
                    PriceWithDiscount = (x.Car.Parts.Sum(x => x.Part.Price) * (1 - (x.Discount / 100))).ToString("f2"),
                })                
                .ToList();
            string output = JsonConvert.SerializeObject(sales, Formatting.Indented);
            return output;
        }
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            InitializeMapper();
            var customers = context.Customers
                .Where(x => x.Cars.Count() > 0)
                .ProjectTo<CustomerWithCarDto>()
                .OrderByDescending(x => x.SpentMoney)
                .ThenByDescending(x => x.BoughtCars)
                .ToList();
            string output = JsonConvert.SerializeObject(customers, Formatting.Indented);
            return output;
        }


        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carsWithParts = context.Cars
                .Select(x => new
                 {
                     car = new CarDto
                     {
                         Make = x.Make,
                         Model = x.Model,
                         TravelledDistance = x.TravelledDistance,
                     },
                     parts = x.Parts
                     .Select(p => new PartsOfCarDto 
                     { 
                         Name = p.Part.Name,
                         Price = $"{p.Part.Price:f2}",
                      })
                     .ToList()
                     })
                .ToList();
            string output = JsonConvert.SerializeObject(carsWithParts, Formatting.Indented);
            return output;
        }
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(x => x.IsImporter == false)
                .Select(x => new NonImportersOutputDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    PartsCount = x.Parts.Count(),
                })
                .ToList();
            string output = JsonConvert.SerializeObject(suppliers, Formatting.Indented);
            return output;
        }
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var toyotaCars = context.Cars
                .Where(x => x.Make == "Toyota")
                .Select(x=>new ToyotaCarsOutput 
                    { Id=x.Id,
                     Make = x.Make,
                        Model=x.Model,
                        TravelledDistance = x.TravelledDistance,
                    })
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .ToList();
            string output = JsonConvert.SerializeObject(toyotaCars, Formatting.Indented);
            return output;
        }
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers.OrderBy(x => x.BirthDate).ThenBy(x => x.IsYoungDriver).Select(x=>new CustomersOutputDto
            {
                Name=x.Name,
                BirthDate = x.BirthDate.ToString("dd/MM/yyyy"),
                IsYoungDriver=x.IsYoungDriver,
            }).ToList();
            string output = JsonConvert.SerializeObject(customers, Formatting.Indented);
            return output;
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            InitializeMapper();
            var sales = JsonConvert.DeserializeObject<IEnumerable<Sale>>(inputJson);
            //var salesDto = JsonConvert.DeserializeObject<IEnumerable<SaleInputDto>>(inputJson);
            //var sales = Mapper.Map<IEnumerable<Sale>>(salesDto);
            //var sales = new List<Sale>();
            //foreach (var saleDto in salesDto)
            //{
            //    var car = context.Cars.FirstOrDefault(x => x.Id == saleDto.CarId);
            //    if (car!=null && !car.Customers.Any(x=>x.Id==saleDto.CustomerId))
            //    {
            //        var sale = Mapper.Map<Sale>(saleDto); 
            //        sales.Add(sale);
            //    }
               
            //}
            context.Sales.AddRange(sales);
            context.SaveChanges();
            return $"Successfully imported {sales.Count()}.";
        }


        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            InitializeMapper();
            var customersDto = JsonConvert.DeserializeObject<IEnumerable<CustomerInputDto>>(inputJson);
            var customers = Mapper.Map<IEnumerable<Customer>>(customersDto);
            context.AddRange(customers);
            context.SaveChanges();
            return $"Successfully imported {customers.Count()}.";

        }
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var carsDto = JsonConvert.DeserializeObject<IEnumerable<CarInputDto>>(inputJson);
            var cars = new List<Car>();
            foreach (var dto in carsDto)
            {
                var car = new Car
                {
                    Model = dto.Model,
                    Make = dto.Make,
                    TravelledDistance = dto.TravelledDistance,
                };
                foreach (var id in dto.PartsId.Where(x => context.Parts.Any(p => p.Id == x)))
                {
                    if (!car.Parts.Any(x => x.PartId == id))
                    {
                        car.Parts.Add(new PartCar { PartId = id });
                    }
                }
                cars.Add(car);
            }
            context.Cars.AddRange(cars);
            context.SaveChanges();
            return $"Successfully imported {cars.Count()}.";
        }
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            InitializeMapper();
            var partsDto = JsonConvert.DeserializeObject<IEnumerable<PartInputDto>>(inputJson);
            var parts = Mapper.Map<IEnumerable<Part>>(partsDto.Where(x => context.Suppliers.Any(s => s.Id == x.SupplierId)));
            context.Parts.AddRange(parts);
            context.SaveChanges();
            return $"Successfully imported {parts.Count()}.";
        }
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            InitializeMapper();
            var suppliersDto = JsonConvert.DeserializeObject<IEnumerable<SupplierInputDto>>(inputJson);
            var suppliers = Mapper.Map<IEnumerable<Supplier>>(suppliersDto);
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();
            return $"Successfully imported {suppliers.Count()}.";
        }
        private static void InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
                cfg.AddProfile<CarDealerProfile>()
            );
            Mapper = config.CreateMapper();
        }
    }
}
