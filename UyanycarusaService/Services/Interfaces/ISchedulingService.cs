using System.Text.Json;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Interfaz para el servicio de Scheduling (OTP)
    /// </summary>
    public interface ISchedulingService
    {
        /// <summary>
        /// Solicita un código OTP para programación
        /// </summary>
        /// <param name="model">Datos de la solicitud OTP</param>
        /// <returns>Respuesta de solicitud OTP como JSON</returns>
        Task<JsonElement> RequestOTPAsync(JsonElement model);
    }
}

