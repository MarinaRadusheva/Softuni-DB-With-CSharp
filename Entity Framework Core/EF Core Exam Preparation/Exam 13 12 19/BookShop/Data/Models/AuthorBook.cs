namespace BookShop.Data.Models
{
    public class AuthorBook
    {
        public int AuthorId { get; set; } //- integer, Primary Key, Foreign key(lic ret quinired)
        public virtual Author Author { get; set; } // -  Author
        public int BookId { get; set; } // - integer, Primary Key, Foreign key(rec quirein td)
        public virtual Book Book { get; set; } // - Book

    }
}