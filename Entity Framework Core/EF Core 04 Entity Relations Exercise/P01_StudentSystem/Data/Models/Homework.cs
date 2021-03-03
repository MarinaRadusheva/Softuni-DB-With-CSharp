using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P01_StudentSystem.Data.Models
{
    public enum ContentTypeEnum
    {
        Application=1, 
        Pdf=2,
        Zip=3
    }
    public class Homework
    {
        [Key]
        public int HomeworkId { get; set; }

        //(string, linking to a file, not unicode)
        [Required]
        [Column(TypeName ="varchar(255)")]
        public string Content { get; set; }

        //(enum – can be Application, Pdf or Zip)
        [Required]
        public ContentTypeEnum ContentType { get; set; }

        [Required]
        public DateTime SubmissionTime { get; set; }

        [ForeignKey(nameof(Student))]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }
        public Course Course { get; set; }

    }
}
