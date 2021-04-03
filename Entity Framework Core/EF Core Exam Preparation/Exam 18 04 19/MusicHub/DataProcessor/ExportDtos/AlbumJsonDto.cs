using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.DataProcessor.ExportDtos
{
    public class AlbumJsonDto
    {

            public string AlbumName { get; set; }
            public string ReleaseDate { get; set; }
            public string ProducerName { get; set; }
            public ICollection<SongJsonDto> Songs { get; set; }
            public string AlbumPrice { get; set; }

    }
}
