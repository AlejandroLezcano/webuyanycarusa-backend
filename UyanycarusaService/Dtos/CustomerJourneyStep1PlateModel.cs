using System.ComponentModel.DataAnnotations;

namespace UyanycarusaService.Dtos
{
    /// <summary>
    /// Modelo para iniciar un customer journey usando License Plate
    /// </summary>
    public class CustomerJourneyStep1PlateModel
    {
        [Required]
        public long VisitId { get; set; }

        [Required]
        [MinLength(1)]
        public string PlateNumber { get; set; } = string.Empty;

        [Required]
        [MinLength(1)]
        [RegularExpression(@"^[A-Z]{2}$", ErrorMessage = "Plate state must be 2 uppercase letters")]
        public string PlateState { get; set; } = string.Empty;
    }
}

