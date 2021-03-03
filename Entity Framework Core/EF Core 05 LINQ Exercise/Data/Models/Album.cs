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
        public int Id { get; set; }
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }
        [Required]
        [Column(TypeName = "Date")]
        public DateTime ReleaseDate { get; set; }

        public decimal Price
        {
            get
            {
               return (decimal)this.Songs.Sum(x => x.Price);               
            }
            private set { }
        }

        [ForeignKey(nameof(Producer))]
        public int? ProducerId { get; set; }
        public virtual Producer Producer { get; set; }
        public virtual ICollection<Song> Songs { get; set; }

    }
}
