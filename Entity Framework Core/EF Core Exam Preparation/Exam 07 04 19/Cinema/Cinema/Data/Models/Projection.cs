using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Data.Models
{
    public class Projection
    {
        public Projection()
        {
            this.Tickets = new HashSet<Ticket>();
        }
        public int Id { get; set; }
        [ForeignKey("Movie")]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        [ForeignKey("Hall")]
        public int HallId { get; set; }
        public Hall Hall { get; set; }
        public DateTime DateTime { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}