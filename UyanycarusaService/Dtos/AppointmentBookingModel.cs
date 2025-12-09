using System.ComponentModel.DataAnnotations;

namespace UyanycarusaService.Dtos
{
    /// <summary>
    /// Model used for booking an appointment with customer and vehicle information.
    /// </summary>
    public class AppointmentBookingModel
    {
        /// <summary>
        /// ID of the customer's vehicle.
        /// </summary>
        [Required]
        public int CustomerVehicleId { get; set; }

        /// <summary>
        /// Branch where the appointment will be scheduled.
        /// </summary>
        [Required]
        public int BranchId { get; set; }

        /// <summary>
        /// Appointment date.
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        /// <summary>
        /// Selected time slot ID.
        /// </summary>
        [Required]
        public int TimeSlotId { get; set; }

        /// <summary>
        /// Customer phone number. Must be valid per NANP rules.
        /// </summary>
        [Required]
        [MinLength(1)]
        [RegularExpression(
            @"^\D*[2-9](\D*\d\D*){2}\D*[2-9](\D*\d\D*){6}$", 
            ErrorMessage = "Invalid phone number format"
        )]
        public string CustomerPhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Customer first name.
        /// </summary>
        [Required]
        [MinLength(1)]
        public string CustomerFirstName { get; set; } = string.Empty;

        /// <summary>
        /// Customer last name.
        /// </summary>
        [Required]
        [MinLength(1)]
        public string CustomerLastName { get; set; } = string.Empty;

        /// <summary>
        /// Customer email (optional).
        /// </summary>
        [MinLength(1)]
        [EmailAddress]
        public string? Email { get; set; }

        /// <summary>
        /// Customer address line 1 (optional).
        /// </summary>
        public string? Address1 { get; set; }

        /// <summary>
        /// Customer address line 2 (optional).
        /// </summary>
        public string? Address2 { get; set; }

        /// <summary>
        /// Customer city (optional).
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// Visit ID associated with the customer journey (optional).
        /// </summary>
        public long? VisitId { get; set; }

        /// <summary>
        /// Indicates if the customer opted in for SMS communications.
        /// </summary>
        public bool? SmsOptIn { get; set; }

        /// <summary>
        /// OTP code used for appointment validation (optional).
        /// </summary>
        public string? OtpCode { get; set; }
    }
}
