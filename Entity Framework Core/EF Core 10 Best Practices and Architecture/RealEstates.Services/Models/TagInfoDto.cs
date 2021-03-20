using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace RealEstates.Services.Models
{
    [XmlType("Tag")]
    public class TagInfoDto
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }
    }
}
