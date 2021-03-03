using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P01_StudentSystem.Data.Models
{
    public class Student
    {
        public Student()
        {
            this.CourseEnrollments = new HashSet<StudentCourse>();
            this.HomeworkSubmissions = new HashSet<Homework>();
        }
        public int StudentId { get; set; }

        [Required] //(up to 100 characters, unicode)
        [MaxLength(100)]
        public string Name { get; set; }

        //(exactly 10 characters, not unicode, not required)
        [Column(TypeName ="char(10)")]
        public string PhoneNumber { get; set; }
        
        [Required]
        public DateTime RegisteredOn { get; set; }
        
        //(not required)
        public DateTime? Birthday { get; set; }
        public ICollection<StudentCourse> CourseEnrollments { get; set; }
        public ICollection<Homework> HomeworkSubmissions { get; set; }

    }
}
