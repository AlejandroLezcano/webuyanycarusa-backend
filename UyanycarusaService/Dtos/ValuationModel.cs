using System.ComponentModel.DataAnnotations;

namespace UyanycarusaService.Dtos
{
    /// <summary>
    /// Modelo para realizar una valuación básica del vehículo
    /// </summary>
    public class ValuationModel
    {
        public int? Cvid { get; set; }

        [Required]
        [Range(1, 9999999)]
        public int Mileage { get; set; }

        [Required]
        [MinLength(1)]
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

        [Required]
        public Guid CustomerJourneyId { get; set; }

        public bool CustomerHasOptedIntoSmsMessages { get; set; }

        public bool Ce { get; set; }

        public string? CaptchaMode { get; set; }
    }
}

