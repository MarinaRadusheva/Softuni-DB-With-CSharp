namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Cinema.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportTopMovies(CinemaContext context, int rating)
        {
            var movies = context.Movies
                .Where(x => x.Rating >= rating && x.Projections.Any(p => p.Tickets.Count()!=0))
                .Select(x => new MovieJsonDto
                {
                    MovieName = x.Title,
                    RatingValue=x.Rating,
                    Rating = x.Rating.ToString("f2"),
                    TotalIncomes = x.Projections.Sum(p => p.Tickets.Sum(t => t.Price)).ToString("f2"),
                    Customers = x.Projections.SelectMany(p => p.Tickets.Select(c => c.Customer).Select(s => new CustomerJsonDto
                    {
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Balance = s.Balance.ToString("f2"),
                        BalanceDec = s.Balance,
                    }))
                    .OrderByDescending(d => d.BalanceDec)
                    .ThenBy(d => d.FirstName)
                    .ThenBy(d => d.LastName)
                    .ToList()
                }).OrderByDescending(x => x.RatingValue)
                .ThenByDescending(x => x.TotalIncomes)
                .Take(10)
                .ToList();
            return JsonConvert.SerializeObject(movies, Formatting.Indented);
        }

        public static string ExportTopCustomers(CinemaContext context, int age)
        {
            var customers = context.Customers
                .Where(x => x.Age >= age)
                
                .OrderByDescending(x => x.Tickets.Sum(t => t.Price))
                .Take(10)
                .ToList()
                .Select(x => new CustomerXmlDto
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SpentMoney = x.Tickets.Sum(t => t.Price).ToString("f2"),
                    SpentTime=TimeSpan.FromTicks(x.Tickets.Sum(t=>t.Projection.Movie.Duration.Ticks)).ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture)
                }).ToList();
            var serializer = new XmlSerializer(typeof(List<CustomerXmlDto>), new XmlRootAttribute("Customers"));
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            StringBuilder sb = new StringBuilder();
            var writer = new StringWriter(sb);
            using(writer)
            {
                serializer.Serialize(writer, customers, namespaces);
            }
            return sb.ToString().TrimEnd();           
        }
    }
}