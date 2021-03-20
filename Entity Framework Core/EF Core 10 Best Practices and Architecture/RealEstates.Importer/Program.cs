using RealEstates.Data;
using RealEstates.Models;
using RealEstates.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RealEstates.Importer
{
    class Program
    {
        static void Main(string[] args)
        {
            ImportFromJsonFile("ApartmentsInSofia.json");
            Console.WriteLine();
            ImportFromJsonFile("HousesInSofia.json");
        }

        private static void ImportFromJsonFile(string jsonFileName)
        {
            var dbContext = new ApplicationDbContext();
            IPropertyService service = new PropertyService(dbContext);
            var properties = JsonSerializer.Deserialize<IEnumerable<PropertyAsJson>>(File.ReadAllText(jsonFileName));
            foreach (var jsonProp in properties)
            {
                service.Add(jsonProp.Size, jsonProp.YardSize, jsonProp.Floor, jsonProp.TotalFloors, jsonProp.District, jsonProp.Year, jsonProp.Type, jsonProp.BuildingType, jsonProp.Price);
                Console.WriteLine(".");
            }
        }
    }
}
