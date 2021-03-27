using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    
    public class UserWithSoldProductDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }
        [XmlElement("lastName")]
        public string LastName { get; set; }
        [XmlArray("soldProducts")]
        public virtual ProductNamePriceDto[] Products { get; set; }
    }
}
