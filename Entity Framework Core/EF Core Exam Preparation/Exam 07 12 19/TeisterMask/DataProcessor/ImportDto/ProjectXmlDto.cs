using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType("Project")]
    public class ProjectXmlDto
    {
        [Required]
        [MaxLength(40)]
        [MinLength(2)]
        public string Name { get; set; }
        [Required]
        public string OpenDate { get; set; }
        public string DueDate { get; set; }
        [XmlArray]
        public TaskXmlDto[] Tasks { get; set; }
    }
}
