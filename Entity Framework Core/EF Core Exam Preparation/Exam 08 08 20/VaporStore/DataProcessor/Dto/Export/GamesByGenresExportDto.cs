using System;
using System.Collections.Generic;
using System.Text;

namespace VaporStore.DataProcessor.Dto.Export
{

    public class GamesByGenresExportDto
    {
        public int Id { get; set; }
        public string Genre { get; set; }
        public IEnumerable<GameExportDto> Games { get; set; }
        public int TotalPlayers { get; set; }
    }

}
