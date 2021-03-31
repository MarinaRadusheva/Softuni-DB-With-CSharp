using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class CellJsonDto
    {
        [Required]
        [Range(1,1000)]
        public int CellNumber { get; set; } //integer in the range [1, 1000] (required)
        [Required]
        public bool HasWindow { get; set; }
    }
}