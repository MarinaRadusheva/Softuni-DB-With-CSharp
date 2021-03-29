using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.DTO.Export
{
    [XmlType("customer")]
    public class CustomersExpenditureOutputDto
    {
        [XmlAttribute("full-name")]
        public string Name { get; set; }
        [XmlAttribute("bought-cars")]
        public int CarsCount { get; set; }
        [XmlAttribute("spent-money")]
        public decimal SpentMoney { get; set; }
        
    }
}
