using System.ComponentModel.DataAnnotations;

namespace UyanycarusaService.Dtos
{
    /// <summary>
    /// Modelo para solicitar un código OTP
    /// </summary>
    public class ScheduleOTPRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int CustomerVehicleId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int BranchId { get; set; }

        [Required]
        [MinLength(1)]
        public string TargetPhoneNumber { get; set; } = string.Empty;
    }
}

