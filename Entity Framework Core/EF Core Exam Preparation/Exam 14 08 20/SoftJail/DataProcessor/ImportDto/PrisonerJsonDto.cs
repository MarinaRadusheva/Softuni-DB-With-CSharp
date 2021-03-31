using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.DataProcessor.ImportDto
{
    public class PrisonerJsonDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string FullName { get; set; } //text with min length 3 and max length 20 (required)
        [Required]
        [RegularExpression(@"^The [A-Z][a-z]+$")]
        public string Nickname { get; set; } //text starting with "The " and a single word only of letters with an uppercase letter for beginning(example: The Prison )
        [Range(18,65)]
        public int Age { get; set; } //integer in the range[18, 65] (required)
        [Required]
        public string IncarcerationDate { get; set; }
        public string ReleaseDate { get; set; }
        [Range(0,double.MaxValue)]
        public decimal? Bail { get; set; } //(non-negative, minimum value: 0)
        public int? CellId { get; set; }
        public IEnumerable<MailJsonDto> Mails { get; set; }



    }
}
