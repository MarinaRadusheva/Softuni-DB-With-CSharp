using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookShop.Data.Models
{
    public class Author
    {
        public Author()
        {
			this.AuthorsBooks = new HashSet<AuthorBook>();
        }
		public int Id { get; set; } //- integer, Primary Key
		[Required]
		[MaxLength(30)]
		public string FirstName { get; set; } //- text with length[3, 30]. (required)
		[Required]
		[MaxLength(30)]
		public string LastName { get; set; } //- text with length[3, 30]. (required)
		[Required]
		[EmailAddress]
		public string Email { get; set; } //- text(required). Validate it! 
		[Required]
		public string Phone { get; set; } 
		public virtual ICollection<AuthorBook> AuthorsBooks { get; set; } //- collection of type AuthorBook

    }
}
