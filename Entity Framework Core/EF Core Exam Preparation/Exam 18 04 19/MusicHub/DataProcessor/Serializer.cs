namespace MusicHub.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using MusicHub.DataProcessor.ExportDtos;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context.Albums.Where(x => x.ProducerId == producerId).OrderByDescending(x => x.Price).ToList()
                .Select(x => new AlbumJsonDto
                {
                    AlbumName = x.Name,
                    ReleaseDate = x.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    ProducerName = x.Producer.Name,
                    AlbumPrice = x.Price.ToString("f2"),
                    Songs = x.Songs.Select(s => new SongJsonDto
                    {
                        SongName = s.Name,
                        Price = s.Price.ToString("f2"),
                        Writer = s.Writer.Name
                    }).OrderByDescending(p => p.SongName)
                    .ThenBy(p => p.Writer)
                    .ToList()
                }).ToList();
            return JsonConvert.SerializeObject(albums, Formatting.Indented);
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs.Where(x => x.Duration.TotalSeconds > duration).ToList()
                .Select(x => new SongXmlDto
                {
                    SongName = x.Name,
                    Writer = x.Writer.Name,
                    Performer = string.Join(", ", x.SongPerformers.Select(s => s.Performer.FirstName + " " + s.Performer.LastName)),
                    AlbumProducer = x.Album.Producer.Name,
                    Duration = x.Duration.ToString("c", CultureInfo.InvariantCulture),
                })
                .OrderBy(x => x.SongName)
                .ThenBy(x => x.Writer)
                .ThenBy(x => x.Performer)
                .ToList();
            var serializer = new XmlSerializer(typeof(List<SongXmlDto>), new XmlRootAttribute("Songs"));
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            StringBuilder output = new StringBuilder();
            using(var writer = new StringWriter(output))
            {
                serializer.Serialize(writer, songs, namespaces);
            }
            return output.ToString().TrimEnd();
        }
    }
}