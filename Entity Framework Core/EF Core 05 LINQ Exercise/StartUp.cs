namespace MusicHub
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            Console.WriteLine(ExportSongsAboveDuration(context, 4));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context.Albums.Where(x => x.ProducerId == producerId).
                Select(x => new
                {
                    Name = x.Name,
                    ReleaseDate = x.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    ProducerName = x.Producer.Name,
                    ALbumPrice = x.Price,
                    Songs = x.Songs.Select(s => new
                    {
                        SongName = s.Name,
                        SongPrice = s.Price,
                        SongWriter = s.Writer.Name
                    }).OrderByDescending(s => s.SongName).
                    ThenBy(s => s.SongWriter).
                    ToList()
                }).
                OrderByDescending(x=>x.ALbumPrice).
                ToList();
            StringBuilder sb = new StringBuilder();
            foreach (var album in albums)
            {
                sb.AppendLine($"-AlbumName: {album.Name}");
                sb.AppendLine($"-ReleaseDate: {album.ReleaseDate}");
                sb.AppendLine($"-ProducerName: {album.ProducerName}");
                sb.AppendLine("-Songs:");
                int songNum = 1;
                foreach (var song in album.Songs)
                {
                    sb.AppendLine($"---#{songNum}");
                    sb.AppendLine($"---SongName: {song.SongName}");
                    sb.AppendLine($"---Price: {song.SongPrice:F2}");
                    sb.AppendLine($"---Writer: {song.SongWriter}");
                    songNum++;
                }
                sb.AppendLine($"-AlbumPrice: {album.ALbumPrice:F2}");
            }
            return sb.ToString().TrimEnd();
            
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs.Select(x => new 
            { 
                SongName = x.Name, 
                SongWriter = x.Writer.Name,
                Performer = x.SongPerformers.Select(x=>x.Performer.FullName), 
                AlbumProducer = x.Album.Producer.Name,
                SongDuration = x.Duration,
                DurationInSeconds = x.Duration.TotalSeconds }).OrderBy(x=>x.SongName).ThenBy(x=>x.SongWriter).ToArray();
            StringBuilder sb = new StringBuilder();
            int count = 1;
            foreach (var song in songs.Where(x=>x.DurationInSeconds>duration))
            {
                
                sb.AppendLine($"-Song #{count++}");
                sb.AppendLine($"---SongName: {song.SongName}");
                sb.AppendLine($"---Writer: {song.SongWriter}");
                sb.AppendLine($"---Performer: {song.Performer.FirstOrDefault()}");
                sb.AppendLine($"---AlbumProducer: {song.AlbumProducer}");
                sb.AppendLine($"---Duration: {song.SongDuration:c}");
            }
            return sb.ToString().TrimEnd();
        }
    }
}
