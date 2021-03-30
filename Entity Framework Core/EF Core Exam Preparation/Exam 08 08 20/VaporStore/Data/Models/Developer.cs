using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VaporStore.Data.Models
{
    public class Developer
    {
        public Developer()
        {
            this.Games = new HashSet<Game>();
        }
        //⦁	Id – integer, Primary Key
        public int Id { get; set; }

        //⦁	Name – text(required)
        [Required]
        public string Name { get; set; }

        //⦁	Games - collection of type Game
        public ICollection<Game> Games { get; set; }
    }
}