using MusicHub.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace MusicHub.DataProcessor.ImportDtos
{
    [XmlType("Song")]
    public class SongXmlDto
    {
        [Required]
        [MaxLength(20)]
        [MinLength(3)]
        public string Name { get; set; }
        [Required]
        public string Duration { get; set; }
        [Required]
        public string CreatedOn { get; set; }
        [Required]
        [EnumDataType(typeof(Genre))]
        public string Genre { get; set; }

        public int? AlbumId { get; set; }
        public int WriterId { get; set; }
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
 
    }
}
