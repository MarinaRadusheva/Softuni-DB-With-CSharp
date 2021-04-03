using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MusicHub.Data.Models
{
    public class Performer
    {
        public Performer()
        {
            this.PerformerSongs = new HashSet<SongPerformer>();
        }
      public int Id { get; set; } // – integer, Primary Key
        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; } //– text with min length 3 and max length 20 (required) 
        [Required]
        [MaxLength(20)]
        public string LastName { get; set; } //– text with min length 3 and max length 20 (required) 
        [Range(18,70)]
        public int Age { get; set; } // – integer(in range between 18 and 70) (required)
        [Range(0, double.MaxValue)]
        public decimal NetWorth { get; set; } // – decimal (non-negative, minimum value: 0) (required)
        public virtual ICollection<SongPerformer> PerformerSongs { get; set; } // - collection of type SongPerformer

    }
}
