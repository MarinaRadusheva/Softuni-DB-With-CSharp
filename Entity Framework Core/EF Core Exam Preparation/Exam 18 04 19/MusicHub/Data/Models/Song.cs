using MusicHub.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MusicHub.Data.Models
{
    public class Song
    {
        public Song()
        {
			this.SongPerformers = new HashSet<SongPerformer>();
        }
	   public int Id { get; set; } // – integer, Primary Key
		[Required]
		[MaxLength(20)]
	   public string Name { get; set; } // – text with min length 3 and max length 20 (required)
		[Required]
		public TimeSpan Duration { get; set; } // – TimeSpan(required)

		public DateTime CreatedOn { get; set; } // – Date(required)
		[Required]
		public Genre Genre { get; set; } // – Genre enumeration with possible values: "Blues, Rap, PopMusic, Rock, Jazz" (required)
		[ForeignKey(nameof(Album))]
		public int? AlbumId { get; set; } //– integer foreign key
		public virtual Album Album { get; set; } //– the song’s album
		[ForeignKey(nameof(Writer))]
		public int WriterId { get; set; } // - integer, foreign key(required)
		public virtual Writer Writer { get; set; } // – the song’s writer
		[Range(0.01, double.MaxValue)]
		public decimal Price { get; set; } // – decimal (non-negative, minimum value: 0) (required)
		public virtual ICollection<SongPerformer> SongPerformers { get; set; } // - collection of type SongPerformer

	}
}
