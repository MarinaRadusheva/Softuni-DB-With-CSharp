﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.Models
{
    public class Part
    {
        public Part()
        {
            this.Cars = new HashSet<PartCar>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<PartCar> Cars { get; set; }
    }
}
