using RealEstates.Data;
using RealEstates.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper.QueryableExtensions;
namespace RealEstates.Services
{
    public class DistrictService : BaseService, IDistrictService
    {
        private readonly ApplicationDbContext context;
        public DistrictService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<DistrictInfoDto> GetMostExpensiveDistricts(int count)
        {
            var districts = context.Districts.ProjectTo<DistrictInfoDto>(this.Mapper.ConfigurationProvider)
            //    .Select(x => new DistrictInfoDto
            //{
            //    Name = x.Name,
            //    PropertiesCount = x.Properties.Count(),
            //    AveragePricePerSqareMetre = (decimal)x.Properties.Sum(p => p.Price / (decimal)p.Size) / x.Properties.Count()
            //})
            .OrderByDescending(x=>x.AveragePricePerSqareMetre)
            .Take(count)
            .ToList();
            return districts;
        }
    }
}
