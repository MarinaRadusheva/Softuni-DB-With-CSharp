using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Data.Models
{
    public class SongPerformer
    {
	  public int SongId { get; set; } // – integer, Primary Key
		public virtual Song Song { get; set; } // – the performer’s song(required)
		public int PerformerId { get; set; } // – integer, Primary Key
		public virtual Performer Performer { get; set; } // – the song’s performer(required)

	}
}
