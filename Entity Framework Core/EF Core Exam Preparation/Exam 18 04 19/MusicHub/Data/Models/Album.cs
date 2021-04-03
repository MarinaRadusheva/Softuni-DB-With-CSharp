using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MusicHub.Data.Models
{
    public class Album
    {
        public Album()
        {
			this.Songs = new HashSet<Song>();
        }
	  public int Id { get; set; } // – integer, Primary Key
		[Required]
		[MaxLength(40)]
		public string Name { get; set; } // – text with min length 3 and max length 40 (required)
		public DateTime ReleaseDate { get; set; } // – Date(required)
		[NotMapped]
		public decimal Price => this.Songs.Sum(x => x.Price);

		[ForeignKey(nameof(Producer))]
		public int? ProducerId { get; set; } // – integer foreign key
		public virtual Producer Producer { get; set; } // – the album’s producer
		public virtual ICollection<Song> Songs { get; set; } // – collection of all songs in the album

	}
}
