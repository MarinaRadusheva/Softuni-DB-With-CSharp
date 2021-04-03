using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MusicHub.DataProcessor.ImportDtos
{
    public class ProducerJsonDto
    {
        [Required]
        [MaxLength(30)]
        [MinLength(3)]
        public string Name { get; set; }
        [RegularExpression(@"^[A-Z][a-z]+ [A-Z][a-z]+$")]
        public string Pseudonym { get; set; }
        [RegularExpression(@"^\+359( \d{3}){3}$")]
        public string PhoneNumber { get; set; }
        public ICollection<AlbumJsonDto> Albums { get; set; }

    }
}
