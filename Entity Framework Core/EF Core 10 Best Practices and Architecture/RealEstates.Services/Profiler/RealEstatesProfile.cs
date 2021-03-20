using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using RealEstates.Models;
using RealEstates.Services.Models;

namespace RealEstates.Services.Profiler
{
    public class RealEstatesProfile : Profile
    {
        public RealEstatesProfile()
        {
            this.CreateMap<Property, PropertyInfoDto>()
                .ForMember(x => x.PropertyType, d => d.MapFrom(x => x.Type.Name));
            this.CreateMap<District, DistrictInfoDto>()
                .ForMember(x => x.AveragePricePerSqareMetre, a => a.MapFrom(x => (decimal)x.Properties.Where(x => x.Price.HasValue).Average(p => p.Price / (decimal)p.Size)))
                .ForMember(x=>x.PropertiesCount, c=>c.MapFrom(x=>x.Properties.Count()));
            this.CreateMap<Property, TopFloorFullInfoDto>()
                .ForMember(x => x.PropertyType, d => d.MapFrom(x => x.Type.Name));
            this.CreateMap<Tag, TagInfoDto>();
        }
    }
}
