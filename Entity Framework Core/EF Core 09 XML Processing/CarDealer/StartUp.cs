using CarDealer.Data;
using System.IO;
using System;
using System.Xml.Serialization;
using CarDealer.DTO.Import;
using System.Collections.Generic;
using System.Linq;
using CarDealer.Models;
using CarDealer.DTO.Export;
using System.Text;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            CarDealerContext context = new CarDealerContext();
            //string suppliersXml = File.ReadAllText(@"../../../Datasets/suppliers.xml");
            //string carsXml = File.ReadAllText(@"../../../Datasets/cars.xml");
            Console.WriteLine(GetSalesWithAppliedDiscount(context));
        }
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            //all sales - car, customer and price of the sale with and without discount.
            var sales = context.Sales
                .Select(x => new SalesWithDiscountsOutputDto()
                {
                    Car = new CarsFromSalesOutputDto()
                    {
                        Make = x.Car.Make,
                        Model = x.Car.Model,
                        Distance = x.Car.TravelledDistance,
                    },
                    Discount = x.Discount,
                    Name = x.Customer.Name,
                    Price = x.Car.PartCars.Sum(p => p.Part.Price),
                    DiscountedPrice = x.Car.PartCars.Sum(p => p.Part.Price)- x.Car.PartCars.Sum(p => p.Part.Price)*x.Discount / 100
                })
                .ToList();
            XmlSerializer serializer = new XmlSerializer(typeof(List<SalesWithDiscountsOutputDto>), new XmlRootAttribute("sales"));
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            using (writer)
            {
                serializer.Serialize(writer, sales, namespaces);
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            //customers that have bought at least 1 car(names, cars count and total spent money) Order by total spent money descending.
            var customers = context.Customers
                .Where(x => context.Sales.Any(s => s.CustomerId == x.Id))
                .Select(x => new CustomersExpenditureOutputDto()
                {
                    Name = x.Name,
                    CarsCount = x.Sales.Count,
                    SpentMoney = x.Sales.Select(p => p.Car.PartCars.Select(c => c.Part).Sum(s=>s.Price)).Sum()
                })
                .OrderByDescending(x => x.SpentMoney)
                .ToList();
            XmlSerializer serializer = new XmlSerializer(typeof(List<CustomersExpenditureOutputDto>), new XmlRootAttribute("customers"));
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            using (writer)
            {
                serializer.Serialize(writer, customers, namespaces);
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            //cars (make, model and travelled distance) along with their parts (name and price, order by price desc) Sort all cars by travelled distance(descending) then by model(ascending).Select top 5 records
            var cars = context.Cars
                .Select(x => new CarsWithPartsOutputDto()
                {
                    Make = x.Make,
                    Model = x.Model,
                    Distance = x.TravelledDistance,
                    Parts = x.PartCars.Select(x => new PartsNamePriceOutputDto()
                    {
                        Name = x.Part.Name,
                        Price = x.Part.Price,
                    }).OrderByDescending(p => p.Price)
                    .ToArray()
                })
                .OrderByDescending(x => x.Distance)
                .ThenBy(x => x.Model)
                .Take(5)
                .ToList();
            XmlSerializer serializer = new XmlSerializer(typeof(List<CarsWithPartsOutputDto>), new XmlRootAttribute("cars"));
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            using (writer)
            {
                serializer.Serialize(writer, cars, namespaces);
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            //suppliers that do not import parts. id, name and the number of parts they can offer to supply.
            var suppliers = context.Suppliers
                .Where(x => x.IsImporter == false)
                .Select(x => new NonImportersOutputDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Parts = x.Parts.Count,
                })
                .ToList();
            XmlSerializer serializer = new XmlSerializer(typeof(List<NonImportersOutputDto>), new XmlRootAttribute("suppliers"));
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            using (writer)
            {
                serializer.Serialize(writer, suppliers, namespaces);
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            //BMWs - order by model alphabetically and by travelled distance descending.
            var cars = context.Cars
                .Where(x => x.Make == "BMW")
                .Select(x => new BMWsOutputDto()
                {
                    Id = x.Id,
                    Model = x.Model,
                    Distance = x.TravelledDistance,
                })
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.Distance)
                .ToList();
            XmlSerializer serializer = new XmlSerializer(typeof(List<BMWsOutputDto>), new XmlRootAttribute("cars"));
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            using (writer)
            {
                serializer.Serialize(writer, cars, namespaces);
            }
            return sb.ToString().TrimEnd();

        }
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            //cars with distance > 2,000,000. Order by make, then by model alphabetically. Take 10.
            var cars = context.Cars
                .Where(x => x.TravelledDistance > 2000000)
                .Select(x => new CarsWithDistanceOutputDto()
                {
                    Make = x.Make,
                    Model = x.Model,
                    Distance = x.TravelledDistance,
                })
                .OrderBy(x => x.Make)
                .ThenBy(x => x.Model)
                .Take(10)
                .ToList();
            XmlSerializer serializer = new XmlSerializer(typeof(List<CarsWithDistanceOutputDto>), new XmlRootAttribute("cars"));
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            using (writer)
            {
                serializer.Serialize(writer, cars, namespaces);
            }           
            return sb.ToString().TrimEnd();
        }
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            StringReader reader = new StringReader(inputXml);
            XmlSerializer serializer = new XmlSerializer(typeof(List<SalesInputDto>), new XmlRootAttribute("Sales"));
            var deserializedSales = (List<SalesInputDto>)serializer.Deserialize(reader);
            var carsIds = context.Cars.Select(x => x.Id).ToList();
            var sales = deserializedSales
                .Where(x => carsIds.Contains(x.CarId))
                .Select(x => new Sale()
                {
                    CarId = x.CarId,
                    CustomerId = x.CustomerId,
                    Discount = x.Discount,
                })
                .ToList();
            context.Sales.AddRange(sales);
            context.SaveChanges();
            return $"Successfully imported {sales.Count}";
        }
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            StringReader reader = new StringReader(inputXml);
            XmlSerializer serializer = new XmlSerializer(typeof(List<CustomersInputDto>), new XmlRootAttribute("Customers"));
            var deserializedCustomers = (List<CustomersInputDto>)serializer.Deserialize(reader);
            var customers = deserializedCustomers.Select(x => new Customer
            {
                Name = x.Name,
                BirthDate = x.BirthDate,
                IsYoungDriver = x.IsYoungDriver,
            })
                .ToList();
            context.Customers.AddRange(customers);
            context.SaveChanges();
            return $"Successfully imported {customers.Count}";
        }
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            StringReader reader = new StringReader(inputXml);
            XmlSerializer serializer = new XmlSerializer(typeof(List<CarsInputDto>), new XmlRootAttribute("Cars"));
            var deserializedCars = (List<CarsInputDto>)serializer.Deserialize(reader);
            var partsIds = context.Parts.Select(x => x.Id).ToList();
            List<Car> cars = new List<Car>();
            foreach (var car in deserializedCars)
            {
                var newCar = new Car()
                {
                    Make = car.Make,
                    Model = car.Model,
                    TravelledDistance = car.TravelledDistance,
                };
                foreach (var part in car.Parts.Select(x=>x.Id).Distinct())
                {
                    if (partsIds.Contains(part))
                    {
                        newCar.PartCars.Add(new PartCar() {PartId=part });
                    }
                }
                cars.Add(newCar);
            }
            context.Cars.AddRange(cars);
            context.SaveChanges();
            return $"Successfully imported {cars.Count}";
        }
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            StringReader reader = new StringReader(inputXml);
            XmlSerializer serializer = new XmlSerializer(typeof(List<PartsInputDto>), new XmlRootAttribute("Parts"));
            var deserializedParts = (List<PartsInputDto>)serializer.Deserialize(reader);
            var parts = deserializedParts
                .Where(x => context.Suppliers.Any(s => s.Id == x.SupplierId))
                .Select(x => new Part()
                {
                    Name = x.Name,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    SupplierId = x.SupplierId,
                })
                .ToList();
            context.Parts.AddRange(parts);
            context.SaveChanges();
            return $"Successfully imported {parts.Count}";
        }
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            StringReader reader = new StringReader(inputXml);
            XmlSerializer serializer = new XmlSerializer(typeof(List<SupplierInputDto>), new XmlRootAttribute("Suppliers"));
            var suppliersDeserialized = (List<SupplierInputDto>)serializer.Deserialize(reader);
            var suppliers = suppliersDeserialized
                .Select(x => new Supplier()
                {
                    Name = x.Name,
                    IsImporter = x.IsImporter,
                })
                .ToList();
            context.AddRange(suppliers);
            context.SaveChanges();
            return $"Successfully imported {suppliers.Count()}";
        }
    }
}