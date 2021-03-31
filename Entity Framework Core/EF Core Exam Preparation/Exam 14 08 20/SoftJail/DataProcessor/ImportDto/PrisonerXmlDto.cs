using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Prisoner")]
    public class PrisonerXmlDto
    {
        
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
