using System.ComponentModel.DataAnnotations;

namespace UyanycarusaService.Dtos
{
    /// <summary>
    /// Modelo para reservar una cita
    /// </summary>
    public class AppointmentBookingModel
    {
        [Required]
        public int CustomerVehicleId { get; set; }

        [Required]
        public int BranchId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public int TimeSlotId { get; set; }

        [Required]
        [MinLength(1)]
        [RegularExpression(@"^\D*[2-9](\D*\d\D*){2}\D*[2-9](\D*\d\D*){6}$", ErrorMessage = "Invalid phone number format")]
        public string CustomerPhoneNumber { get; set; } = string.Empty;

        [Required]
        [MinLength(1)]
        public string CustomerFirstName { get; set; } = string.Empty;

        [Required]
        [MinLength(1)]
        public string CustomerLastName { get; set; } = string.Empty;

        [MinLength(1)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        public string? City { get; set; }

        public long? VisitId { get; set; }

        public bool? smsOptIn { get; set; }

        public string? OtpCode { get; set; }
    }
}

