using Microsoft.EntityFrameworkCore;
using RealEstates.Data;
using RealEstates.Services;
using RealEstates.Services.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RealEstates.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
            var db = new ApplicationDbContext();
            db.Database.Migrate();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Property Search");
                Console.WriteLine("2. Most Expensive Districts");
                Console.WriteLine("3. Add Tags");
                Console.WriteLine("4. Tag properties");
                Console.WriteLine("5. Top floor properties info");
                Console.WriteLine("0. EXIT");
                bool parsed = int.TryParse(Console.ReadLine(), out int option);
                if (parsed && option==0)
                {
                    break;
                }
                if (parsed && (option>=1 && option<=5))
                {
                    switch (option)
                    {
                        case 1:
                            PropertySearch(db);
                            break;
                        case 2:
                            MostExpensiveDistrict(db);
                            break;
                        case 3:
                            AddTag(db);
                            break;
                        case 4:
                            TagAllProperties(db);
                            break;
                        case 5:
                            GetFullInfoOfTopFloors(db);
                            break;
                    
                    }
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
                
            }
        }

        private static void GetFullInfoOfTopFloors(ApplicationDbContext db)
        {
            Console.WriteLine("Count of most expensive properties:");
            int count = int.Parse(Console.ReadLine());
            IPropertyService service = new PropertyService(db);
            var properties = service.GetFullInfoTopFloors(count);
            var xmlSerializer = new XmlSerializer(typeof(TopFloorFullInfoDto[]));
            var stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, properties);
            Console.WriteLine(stringWriter.ToString());
            //foreach (var prop in properties)
            //{
            //    Console.WriteLine(prop.Id);
            //    Console.WriteLine(prop.BuildingTypeName);
            //    Console.WriteLine(prop.DistrictName);
            //    Console.WriteLine(prop.Price);
            //    Console.WriteLine(prop.Size);
            //    Console.WriteLine(prop.Floor);
            //    Console.WriteLine(prop.PropertyType);
            //    Console.WriteLine(prop.Year);
            //    foreach (var tag in prop.Tags)
            //    {
            //        Console.WriteLine(tag.Name);
            //    }
            //    Console.WriteLine();
            //}
        }

        private static void TagAllProperties(ApplicationDbContext db)
        {
            Console.WriteLine("Tagging started.");
            ITagService service = new TagService(db);
            service.BulkTagProperties();
            Console.WriteLine("Tagging finished");
        }

        private static void AddTag(ApplicationDbContext db)
        {
            
            ITagService service = new TagService(db);
            while (true)
            {
                Console.WriteLine("Tag name:");
                string tagName = Console.ReadLine();
                if (db.Tags.FirstOrDefault(x=>x.Name==tagName)!=null)
                {
                    Console.WriteLine("Tag already added!");
                    continue;
                }
                Console.WriteLine("Tag importance(optional)");
                bool importanceParsed = int.TryParse(Console.ReadLine(), out int importanceInput);
                int? importance = null;
                if (importanceParsed)
                {
                    importance = importanceInput;
                }
                service.Add(tagName, importance);
                Console.WriteLine("Continue adding tags? Press 'N' or 'H' for NO");
                var key = Console.ReadKey();
                Console.WriteLine();
                if (key.KeyChar=='N'|| key.KeyChar=='Н')
                {
                    break;
                }
                
            }
        }

        private static void MostExpensiveDistrict(ApplicationDbContext db)
        {
            Console.WriteLine("District count:");
            int districtCount = int.Parse(Console.ReadLine());
            IDistrictService service = new DistrictService(db);
            var districts = service.GetMostExpensiveDistricts(districtCount);
            foreach (var district in districts)
            {
                Console.WriteLine($"{district.Name} - {district.AveragePricePerSqareMetre:0.00}€/m² ({district.PropertiesCount})");
            }
        }

        private static void PropertySearch(ApplicationDbContext db)
        {
            Console.WriteLine("Min Price:");
            int minPrice = int.Parse(Console.ReadLine());
            Console.WriteLine("Max Price:");
            int maxPrice = int.Parse(Console.ReadLine());
            Console.WriteLine("Min Size:");
            int minSize = int.Parse(Console.ReadLine());
            Console.WriteLine("Max size:");
            int maxSize = int.Parse(Console.ReadLine());
            IPropertyService service = new PropertyService(db);
            IEnumerable<PropertyInfoDto> properties = service.Search(minPrice, maxPrice, minSize, maxSize);
            foreach (var property in properties)
            {
                Console.WriteLine($"{property.DistrictName}; {property.BuildingTypeName}; {property.PropertyType}; {property.Size}m² => {property.Price}€");
            }
        }
    }
}
