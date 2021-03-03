using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P01_StudentSystem.Data.Models
{
    public enum RecourceTypeEnum
    {
        Video=1,
        Presentation=2,
        Document=3,
        Other=4
    }
    public class Resource
    {
        public int ResourceId {get; set; }

        //(up to 50 characters, unicode)
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        //(not unicode)
        [Required]
        [Column(TypeName ="varchar(max)")]
        public string Url { get; set; }

        //(enum – can be Video, Presentation, Document or Other)
        [Required]
        public RecourceTypeEnum ResourceType { get; set; }
    
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }

    }
}
