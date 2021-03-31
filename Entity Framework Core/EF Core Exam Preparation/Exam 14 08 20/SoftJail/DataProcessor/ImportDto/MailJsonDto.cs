using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.DataProcessor.ImportDto
{
    public class MailJsonDto
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public string Sender { get; set; }
        [Required]
        [RegularExpression(@"^[A-Z,a-z, ,0-9]+ str.$")]
        public string Address { get; set; } //example:62 Muir Hill str.“)
    }
}
