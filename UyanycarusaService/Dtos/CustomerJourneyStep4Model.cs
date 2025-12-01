using System.ComponentModel.DataAnnotations;

namespace UyanycarusaService.Dtos
{
    /// <summary>
    /// Modelo para actualizar el trabajo de carrocería en el journey (Paso 4)
    /// </summary>
    public class CustomerJourneyStep4Model
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

        public List<ComponentFaultViewModel>? ComponentDamageFaults { get; set; }

        [Required]
        public bool UsedForOtherPurpose { get; set; }

        [Required]
        public bool HasOdometerBeenChanged { get; set; }

        [Required]
        public bool HasFloodTheftOrSalvageHistory { get; set; }

        public bool WasAccidentHistoryMarkedYesInStep3 { get; set; }

        public bool CaptchaWasDisplayed { get; set; }
    }

    /// <summary>
    /// Modelo para representar un componente con falla
    /// </summary>
    public class ComponentFaultViewModel
    {
        public int ZoneID { get; set; }

        public int ComponentID { get; set; }

        public int FaultID { get; set; }
    }
}

