using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CarDealer.Models
{
    public class Car
    {
        public Car()
        {
            this.Customers = new HashSet<Sale>();
            this.Parts = new HashSet<PartCar>();
        }
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public long TravelledDistance { get; set; }
        public ICollection<PartCar> Parts { get; set; }
        public ICollection<Sale> Customers { get; set; }
    }
}
