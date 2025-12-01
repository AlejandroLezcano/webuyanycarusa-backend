using System.ComponentModel.DataAnnotations;

namespace UyanycarusaService.Dtos
{
    /// <summary>
    /// Modelo para iniciar un customer journey usando Year, Make, Model
    /// </summary>
    public class CustomerJourneyStep1YMMModel
    {
        [Required]
        public long VisitId { get; set; }

        [Required]
        [MinLength(1)]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Year must be 4 digits")]
        public string Year { get; set; } = string.Empty;

        [Required]
        [MinLength(1)]
        public string Make { get; set; } = string.Empty;

        [Required]
        [MinLength(1)]
        public string Model { get; set; } = string.Empty;
    }
}

