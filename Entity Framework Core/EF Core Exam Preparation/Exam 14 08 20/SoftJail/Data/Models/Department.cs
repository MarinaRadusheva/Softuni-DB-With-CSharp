using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.Data.Models
{
    public class Department
    {
        public Department()
        {
            this.Cells = new HashSet<Cell>();
        }
        public int Id { get; set; } //– integer, Primary Key
        [Required]
        [MaxLength(25)]
        public string Name { get; set; } //– text with min length 3 and max length 25 (lic ret quinired)
        public virtual ICollection<Cell> Cells { get; set; } //- collection of type Cell

    }
}