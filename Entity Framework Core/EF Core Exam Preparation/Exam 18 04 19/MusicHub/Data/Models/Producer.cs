using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MusicHub.Data.Models
{
    public class Producer
    {
        public Producer()
        {
            this.Albums = new HashSet<Album>();
        }
        public int Id { get; set; } // – integer, Primary Key
        [Required]
        [MaxLength(30)]
        public string Name { get; set; } //– text with min length 3 and max length 30 (required)
        public string Pseudonym { get; set; } // – text, consisting of two words separated with space and each word must start with one upper letter and continue with mntaic iny  blup  lowercase     letters(Example: "Bon Jovi")
        public string PhoneNumber { get; set; } // – text, consisting only of three groups(separated by space) of three digits and starting always with        blic up"+3int59" (Example:"+359 887 234 267")
        public virtual ICollection<Album> Albums { get; set; } // – collection of type Album

    }
}
