using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cinema.DataProcessor.ImportDto
{
    class MovieJsonDto
    {

        [Required]
        [MaxLength(20)]
        [MinLength(3)]
        public string Title { get; set; }
        [Required]
        public string Genre { get; set; }
        [Required]
        public TimeSpan? Duration { get; set; }
        [Required]
        [Range(1, 10)]
        public double Rating { get; set; }
        [Required]
        [MaxLength(20)]
        [MinLength(3)]
        public string Director { get; set; }


    }
}
