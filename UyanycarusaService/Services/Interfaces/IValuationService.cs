using System.Text.Json;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Interfaz para el servicio de valuaciones
    /// </summary>
    public interface IValuationService
    {
        /// <summary>
        /// Realiza una valuación básica del vehículo
        /// </summary>
        /// <param name="model">Payload enviado al endpoint /Valuation</param>
        /// <returns>Respuesta de valuación como JSON</returns>
        Task<JsonElement> CreateValuationAsync(JsonElement model);

        /// <summary>
        /// Realiza una valuación del vehículo incluyendo daños
        /// </summary>
        /// <param name="model">Payload enviado al endpoint /Valuation/with-damage</param>
        /// <returns>Respuesta de valuación como JSON</returns>
        Task<JsonElement> CreateValuationWithDamageAsync(JsonElement model);
    }
}


