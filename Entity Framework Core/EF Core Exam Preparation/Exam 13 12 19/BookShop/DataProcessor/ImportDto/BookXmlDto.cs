using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace BookShop.DataProcessor.ImportDto
{
    [XmlType("Book")]
    public class BookXmlDto
    {
        [Required]
        [MaxLength(30)]
        [MinLength(3)]
        public string Name { get; set; } //- text with length[3, 30]. (required)
        [Required]
        [Range(1, 3)]
        public string Genre { get; set; } 
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; } //- decimal in range between 0.01 and max value of decimal
        [Range(50, 5000)]
        public int Pages { get; set; } //– integer in range between 50 and 5000
        public string PublishedOn { get; set; } //- date and time(required)
    }
}
