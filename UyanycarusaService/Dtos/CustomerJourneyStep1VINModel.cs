using System.ComponentModel.DataAnnotations;

namespace UyanycarusaService.Dtos
{
    /// <summary>
    /// Model to start a customer journey using VIN
    /// </summary>
    public class CustomerJourneyStep1VINModel
    {
        public long? VisitId { get; set; }

        [Required]
        [MinLength(17)]
        [MaxLength(17)]
        [System.Text.Json.Serialization.JsonPropertyName("vin")]
        public string Vin { get; set; } = string.Empty;
    }
}

