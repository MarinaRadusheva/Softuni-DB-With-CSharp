namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using BookShop.Data.Models;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof(BookXmlDto[]), new XmlRootAttribute("Books"));
            StringReader reader = new StringReader(xmlString);
            using (reader)
            {
                var books = (BookXmlDto[])serializer.Deserialize(reader);
                foreach (var bookDto in books)
                {
                    
                    if (!IsValid(bookDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    var dateIsValid = DateTime.TryParseExact(bookDto.PublishedOn, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date);
                    if (!dateIsValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    var book = new Book
                    {
                        Name = bookDto.Name,
                        Genre = (Genre)Enum.Parse(typeof(Genre), bookDto.Genre),
                        Price = bookDto.Price,
                        Pages = bookDto.Pages,
                        PublishedOn = date,
                    };
                    context.Books.Add(book);
                    context.SaveChanges();
                    sb.AppendLine(string.Format(SuccessfullyImportedBook, book.Name, book.Price));
                }
                return sb.ToString().TrimEnd();
            }
         
        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            var authors = JsonConvert.DeserializeObject<IEnumerable<AuthorJsonDto>>(jsonString);
            StringBuilder sb = new StringBuilder();
            foreach (var auth in authors)
            {
                if (!IsValid(auth))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (context.Authors.Any(x=>x.Email==auth.Email))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var author = new Author
                {
                    FirstName=auth.FirstName,
                    LastName=auth.LastName,
                    Email=auth.Email,
                    Phone=auth.Phone,
                };
                foreach (var authBook in auth.Books)
                {
                    if (authBook.Id!=null && context.Books.Any(x=>x.Id==authBook.Id))
                    {
                        author.AuthorsBooks.Add(new AuthorBook { BookId = authBook.Id.Value });
                    }
                }
                if (author.AuthorsBooks.Count()==0)
                {

                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                context.Authors.Add(author);
                context.SaveChanges();
                sb.AppendLine(string.Format(SuccessfullyImportedAuthor, author.FirstName + " " + author.LastName, author.AuthorsBooks.Count()));
            }
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}