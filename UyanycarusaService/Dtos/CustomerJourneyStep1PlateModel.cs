using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UyanycarusaService.Dtos
{
    /// <summary>
    /// Model to start a customer journey using License Plate
    /// </summary>
    public class CustomerJourneyStep1PlateModel
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? VisitId { get; set; }

        [Required]
        [MinLength(1)]
        [JsonPropertyName("plateNumber")]
        public string PlateNumber { get; set; } = string.Empty;

        [Required]
        [MinLength(2)]
        [MaxLength(2)]
        [RegularExpression(@"^[A-Z]{2}$", ErrorMessage = "Plate state must be 2 uppercase letters")]
        [JsonPropertyName("plateState")]
        public string PlateState { get; set; } = string.Empty;
    }
}

