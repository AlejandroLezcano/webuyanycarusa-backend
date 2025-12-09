using System.ComponentModel.DataAnnotations;

namespace UyanycarusaService.Dtos
{
    /// <summary>
    /// Model to start a customer journey using Year, Make, Model
    /// </summary>
    public class CustomerJourneyStep1YMMModel
    {
        public long? VisitId { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(4)]
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

