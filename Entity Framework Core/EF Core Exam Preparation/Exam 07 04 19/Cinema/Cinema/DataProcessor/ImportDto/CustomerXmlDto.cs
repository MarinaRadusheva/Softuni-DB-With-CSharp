using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Cinema.DataProcessor.ImportDto
{
    [XmlType("Customer")]
    public class CustomerXmlDto
    {
        [Required]
        [MaxLength(20)]
        [MinLength(3)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(20)]
        [MinLength(3)]
        public string LastName { get; set; }
        [Required]
        [Range(12, 110)]
        public int Age { get; set; }
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Balance { get; set; }
        [XmlArray]
        public TicketXmlDto[] Tickets { get; set; }
    }
}
