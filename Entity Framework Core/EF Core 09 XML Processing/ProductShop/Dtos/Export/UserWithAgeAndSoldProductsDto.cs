﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{


    [XmlType("User")]
    public class UserWithAgeAndSoldProductsDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }     
        
        [XmlElement("age")]
        public int? Age { get; set; }

        
        public SoldProductsDto SoldProducts { get; set; }

        //[XmlIgnore]
        //public int ProductsCount { get; set; }

    }
}
