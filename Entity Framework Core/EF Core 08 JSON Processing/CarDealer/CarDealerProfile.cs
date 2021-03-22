using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using System;
using System.Linq;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            
            CreateMap<SupplierInputDto, Supplier>();
            CreateMap<PartInputDto, Part>();
            CreateMap<CustomerInputDto, Customer>();
            CreateMap<SaleInputDto, Sale>();
            CreateMap<Customer, CustomerWithCarDto>()
                .ForMember(x => x.FullName, n => n.MapFrom(p => p.Name))
                .ForMember(x => x.BoughtCars, n => n.MapFrom(p => p.Cars.Count))
                .ForMember(x => x.SpentMoney, n => n.MapFrom(p => p.Cars
                                                            .Select(c => c.Car.Parts
                                                            .Select(x => x.Part)
                                                            .Sum(x => x.Price))
                                                            .Sum()));
        }
    }
}
