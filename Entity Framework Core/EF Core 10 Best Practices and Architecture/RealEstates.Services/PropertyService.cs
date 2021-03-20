using RealEstates.Data;
using RealEstates.Models;
using RealEstates.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper.QueryableExtensions;
namespace RealEstates.Services
{
    public class PropertyService : BaseService, IPropertyService
    {
        private readonly ApplicationDbContext dbContext;
        public PropertyService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Add(int size, int yardSize, int floor, int totalFloors, string district, int year, string type, string buildingType, int price)
        {
            byte? floor1 = (byte)floor;
            if (floor<=0||floor>255)
            {
                floor1 = null;
            }
            byte? totalFloors1 = (byte)totalFloors;
            if (totalFloors <= 0 || totalFloors>255)
            {
                totalFloors1 = null;
            }
            int? year1 = year;
            if (year<1800)
            {
                year1 = null;
            }
            int? yardSize1 = yardSize;
            if (yardSize < 1800)
            {
                yardSize1 = null;
            }
            int? price1 = price;
            if (price<=0)
            {
                price1 = null;
            }
            var property = new Property
            {
                Size = size,
                YardSize = yardSize,
                Floor = floor1,
                TotalFloors = totalFloors1,
                District = dbContext.Districts.FirstOrDefault(x => x.Name == district) ?? new District { Name = district },
                Year = year1,
                Type = dbContext.PropertyTypes.FirstOrDefault(x=>x.Name==type) ?? new PropertyType {  Name = type},
                BuildingType = dbContext.BuildingTypes.FirstOrDefault(x=>x.Name==buildingType) ?? new BuildingType {  Name=buildingType},
                Price = price1,
            };
            dbContext.Properties.Add(property);
            dbContext.SaveChanges();
        }

        public IEnumerable<PropertyInfoDto> Search(int minPrice, int maxPrice, int minSize, int maxSize)
        {
            var propertiesMatch = dbContext.Properties.Where(x => x.Price >= minPrice && x.Price <= maxPrice && x.Size >= minSize && x.Size <= maxSize).ProjectTo<PropertyInfoDto>(this.Mapper.ConfigurationProvider)
                .ToList();
            return propertiesMatch;
        }
        public IEnumerable<TopFloorFullInfoDto> GetFullInfoTopFloors(int count)
        {
            var properties = dbContext.Properties
                .Where(x => x.Floor.HasValue && x.TotalFloors.HasValue && x.TotalFloors > 1 && x.Tags.Any(x => x.Name == "последен-етаж"))
                .ProjectTo<TopFloorFullInfoDto>(this.Mapper.ConfigurationProvider)
                .OrderByDescending(x=>x.Price)
                .ThenByDescending(x=>x.Size)
                .Take(count)
                .ToArray();
            return properties;
        }

    }
}
