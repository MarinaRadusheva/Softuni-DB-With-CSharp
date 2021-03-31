using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.Data.Models
{
    public class Cell
    {
        public Cell()
        {
            this.Prisoners = new HashSet<Prisoner>();
        }
        public int Id { get; set; }
        public int CellNumber  { get; set; } //integer in the range [1, 1000] (required)
        public bool HasWindow { get; set; } //bool (required)
        public int DepartmentId 
        { get; set; } //- integer, foreign key(required)
        public virtual Department Department { get; set; } //the cell's department (required)
        public virtual ICollection<Prisoner> Prisoners { get; set; }

    }
}