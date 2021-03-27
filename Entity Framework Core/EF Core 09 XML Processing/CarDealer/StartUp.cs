using CarDealer.Data;
using System.IO;
using System;
using System.Xml.Serialization;
using CarDealer.DTO.Import;
using System.Collections.Generic;
using System.Linq;
using CarDealer.Models;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            CarDealerContext context = new CarDealerContext();
            string suppliersXml = File.ReadAllText(@"../../../Datasets/suppliers.xml");
            string carsXml = File.ReadAllText(@"../../../Datasets/cars.xml");
            
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