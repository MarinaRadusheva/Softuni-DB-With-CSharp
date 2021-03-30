namespace VaporStore.DataProcessor
{
	using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.DataProcessor.Dto.Export;

    public static class Serializer
	{
		public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
		{
			var genres = context.Genres.ToList()
				.Where(x => genreNames.Contains(x.Name))
				.Select(x => new GamesByGenresExportDto
				{
					Id = x.Id,
					Genre = x.Name,
					TotalPlayers = x.Games.SelectMany(x => x.Purchases).Count(),
					Games=x.Games.ToList()
						.Where(g=>g.Purchases.Any())
						.Select(g=>new GameExportDto
						{
							Id=g.Id,
							Title=g.Name,
							Developer=g.Developer.Name,
							Tags=string.Join(", ", g.GameTags.Select(t=>t.Tag.Name)),
							Players=g.Purchases.Count()
						})
						.OrderByDescending(p=>p.Players)
						.ThenBy(p=>p.Id)
						.ToList()
				})
				.OrderByDescending(x=>x.TotalPlayers)
				.ThenBy(x=>x.Id)
				.ToList();
			string output = JsonConvert.SerializeObject(genres, Formatting.Indented);
			return output;
		}

		public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
		{
			var users = context.Users
				.ToList()
				.Where(x => x.Cards.Any(c=>c.Purchases.Any(p=>p.Type.ToString() == storeType)))
				.Select(x => new UserExportDto
				{
					Username = x.Username,
					Purchases = x.Cards.SelectMany(c => c.Purchases).Where(t => t.Type.ToString() == storeType)
							.Select(t => new PurchaseExportDto
							{
								Card = t.Card.Number,
								Cvc = t.Card.Cvc,
								Date = t.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
								Game = new PurchasedGameDto
								{
									Title = t.Game.Name,
									Genre = t.Game.Genre.Name,
									Price = t.Game.Price,
								}
							})
							.OrderBy(x=>x.Date)
							.ToArray(),
					TotalSpent = x.Cards.Sum(c=>c.Purchases.Where(p=>p.Type.ToString()==storeType).Sum(p=>p.Game.Price)),
					//x.Cards.SelectMany(p => p.Purchases.Where(t => t.Type.ToString() == storeType)).Sum(s => s.Game.Price),
				})
				.OrderByDescending(x => x.TotalSpent)
				.ThenBy(x => x.Username)
				.ToArray();
			XmlSerializer serializer = new XmlSerializer(typeof(UserExportDto[]), new XmlRootAttribute("Users"));
			var namespaces = new XmlSerializerNamespaces();
			namespaces.Add("", "");
			StringBuilder sb = new StringBuilder();
			StringWriter writer = new StringWriter(sb);
			serializer.Serialize(writer, users, namespaces);
			return sb.ToString().TrimEnd();
		}
	}
}