namespace MusicHub.DataProcessor
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
    using MusicHub.Data.Models;
    using MusicHub.Data.Models.Enums;
    using MusicHub.DataProcessor.ImportDtos;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data";

        private const string SuccessfullyImportedWriter 
            = "Imported {0}";
        private const string SuccessfullyImportedProducerWithPhone 
            = "Imported {0} with phone: {1} produces {2} albums";
        private const string SuccessfullyImportedProducerWithNoPhone
            = "Imported {0} with no phone number produces {1} albums";
        private const string SuccessfullyImportedSong 
            = "Imported {0} ({1} genre) with duration {2}";
        private const string SuccessfullyImportedPerformer
            = "Imported {0} ({1} songs)";

        public static string ImportWriters(MusicHubDbContext context, string jsonString)
        {
            StringBuilder output = new StringBuilder();
            var writers = JsonConvert.DeserializeObject<IEnumerable<WriterJsonDto>>(jsonString);
            List<Writer> writersToAdd = new List<Writer>();
            foreach (var writer in writers)
            {
                if (!IsValid(writer))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                writersToAdd.Add(new Writer { Name = writer.Name, Pseudonym = writer.Pseudonym });
                output.AppendLine(string.Format(SuccessfullyImportedWriter, writer.Name));
            }
            context.Writers.AddRange(writersToAdd);
            context.SaveChanges();
            return output.ToString().TrimEnd();
        }

        public static string ImportProducersAlbums(MusicHubDbContext context, string jsonString)
        {
            StringBuilder output = new StringBuilder();
            var producers = JsonConvert.DeserializeObject<IEnumerable<ProducerJsonDto>>(jsonString);
            foreach (var producer in producers)
            {
                if (!IsValid(producer) || !producer.Albums.All(IsValid))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                var newProducer = new Producer
                {
                    Name = producer.Name,
                    Pseudonym = producer.Pseudonym,
                    PhoneNumber = producer.PhoneNumber
                };
                foreach (var album in producer.Albums)
                {
                    var newAlbum = new Album
                    {
                        Name = album.Name,
                        ReleaseDate = DateTime.ParseExact(album.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    };
                    newProducer.Albums.Add(newAlbum);
                }
                context.Producers.Add(newProducer);
                context.SaveChanges();
                if (newProducer.PhoneNumber==null)
                {
                    output.AppendLine(string.Format(SuccessfullyImportedProducerWithNoPhone, newProducer.Name, newProducer.Albums.Count()));
                }
                else
                {
                    output.AppendLine(string.Format(SuccessfullyImportedProducerWithPhone, newProducer.Name, newProducer.PhoneNumber, newProducer.Albums.Count()));
                }
                
            }
            return output.ToString().TrimEnd();
        }

        public static string ImportSongs(MusicHubDbContext context, string xmlString)
        {
            StringBuilder output = new StringBuilder();
            var serializer = new XmlSerializer(typeof(List<SongXmlDto>), new XmlRootAttribute("Songs"));
            var allSongs = new List<Song>();
            using(StringReader reader = new StringReader(xmlString))
            {
                var songs = (List<SongXmlDto>)serializer.Deserialize(reader);
                foreach (var song in songs)
                {
                    if (!IsValid(song))
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (song.AlbumId.HasValue && !context.Albums.Any(a=>a.Id==song.AlbumId))
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (!context.Writers.Any(w=>w.Id==song.WriterId))
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    var newSong = new Song
                    {
                        Name = song.Name,
                        Duration = TimeSpan.ParseExact(song.Duration, "c", CultureInfo.InvariantCulture),
                        CreatedOn = DateTime.ParseExact(song.CreatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Genre = (Genre)Enum.Parse(typeof(Genre), song.Genre),
                        AlbumId = song.AlbumId,
                        WriterId = song.WriterId,
                        Price = song.Price,
                    };
                    allSongs.Add(newSong);
                    output.AppendLine(string.Format(SuccessfullyImportedSong, newSong.Name, newSong.Genre.ToString(), newSong.Duration.ToString()));
                }
            }
            context.Songs.AddRange(allSongs);
            context.SaveChanges();
            return output.ToString().TrimEnd();
        }

        public static string ImportSongPerformers(MusicHubDbContext context, string xmlString)
        {
            StringBuilder output = new StringBuilder();
            var serializer = new XmlSerializer(typeof(List<PerformerXmlDto>), new XmlRootAttribute("Performers"));
            using (StringReader reader = new StringReader(xmlString))
            {
                var performers = (List<PerformerXmlDto>)serializer.Deserialize(reader);
                foreach (var performer in performers)
                {
                    List<int> songIds = context.Songs.Select(x => x.Id).ToList();
                    if (!IsValid(performer) || !performer.Songs.Select(x=>x.Id).All(songIds.Contains))
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    var newPerformer = new Performer
                    {
                        FirstName = performer.FirstName,
                        LastName = performer.LastName,
                        Age = performer.Age,
                        NetWorth = performer.NetWorth,
                    };
                    context.Performers.Add(newPerformer);
                    context.SaveChanges();
                    foreach (var song in performer.Songs)
                    {
                        newPerformer.PerformerSongs.Add(new SongPerformer { SongId = song.Id });
                    }
                    context.SaveChanges();
                    output.AppendLine(string.Format(SuccessfullyImportedPerformer, newPerformer.FirstName, newPerformer.PerformerSongs.Count()));
                }
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