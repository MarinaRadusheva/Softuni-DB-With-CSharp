using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cinema.Data.Models
{
    public class Customer
    {
        public Customer()
        {
            this.Tickets = new HashSet<Ticket>();
        }
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }
        [Range(12,110)]
        public int Age { get; set; }
        [Range(0.01, double.MaxValue)]
        public decimal Balance { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}
