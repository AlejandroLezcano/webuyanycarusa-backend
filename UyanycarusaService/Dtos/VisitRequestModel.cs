using System.ComponentModel.DataAnnotations;

namespace UyanycarusaService.Dtos
{
    /// <summary>
    /// Modelo para registrar una visita
    /// </summary>
    public class VisitRequestModel
    {
        [Required]
        public string? IpAddress { get; set; }

        [Required]
        public string? UserAgent { get; set; }

        [Required]
        public string? Browser { get; set; }

        public string? ReferringUrl { get; set; }

        public string? SearchTerm { get; set; }
    }
}

