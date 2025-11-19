using System.Text.Json;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Interfaz para el servicio de citas (appointments)
    /// </summary>
    public interface IAppointmentService
    {
        /// <summary>
        /// Obtiene la disponibilidad de citas para un código postal y vehículo específico
        /// </summary>
        /// <param name="zipCode">Código postal (5 dígitos)</param>
        /// <param name="customerVehicleId">ID del vehículo del cliente</param>
        /// <returns>Respuesta de disponibilidad como JSON</returns>
        Task<JsonElement> GetAvailabilityAsync(string zipCode, int customerVehicleId);

        /// <summary>
        /// Reserva una cita para un vehículo
        /// </summary>
        /// <param name="model">Datos de la reserva de cita</param>
        /// <returns>Respuesta de reserva como JSON</returns>
        Task<JsonElement> BookAppointmentAsync(JsonElement model);

        /// <summary>
        /// Reprograma una cita existente
        /// </summary>
        /// <param name="existingAppointmentId">ID de la cita existente</param>
        /// <param name="model">Datos de la nueva reserva de cita</param>
        /// <returns>Respuesta de reprogramación como JSON</returns>
        Task<JsonElement> RescheduleAppointmentAsync(int existingAppointmentId, JsonElement model);

        /// <summary>
        /// Cancela una cita existente
        /// </summary>
        /// <param name="customerVehicleId">ID del vehículo del cliente</param>
        /// <param name="phoneNumber">Número de teléfono del cliente</param>
        /// <returns>Respuesta de cancelación como JSON</returns>
        Task<JsonElement> CancelAppointmentAsync(int customerVehicleId, long phoneNumber);
    }
}

