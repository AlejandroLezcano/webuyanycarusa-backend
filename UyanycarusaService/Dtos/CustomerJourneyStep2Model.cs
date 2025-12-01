using System.ComponentModel.DataAnnotations;

namespace UyanycarusaService.Dtos
{
    /// <summary>
    /// Modelo para actualizar los detalles del vehículo en el journey (Paso 2)
    /// </summary>
    public class CustomerJourneyStep2Model
    {
        [Required]
        [MinLength(1)]
        public string Series { get; set; } = string.Empty;

        [Required]
        [MinLength(1)]
        public string BodyStyle { get; set; } = string.Empty;
    }
}

