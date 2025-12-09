using System.ComponentModel.DataAnnotations;

namespace UyanycarusaService.Dtos
{
    /// <summary>
    /// Model for requesting an OTP code
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

