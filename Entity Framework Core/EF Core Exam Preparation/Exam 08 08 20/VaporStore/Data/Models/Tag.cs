using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VaporStore.Data.Models
{
    public class Tag
    {
        public Tag()
        {
            this.GameTags = new HashSet<GameTag>();
        }
        //⦁	Id – integer, Primary Key
        public int Id { get; set; }

        //⦁	Name – text(required)
        [Required]
        public string Name { get; set; }

        //⦁	GameTags - collection of type GameTag
        public ICollection<GameTag> GameTags { get; set; }
    }
}