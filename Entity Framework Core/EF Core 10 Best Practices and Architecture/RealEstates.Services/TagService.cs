using RealEstates.Data;
using RealEstates.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RealEstates.Services
{
    public class TagService : ITagService
    {
        private readonly ApplicationDbContext context;
        public TagService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void Add(string name, int? importance)
        {
            Tag tag = new Tag { Name = name, Importance = importance };
            context.Tags.Add(tag);
            context.SaveChanges();
        }

        public void BulkTagProperties()
        {
            var properties = context.Properties.ToList();
            foreach (var prop in properties)
            {
                decimal averagePrice = GetAveragePrice(prop.DistrictId);
                decimal pricePerSquareMetre = GetPricePerSquareMetre(prop.Price, prop.Size);
                Tag priceTag;
                if (pricePerSquareMetre>averagePrice)
                {
                    priceTag = context.Tags.FirstOrDefault(x => x.Name == "скъп-имот");
                    prop.Tags.Add(priceTag);
                }
                if (pricePerSquareMetre<=averagePrice)
                {
                    priceTag = context.Tags.FirstOrDefault(x => x.Name == "евтин-имот");
                    prop.Tags.Add(priceTag);
                }
                var year = DateTime.Now.AddYears(-10).Year;
                Tag ageTag;
                if (prop.Year.HasValue && prop.Year>year)
                {
                    ageTag = context.Tags.FirstOrDefault(x => x.Name == "нов-имот");
                    prop.Tags.Add(ageTag);
                }
                if (prop.Year.HasValue && prop.Year<=year)
                {
                    ageTag = context.Tags.FirstOrDefault(x => x.Name == "стар-имот");
                    prop.Tags.Add(ageTag);
                }
                Tag sizeTag;
                if (prop.Size<=100)
                {
                    sizeTag = context.Tags.FirstOrDefault(x => x.Name == "малък-имот");
                    prop.Tags.Add(sizeTag);
                }
                else if (prop.Size>100&&prop.Size<=300)
                {
                    sizeTag = context.Tags.FirstOrDefault(x => x.Name == "голям-имот");
                    prop.Tags.Add(sizeTag);
                }
                else
                {
                    sizeTag = context.Tags.FirstOrDefault(x => x.Name == "огромен-имот");
                    prop.Tags.Add(sizeTag);
                }
                Tag floorTag;
                if (prop.Floor.HasValue && prop.Floor==1)
                {
                    floorTag = context.Tags.FirstOrDefault(x => x.Name == "първи-етаж");
                    prop.Tags.Add(floorTag);
                }
                if (prop.Floor.HasValue && prop.TotalFloors.HasValue && prop.Floor==prop.TotalFloors)
                {
                    floorTag = context.Tags.FirstOrDefault(x => x.Name == "последен-етаж");
                    prop.Tags.Add(floorTag);
                }
            }
            context.SaveChanges();
        }

        private decimal GetAveragePrice(int districtId)
        {
            return  context.Properties.Where(x => x.DistrictId == districtId && x.Price.HasValue).Average(x => x.Price / (decimal)x.Size) ?? 0;
        }
        private decimal GetPricePerSquareMetre(int? price, int size)
        {
            return price / (decimal)size ?? 0;
        }
    }
}
