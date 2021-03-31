using System.ComponentModel.DataAnnotations;

namespace SoftJail.Data.Models
{
    public class Mail
    {
       public int Id { get; set; } //– integer, Primary Key
       [Required]
       public string Description { get; set; } //– text(required)
        [Required]
       public string Sender { get; set; } //– text(required)
        [Required]
       public string Address { get; set; } //– text, consisting only of letters, blic upspaitncesopndnumbers,     which ends with “ str.” (ric equieintd)bluppro (ple:62    Muir Hill str.“)
       public int PrisonerId { get; set; } //- integer, foreign key(required)
       public virtual Prisoner Prisoner { get; set; } //– the mail's Prisoner (required)

    }
}