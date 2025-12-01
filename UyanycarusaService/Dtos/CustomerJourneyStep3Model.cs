using System.ComponentModel.DataAnnotations;

namespace UyanycarusaService.Dtos
{
    /// <summary>
    /// Modelo para actualizar la condición del vehículo en el journey (Paso 3)
    /// </summary>
    public class CustomerJourneyStep3Model
    {
        [Required]
        [Range(1, 9999999)]
        public int Mileage { get; set; }

        [Required]
        [MinLength(1)]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Zip code must be 5 digits")]
        public string ZipCode { get; set; } = string.Empty;

        [Required]
        [MinLength(1)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public bool IsFinancedOrLeased { get; set; }

        [Required]
        public bool CarIsDriveable { get; set; }

        [Required]
        public bool HasDamage { get; set; }

        [Required]
        public bool HasBeenInAccident { get; set; }

        [Phone]
        public string? OptionalPhoneNumber { get; set; }

        public bool CustomerHasOptedIntoSmsMessages { get; set; }

        public bool Ce { get; set; }

        public string? CaptchaMode { get; set; }
    }
}

