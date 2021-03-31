using SoftJail.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
	[XmlType("Officer")]
    public class OfficerXmlDto
	{
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Name { get; set; } //text with min length 3 and max length 30 (required)
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Money { get; set; } // (non-negative, minimum value: 0) (required)
        [Required]
        public string Position { get; set; }
        [Required]
        public string Weapon { get; set; }
        public int DepartmentId { get; set; }
        public PrisonerXmlDto[] Prisoners { get; set; }


	}
}
