using System.ComponentModel.DataAnnotations;

namespace UyanycarusaService.Dtos
{
    /// <summary>
    /// Modelo para iniciar un customer journey usando VIN
    /// </summary>
    public class CustomerJourneyStep1VINModel
    {
        [Required]
        public long VisitId { get; set; }

        [Required]
        [MinLength(1)]
        [System.Text.Json.Serialization.JsonPropertyName("vin")]
        public string Vin { get; set; } = string.Empty;
    }
}

