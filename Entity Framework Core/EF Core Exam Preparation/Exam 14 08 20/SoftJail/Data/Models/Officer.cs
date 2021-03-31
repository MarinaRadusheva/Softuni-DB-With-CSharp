using SoftJail.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.Data.Models
{
    public class Officer
    {
        public Officer()
        {
            this.OfficerPrisoners = new HashSet<OfficerPrisoner>();
        }
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string FullName { get; set; } //text with min length 3 and max length 30 (required)
        public decimal Salary { get; set; } //(non-negative, minimum value: 0) (required)

        [Required]
        public Position Position { get; set; }
        [Required]
        public Weapon Weapon { get; set; }
        public int DepartmentId  { get; set; } //(required)

        public virtual Department Department { get; set; } //(required)

        public virtual ICollection<OfficerPrisoner> OfficerPrisoners { get; set; }
    }
}
