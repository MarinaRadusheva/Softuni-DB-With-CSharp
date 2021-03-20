using AutoMapper;
using RealEstates.Services.Profiler;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstates.Services
{
    public abstract class BaseService
    {
        public BaseService()
        {
            InitializeAutomapper();
        }
        protected IMapper Mapper { get; private set; }
        private void InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<RealEstatesProfile>());
            this.Mapper = config.CreateMapper();
        }
    }
}
