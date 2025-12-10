using System.Text.Json;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Interface for appointment service
    /// </summary>
    public interface IAppointmentService
    {
        /// <summary>
        /// Gets appointment availability for a specific zip code and vehicle
        /// </summary>
        /// <param name="zipCode">Zip code (5 digits)</param>
        /// <param name="customerVehicleId">Customer vehicle ID</param>
        /// <returns>Availability response as JSON</returns>
        Task<JsonElement> GetAvailabilityAsync(string zipCode, int customerVehicleId);

        /// <summary>
        /// Books an appointment for a vehicle
        /// </summary>
        /// <param name="model">Appointment booking data</param>
        /// <returns>Booking response as JSON</returns>
        Task<JsonElement> BookAppointmentAsync(JsonElement model);

        /// <summary>
        /// Reschedules an existing appointment
        /// </summary>
        /// <param name="existingAppointmentId">Existing appointment ID</param>
        /// <param name="model">New appointment booking data</param>
        /// <returns>Reschedule response as JSON</returns>
        Task<JsonElement> RescheduleAppointmentAsync(int existingAppointmentId, JsonElement model);

        /// <summary>
        /// Cancels an existing appointment
        /// </summary>
        /// <param name="customerVehicleId">Customer vehicle ID</param>
        /// <param name="phoneNumber">Customer phone number</param>
        /// <returns>Cancellation response as JSON</returns>
        Task<JsonElement> CancelAppointmentAsync(int customerVehicleId, long phoneNumber);
    }
}

