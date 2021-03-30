using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VaporStore.Data.Models
{
    public class User
    {
        public User()
        {
            this.Cards = new HashSet<Card>();
        }
        //⦁	Id – integer, Primary Key
        public int Id { get; set; }

        //⦁	Username – text with length[3, 20] (required)
        [Required]
        [MaxLength(20)]
        public string Username { get; set; }
        //⦁	FullName – text, which has two words, consisting of Latin letters.Both start with an upper letter and are followed /bylower   ./      letters.The two words are separated by a single space (ex. "John Smith") (required)
        [Required]
        
        public string FullName { get; set; }

        //⦁	Email – text(required)
        [Required]
        //[EmailAddress]
        public string Email { get; set; }

        //⦁	Age – integer in the range[3, 103] (required)
        [Range(3, 103)]
        public int Age { get; set; }

        //⦁	Cards – collection of type Card
        public ICollection<Card> Cards { get; set; }
    }
}
