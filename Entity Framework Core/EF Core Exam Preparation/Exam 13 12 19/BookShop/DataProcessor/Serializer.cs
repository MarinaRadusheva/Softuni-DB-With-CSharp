namespace BookShop.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportMostCraziestAuthors(BookShopContext context)
        {
            var authors = context.Authors
                .Select(x => new AuthorJsonDto
                {

                    AuthorName = x.FirstName + " " + x.LastName,
                    Books = x.AuthorsBooks
                    .OrderByDescending(b=>b.Book.Price)
                    .Select(b => new BookJsonDto
                    {
                        BookName = b.Book.Name,
                        BookPrice = b.Book.Price.ToString("f2")
                    })
                    
                    .ToList(),
                })
                .ToList()
                .OrderByDescending(x=>x.Books.Count())
                .ThenBy(x=>x.AuthorName);
            return JsonConvert.SerializeObject(authors, Formatting.Indented);
        }

        public static string ExportOldestBooks(BookShopContext context, DateTime date)
        {
            //top 10 oldest books that are published before the given date and are of type science. 
            var books = context.Books
                .Where(x => x.PublishedOn < date && x.Genre == Genre.Science)
                .ToList()
                .OrderByDescending(x => x.Pages)
                .ThenByDescending(x => x.PublishedOn)
                .Select(x => new OldBookXmlDto
                {
                    Pages = x.Pages,
                    Name = x.Name,
                    Date = x.PublishedOn.ToString("d", CultureInfo.InvariantCulture),
                })               
                .Take(10)
                .ToArray();
            XmlSerializer serializer = new XmlSerializer(typeof(OldBookXmlDto[]), new XmlRootAttribute("Books"));
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            StringBuilder sb = new StringBuilder();
            var writer = new StringWriter(sb);
            using(writer)
            {
                serializer.Serialize(writer, books, namespaces);
            }
            return sb.ToString().TrimEnd();
        }
    }
}