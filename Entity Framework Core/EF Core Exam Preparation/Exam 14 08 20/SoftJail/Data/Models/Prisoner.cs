using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.Data.Models
{
    public class Prisoner
    {
        public Prisoner()
        {
            this.Mails = new HashSet<Mail>();
            this.PrisonerOfficers = new HashSet<OfficerPrisoner>();
        }
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string FullName { get; set; } //– text with min length 3 and max length 20 (required)
        [Required]
        public string Nickname { get; set; } //– text starting with "The " and a single word only of letters with an uppercase letter for beginning(example: The Prison )
        public int Age { get; set; } //– integer in the range[18, 65] (required)
        public DateTime IncarcerationDate { get; set; }  //Date(required)
        public DateTime? ReleaseDate { get; set; } //– Date
        public decimal? Bail { get; set; } // (non-negative, minimum value: 0)
        public int? CellId { get; set; } //- integer, foreign key
        public virtual Cell Cell { get; set; } // – the prisoner's cell
        public virtual ICollection<Mail> Mails { get; set; } //- collection of type Mail
        public virtual ICollection<OfficerPrisoner> PrisonerOfficers { get; set; } //- collection of type OfficerPrisoner
    }
}