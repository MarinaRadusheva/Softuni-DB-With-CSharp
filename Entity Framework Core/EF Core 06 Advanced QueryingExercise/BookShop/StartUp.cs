using BookShop.Data;
using BookShop.Models.Enums;
using BookShop.Initializer;
using System;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using BookShop.Models;
using System.Collections.Generic;
using System.Globalization;

namespace BookShop
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            using BookShopContext context = new BookShopContext();
            //DbInitializer.ResetDatabase(context);
            Console.WriteLine(GetMostRecentBooks(context));
        }
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            //string commandFormatted = command[0].ToString().ToUpper() + command.Substring(1).ToLower();
            var ageRestriction = Enum.Parse<AgeRestriction>(command, true);
            var titles = context.Books.Where(x => x.AgeRestriction == ageRestriction).Select(x => x.Title).OrderBy(x=>x).ToList();
            StringBuilder sb = new StringBuilder();
            foreach (var title in titles)
            {
                sb.AppendLine(title);
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books.Where(x => (int)x.EditionType == 2 && x.Copies < 5000).OrderBy(x => x.BookId).Select(x => x.Title).ToList();
            return string.Join(Environment.NewLine, books);
        }
         public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books.
                Where(x => x.Price > 40).
                Select(x => new { x.Title, x.Price }).
                OrderByDescending(x => x.Price).
                ToList();
            StringBuilder sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:F2}");
            }
            return sb.ToString().TrimEnd();
            //var books = context.Books.FromSqlRaw("SELECT Title as Title, Price as Price FROM Books  WHERE Price > 40 ORDER BY Price DESC");
            //StringBuilder sb = new StringBuilder();
            //foreach (var book in books)
            //{
            //    sb.AppendLine($"{book.Title} - ${book.Price}");
            //}
            //return sb.ToString().TrimEnd();
        }
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books.
                Where(b => b.ReleaseDate.HasValue && b.ReleaseDate.Value.Year != year).
                Select(x => x.Title).
                ToList();
            return string.Join(Environment.NewLine, books);
        }
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            List<string> categories = input.
                Split().
                Select(x => x.ToLower()).
                ToList();
            var books = context.Books.
                Where(x => x.BookCategories.Any(x => categories.Contains(x.Category.Name.ToLower()))).
                Select(x => x.Title).
                OrderBy(x=>x).
                ToList();
            return string.Join(Environment.NewLine, books);
        }
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var parsedDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var books = context.Books.
                Where(x => x.ReleaseDate.HasValue && x.ReleaseDate.Value < parsedDate).
                Select(x => new { x.Title, x.EditionType, x.Price, x.ReleaseDate.Value }).
                OrderByDescending(x => x.Value).
                ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:F2}");
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors.
                Where(x => x.FirstName.EndsWith(input.ToLower())).
                Select(x => new { x.FirstName, x.LastName}).
                OrderBy(x=>x.FirstName).ThenBy(x=>x.LastName).
                ToList();
            return string.Join(Environment.NewLine, authors.Select(x=>$"{x.FirstName} {x.LastName}"));
        }
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books.
                Where(x => x.Title.ToLower().Contains(input.ToLower())).
                Select(x => x.Title).OrderBy(x => x).
                ToList();
            return string.Join(Environment.NewLine, books);
        }
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books.
                Where(x => x.Author.LastName.ToLower().StartsWith(input.ToLower())).
                Select(x => new { x.Title, x.Author.FirstName, x.Author.LastName, x.BookId }).
                OrderBy(x => x.BookId).
                ToList();
            StringBuilder sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} ({book.FirstName} {book.LastName})");
            }
            return sb.ToString().TrimEnd();
        }
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context.Books.Where(x => x.Title.Length > lengthCheck).ToArray();
            return books.Length;
        }
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors.Select(x => new { x.FirstName, x.LastName, TotalCopies = x.Books.Sum(x => x.Copies) }).OrderByDescending(x => x.TotalCopies).ToList();
            StringBuilder sb = new StringBuilder();
            foreach (var author in authors)
            {
                sb.AppendLine($"{author.FirstName} {author.LastName} - {author.TotalCopies}");
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categories = context.Categories.
                Select(x => new 
                { 
                    x.Name, 
                    TotalProfit = x.CategoryBooks.Sum(x => (x.Book.Copies * x.Book.Price))
                }).
                OrderByDescending(x=>x.TotalProfit).
                ThenBy(x => x.Name).
                ToList();
            return string.Join(Environment.NewLine, categories.Select(x => $"{x.Name} ${x.TotalProfit:F2}"));
        }
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categories = context.Categories.
                Select(x=>new 
                { 
                    x.Name, 
                    AllBooks =x.CategoryBooks.
                    Select(s => s.Book).
                    OrderByDescending(b=>b.ReleaseDate).
                    Take(3).
                    ToList() 
                }).
                OrderBy(x=>x.Name);
            StringBuilder sb = new StringBuilder();
            foreach (var category in categories)
            {
                sb.AppendLine($"--{category.Name}");
                foreach (var book in category.AllBooks)
                {
                    sb.AppendLine($"{book.Title} ({book.ReleaseDate.Value.Year})");
                }        

            }
            return sb.ToString().TrimEnd();
        }
        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books.Where(x => x.ReleaseDate.Value.Year < 2010).ToList();
            foreach (var book in books)
            {
                book.Price += 5;
            }
            context.SaveChanges();
        }
        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books.Where(x => x.Copies < 4200).ToList();
            foreach (var book in books)
            {
                context.Books.Remove(book);
            }
            context.SaveChanges();
            return books.Count();
        }
    }
}
