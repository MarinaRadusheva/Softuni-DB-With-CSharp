using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.DTO.Import
{
    [XmlType("Part")]
    public class PartsInputDto
    {
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("price")]
        public decimal Price { get; set; }
        [XmlElement("quantity")]
        public int Quantity { get; set; }
        [XmlElement("supplierId")]
        public int SupplierId { get; set; }
      

    }
}
