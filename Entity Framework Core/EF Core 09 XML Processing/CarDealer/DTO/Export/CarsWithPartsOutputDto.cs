using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.DTO.Export
{
    [XmlType("car")]
    public class CarsWithPartsOutputDto
    {

        [XmlAttribute("make")]
        public string Make { get; set; }
        [XmlAttribute("model")]
        public string Model { get; set; }
        [XmlAttribute("travelled-distance")]
        public long Distance { get; set; }
        [XmlArray("parts")]
        public PartsNamePriceOutputDto[] Parts { get; set; }
    }
}
