﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.DTO
{
    public class CustomerInputDto
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public bool IsYoungDriver { get; set; }
    }
}
