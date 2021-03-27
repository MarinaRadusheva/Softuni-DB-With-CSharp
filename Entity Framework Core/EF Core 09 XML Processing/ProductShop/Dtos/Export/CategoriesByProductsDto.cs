using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("Category")]
    public class CategoriesByProductsDto
    {
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("count")]
        public int ProductsCount { get; set; }
        [XmlElement("averagePrice")]
        public string AveragePrice { get; set; }
        [XmlElement("totalRevenue")]
        public decimal Revenue { get; set; }
    }
}
