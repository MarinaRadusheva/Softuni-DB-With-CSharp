using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
using VaporStore.Data.Models.Enums;

namespace VaporStore.DataProcessor.Dto.Import
{
    [XmlType("Purchase")]
    public class PurchaseImportDto
    {
        [Required]
        [XmlAttribute("title")]
        public string Title { get; set; }
        [XmlElement]
        public PurchaseType? Type { get; set; }

        [XmlElement]
        [RegularExpression("^[A-Z,0-9]{4}-[A-Z,0-9]{4}-[A-Z,0-9]{4}$")]
        public string Key { get; set; }
        [Required]
        [XmlElement]
        [RegularExpression(@"^[0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4}$")]
        public string Card { get; set; }

        [XmlElement]
        public string Date { get; set; }
        
    }
}
