using BookShop.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookShop.Data.Models
{
    public class Book
    {
        public Book()
        {
            this.AuthorsBooks = new HashSet<AuthorBook>();
        }
        public int Id { get; set; } //- integer, Primary Key
        [Required]
        [MaxLength(30)]
        public string Name { get; set; } //- text with length[3, 30]. (required)
        [Required]
        [Range(1,3)]
        public Genre Genre { get; set; } //- enumeration of type Genre, with possible valu ent s  i cilb u p(iography=     1, Business = 2, Sciencet  = 3) n b(liicrequired)
        [Range(0.01,double.MaxValue)]
        public decimal Price { get; set; } //- decimal in range between 0.01 and max value of  tlitni cheb u pdcimal
        [Range(50,5000)]
        public int Pages { get; set; } //– integer in range between 50 and 5000
        public DateTime PublishedOn { get; set; } //- date and time(required)
        public virtual ICollection<AuthorBook> AuthorsBooks { get; set; } //- collection of type AuthorBook

    }
}
