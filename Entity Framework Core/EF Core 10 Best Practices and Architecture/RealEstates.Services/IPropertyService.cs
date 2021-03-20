using RealEstates.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstates.Services
{
    public interface IPropertyService
    {
        void Add(int size, int yardSize, int floor, int totalFloors, string district, int year, string type, string buildingType, int price);
        IEnumerable<PropertyInfoDto> Search(int minPrice, int maxPrice, int minSize, int maxSize);
        IEnumerable<TopFloorFullInfoDto> GetFullInfoTopFloors(int count);
    }
}
