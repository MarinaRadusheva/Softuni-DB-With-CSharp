namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Cinema.Data.Models;
    using Cinema.Data.Models.Enums;
    using Cinema.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportMovie
            = "Successfully imported {0} with genre {1} and rating {2}!";
        private const string SuccessfulImportHallSeat
            = "Successfully imported {0}({1}) with {2} seats!";
        private const string SuccessfulImportProjection
            = "Successfully imported projection {0} on {1}!";
        private const string SuccessfulImportCustomerTicket
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            var movies = JsonConvert.DeserializeObject<IEnumerable<MovieJsonDto>>(jsonString);
            foreach (var movie in movies)
            {
                if (!IsValid(movie) || context.Movies.Any(x => x.Title == movie.Title))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var parsedGenre = Enum.TryParse(typeof(Genre), movie.Genre, out var genre);
                if (!parsedGenre)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var newMovie = new Movie
                {
                    Title = movie.Title,
                    Genre = (Genre)genre,
                    Director = movie.Director,
                    Rating = movie.Rating,
                    Duration = movie.Duration.Value,
                };
                context.Movies.Add(newMovie);
                context.SaveChanges();
                sb.AppendLine(string.Format(SuccessfulImportMovie, newMovie.Title, newMovie.Genre.ToString(), newMovie.Rating.ToString("f2")));
            }
            return sb.ToString().TrimEnd();
        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            var halls = JsonConvert.DeserializeObject<IEnumerable<HallJsonDto>>(jsonString);
            foreach (var hall in halls)
            {
                if (!IsValid(hall))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var newHall = new Hall
                {
                    Name = hall.Name,
                    Is3D = hall.Is3D,
                    Is4Dx = hall.Is4Dx,
                };
                for (int i = 0; i < hall.Seats; i++)
                {
                    newHall.Seats.Add(new Seat());
                }
                context.Halls.Add(newHall);
                context.SaveChanges();
                var projectionType = "Normal";
                if (newHall.Is3D && newHall.Is4Dx)
                {

                    projectionType = "4Dx/3D";
                }
                if (newHall.Is3D && !newHall.Is4Dx)
                {
                    projectionType = "3D";
                }
                if (newHall.Is4Dx && !newHall.Is3D)
                {
                    projectionType = "4Dx";
                }
                sb.AppendLine(string.Format(SuccessfulImportHallSeat, newHall.Name, projectionType, newHall.Seats.Count()));
            }
            return sb.ToString().TrimEnd();
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            var serializer = new XmlSerializer(typeof(ProjectionXmlDto[]), new XmlRootAttribute("Projections"));
            var reader = new StringReader(xmlString);
            using (reader)
            {
                var projections = (ProjectionXmlDto[])serializer.Deserialize(reader);
                foreach (var projection in projections)
                {
                    if (!context.Movies.Any(x => x.Id == projection.MovieId) || !context.Halls.Any(x => x.Id == projection.HallId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    var newProjection = new Projection
                    {
                        HallId = projection.HallId,
                        MovieId = projection.MovieId,
                        DateTime = DateTime.ParseExact(projection.DateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                    };
                    context.Projections.Add(newProjection);
                    context.SaveChanges();
                    sb.AppendLine(string.Format(SuccessfulImportProjection, newProjection.Movie.Title, newProjection.DateTime.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                }
            }
            return sb.ToString().TrimEnd();
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            var serializer = new XmlSerializer(typeof(CustomerXmlDto[]), new XmlRootAttribute("Customers"));
            var reader = new StringReader(xmlString);
            using (reader)
            {
                var customers = (CustomerXmlDto[])serializer.Deserialize(reader);
                foreach (var customer in customers)
                {
                    if (!IsValid(customer) || !customer.Tickets.All(IsValid))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (!customer.Tickets.Select(x => x.ProjectionId).All(p => context.Projections.Any(c => c.Id == p)))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    var newCustomer = new Customer
                    {
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Age = customer.Age,
                        Balance = customer.Balance,
                    };
                    context.Customers.Add(newCustomer);
                    context.SaveChanges();
                    foreach (var ticket in customer.Tickets)
                    {
                        var newTicket = new Ticket
                        {
                            ProjectionId = ticket.ProjectionId,
                            Price = ticket.Price,
                        };
                        newCustomer.Tickets.Add(newTicket);
                    }
                    context.SaveChanges();
                    sb.AppendLine(string.Format(SuccessfulImportCustomerTicket, newCustomer.FirstName, newCustomer.LastName, newCustomer.Tickets.Count()));
                }
                
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