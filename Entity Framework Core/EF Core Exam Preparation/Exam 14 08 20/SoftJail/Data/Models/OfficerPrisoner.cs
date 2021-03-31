namespace SoftJail.Data.Models
{
    public class OfficerPrisoner
    {
        public int PrisonerId { get; set; } //– integer, Primary Key
        public virtual Prisoner Prisoner { get; set; } //– the officer’s prisoner(required)
        public int OfficerId { get; set; } //– integer, Primary Key
        public virtual Officer Officer { get; set; } //– the prisoner’s officer(required)

    }
}