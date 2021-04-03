using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MusicHub.Data.Models
{
    public class Writer
    {
	  public int Id { get; set; } // – integer, Primary Key
		[Required]
		[MaxLength(20)]
		public string Name { get; set; } //– text with min length 3 and max length 20 (required)

		public string Pseudonym { get; set; } // – text, consisting of two words separated with space and each word must start with one upper letter and continue with many int  blic uplowercase   letters(Example: "Freddie Mercury")
		public virtual ICollection<Song> Songs { get; set; } // – collection of type Song

	}
}
