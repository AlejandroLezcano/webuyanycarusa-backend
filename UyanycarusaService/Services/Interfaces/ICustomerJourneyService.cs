using System.Text.Json;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Interfaz para el servicio de customer journey
    /// </summary>
    public interface ICustomerJourneyService
    {
        /// <summary>
        /// Inicia un customer journey usando Year, Make, Model
        /// </summary>
        /// <param name="model">Datos del vehículo (YMM)</param>
        /// <returns>Respuesta del journey como JSON</returns>
        Task<JsonElement> CreateJourneyWithYMMAsync(JsonElement model);

        /// <summary>
        /// Inicia un customer journey usando VIN
        /// </summary>
        /// <param name="model">Datos del vehículo (VIN)</param>
        /// <returns>Respuesta del journey como JSON</returns>
        Task<JsonElement> CreateJourneyWithVINAsync(JsonElement model);

        /// <summary>
        /// Inicia un customer journey usando License Plate
        /// </summary>
        /// <param name="model">Datos del vehículo (Plate)</param>
        /// <returns>Respuesta del journey como JSON</returns>
        Task<JsonElement> CreateJourneyWithPlateAsync(JsonElement model);

        /// <summary>
        /// Obtiene un customer journey por su ID (UUID)
        /// </summary>
        /// <param name="id">ID del journey (UUID)</param>
        /// <returns>Respuesta del journey como JSON</returns>
        Task<JsonElement> GetJourneyByIdAsync(string id);

        /// <summary>
        /// Obtiene un customer journey por visitId
        /// </summary>
        /// <param name="visitId">ID de la visita</param>
        /// <returns>Respuesta del journey como JSON</returns>
        Task<JsonElement> GetJourneyByVisitIdAsync(int visitId);

        /// <summary>
        /// Actualiza los detalles del vehículo en el journey (Paso 2)
        /// </summary>
        /// <param name="id">ID del journey (UUID)</param>
        /// <param name="model">Detalles del vehículo</param>
        /// <returns>Respuesta del journey como JSON</returns>
        Task<JsonElement> UpdateVehicleDetailsAsync(string id, JsonElement model);

        /// <summary>
        /// Obtiene las opciones de daño disponibles para un journey
        /// </summary>
        /// <param name="customerJourneyId">ID del journey (UUID)</param>
        /// <returns>Respuesta con opciones de daño como JSON</returns>
        Task<JsonElement> GetDamageOptionsAsync(string customerJourneyId);

        /// <summary>
        /// Actualiza la condición del vehículo en el journey (Paso 3)
        /// </summary>
        /// <param name="id">ID del journey (UUID)</param>
        /// <param name="model">Condición del vehículo</param>
        /// <returns>Respuesta del journey como JSON</returns>
        Task<JsonElement> UpdateVehicleConditionAsync(string id, JsonElement model);

        /// <summary>
        /// Actualiza el trabajo de carrocería en el journey (Paso 4)
        /// </summary>
        /// <param name="id">ID del journey (UUID)</param>
        /// <param name="model">Trabajo de carrocería</param>
        /// <returns>Respuesta del journey como JSON</returns>
        Task<JsonElement> UpdateBodyWorkAsync(string id, JsonElement model);
    }
}

