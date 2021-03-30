namespace VaporStore.DataProcessor
{
	using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.Dto.Import;

    public static class Deserializer
	{
		public static string ImportGames(VaporStoreDbContext context, string jsonString)
		{
			StringBuilder output = new StringBuilder();
			var gamesDtos = JsonConvert.DeserializeObject<IEnumerable<GameImportDto>>(jsonString);
            foreach (var gameDto in gamesDtos)
            {
                if (!IsValid(gameDto)||gameDto.Tags.Count==0)
                {
					output.AppendLine("Invalid Data");
					continue;
                }
				var genre = context.Genres.FirstOrDefault(x => x.Name == gameDto.Genre)?? new Genre { Name = gameDto.Genre };
				var developer = context.Developers.FirstOrDefault(x => x.Name == gameDto.Developer) ?? new Developer { Name = gameDto.Developer };
				var game = new Game
				{
					Name = gameDto.Name,
					Genre = genre,
					Developer = developer,
					Price = gameDto.Price,
					ReleaseDate = gameDto.ReleaseDate.Value,
				};
                foreach (var gameTag in gameDto.Tags)
                {
					var tag = context.Tags.FirstOrDefault(x => x.Name == gameTag) ?? new Tag { Name = gameTag };
					game.GameTags.Add(new GameTag { Tag = tag });
                }
				context.Games.Add(game);
				context.SaveChanges();
				output.AppendLine($"Added {game.Name} ({game.Genre.Name}) with {game.GameTags.Count} tags");
            }
			return output.ToString().TrimEnd();			
		}

		public static string ImportUsers(VaporStoreDbContext context, string jsonString)
		{
			StringBuilder output = new StringBuilder();
			var users = JsonConvert.DeserializeObject<IEnumerable<UserImportDto>>(jsonString);
            foreach (var userDto in users)
            {
				if (!IsValid(userDto) || !userDto.Cards.All(IsValid))
                {
					output.AppendLine("Invalid Data");
					continue;
				}
				var user = new User
				{
					Age = userDto.Age,
					Email = userDto.Email,
					FullName = userDto.FullName,
					Username = userDto.Username,
					Cards = userDto.Cards.Select(x => new Card
					{
						Number = x.Number,
						Cvc = x.CVC,
						Type = x.Type.Value,
					}).ToList()
				};
				context.Users.Add(user);
				context.SaveChanges();
				output.AppendLine($"Imported {user.Username} with { user.Cards.Count} cards");

			}
			return output.ToString().TrimEnd();
		}

		public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
		{
			StringBuilder output = new StringBuilder();
			XmlSerializer serializer = new XmlSerializer(typeof(PurchaseImportDto[]), new XmlRootAttribute("Purchases"));
			var purchasesDto = (PurchaseImportDto[])serializer.Deserialize(new StringReader(xmlString));
            foreach (var purchaseDto in purchasesDto)
            {
                if (!IsValid(purchaseDto))
                {
					output.AppendLine("Invalid Data");
					continue;
                }
				var parsedDate = DateTime.TryParseExact(purchaseDto.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date);
                if (!parsedDate)
                {
					output.AppendLine("Invalid Data");
					continue;
				}
				var purchase = new Purchase
				{
					Date = date,
					Type = purchaseDto.Type.Value,
					Card = context.Cards.FirstOrDefault(x => x.Number == purchaseDto.Card),
					Game = context.Games.FirstOrDefault(x => x.Name == purchaseDto.Title),
					ProductKey = purchaseDto.Key,
				};
				context.Purchases.Add(purchase);
				context.SaveChanges();
				output.AppendLine($"Imported {purchase.Game.Name} for {purchase.Card.User.Username}");
            }
			return output.ToString().TrimEnd();
		}

		private static bool IsValid(object dto)
		{
			var validationContext = new ValidationContext(dto);
			var validationResult = new List<ValidationResult>();

			return Validator.TryValidateObject(dto, validationContext, validationResult, true);
		}
	}
}