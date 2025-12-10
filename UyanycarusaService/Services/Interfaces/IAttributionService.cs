using System.Text.Json;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Interfaz para el servicio de atribución
    /// </summary>
    public interface IAttributionService
    {
        /// <summary>
        /// Crea o obtiene un visitor
        /// </summary>
        /// <param name="oldVisitorId">ID del visitor anterior opcional (para migración)</param>
        /// <returns>Respuesta con información del visitor como JSON</returns>
        Task<JsonElement> CreateOrGetVisitorAsync(long? oldVisitorId = null);

        /// <summary>
        /// Registra una visita para un visitor
        /// </summary>
        /// <param name="visitorId">ID del visitor</param>
        /// <param name="model">Datos de la visita</param>
        /// <returns>Respuesta con información de la visita como JSON</returns>
        Task<JsonElement> CreateVisitAsync(long visitorId, JsonElement model);
    }
}

