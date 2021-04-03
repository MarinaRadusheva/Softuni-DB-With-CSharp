using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace MusicHub.DataProcessor.ImportDtos
{
    [XmlType("Performer")]
    public class PerformerXmlDto
    {
        [Required]
        [MaxLength(20)]
        [MinLength(3)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(20)]
        [MinLength(3)]
        public string LastName { get; set; }
        [Required]
        [Range(18, 70)]
        public int Age { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal NetWorth { get; set; }
        [XmlArray("PerformersSongs")]
        public SongIdXmlDto[] Songs { get; set; }
    }
}
