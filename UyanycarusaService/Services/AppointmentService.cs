using System.Net.Http.Json;
using System.Text.Json;
using UyanycarusaService.ModelsTests;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Servicio para operaciones de citas (appointments)
    /// </summary>
    public class AppointmentService : IAppointmentService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AppointmentService> _logger;
        private readonly bool _useTestData;

        public AppointmentService(
            IHttpClientFactory httpClientFactory,
            ILogger<AppointmentService> logger,
            IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("WebuyAnyCarApi");
            _logger = logger;
            _useTestData = configuration.GetValue<bool>("dataTest");
        }

        /// <inheritdoc />
        public async Task<JsonElement> GetAvailabilityAsync(string zipCode, int customerVehicleId)
        {
            try
            {
                _logger.LogInformation("Solicitando disponibilidad de citas para zipCode {ZipCode} y customerVehicleId {CustomerVehicleId}", zipCode, customerVehicleId);

                var response = await _httpClient.GetAsync($"/Appointment/availability/{zipCode}/{customerVehicleId}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    _logger.LogInformation("Disponibilidad de citas obtenida exitosamente");
                    return json;
                }

                _logger.LogWarning("El servicio externo /Appointment/availability retornó un código de estado: {StatusCode}", response.StatusCode);

                if (_useTestData)
                {
                    _logger.LogInformation("Usando datos de prueba de disponibilidad desde AppointmentTestData (dataTest=true)");
                    return AppointmentTestData.GetAvailability();
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Error al obtener disponibilidad de citas. StatusCode: {response.StatusCode}, Detail: {errorContent}");
            }
            catch (HttpRequestException ex)
            {
                if (_useTestData)
                {
                    _logger.LogWarning(ex, "No se pudo comunicar con el servicio externo /Appointment/availability, usando datos de prueba (dataTest=true)");
                    return AppointmentTestData.GetAvailability();
                }

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener disponibilidad de citas");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<JsonElement> BookAppointmentAsync(JsonElement model)
        {
            try
            {
                _logger.LogInformation("Reservando cita en el servicio externo /Appointment/book");

                using var response = await _httpClient.PostAsJsonAsync("/Appointment/book", model);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    _logger.LogInformation("Cita reservada exitosamente");
                    return json;
                }

                _logger.LogWarning("El servicio externo /Appointment/book retornó un código de estado: {StatusCode}", response.StatusCode);

                if (_useTestData)
                {
                    _logger.LogInformation("Usando datos de prueba de reserva desde AppointmentTestData (dataTest=true)");
                    return AppointmentTestData.GetBookedAppointment();
                }

                throw new HttpRequestException(
                    $"Error al reservar la cita. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                if (_useTestData)
                {
                    _logger.LogWarning(ex, "No se pudo comunicar con el servicio externo /Appointment/book, usando datos de prueba (dataTest=true)");
                    return AppointmentTestData.GetBookedAppointment();
                }

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al reservar la cita");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<JsonElement> RescheduleAppointmentAsync(int existingAppointmentId, JsonElement model)
        {
            try
            {
                _logger.LogInformation("Reprogramando cita {AppointmentId} en el servicio externo /Appointment/reschedule", existingAppointmentId);

                using var response = await _httpClient.PostAsJsonAsync($"/Appointment/{existingAppointmentId}/reschedule", model);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    _logger.LogInformation("Cita reprogramada exitosamente");
                    return json;
                }

                _logger.LogWarning("El servicio externo /Appointment/reschedule retornó un código de estado: {StatusCode}", response.StatusCode);

                if (_useTestData)
                {
                    _logger.LogInformation("Usando datos de prueba de reprogramación desde AppointmentTestData (dataTest=true)");
                    return AppointmentTestData.GetRescheduledAppointment();
                }

                throw new HttpRequestException(
                    $"Error al reprogramar la cita. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                if (_useTestData)
                {
                    _logger.LogWarning(ex, "No se pudo comunicar con el servicio externo /Appointment/reschedule, usando datos de prueba (dataTest=true)");
                    return AppointmentTestData.GetRescheduledAppointment();
                }

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al reprogramar la cita");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<JsonElement> CancelAppointmentAsync(int customerVehicleId, long phoneNumber)
        {
            try
            {
                _logger.LogInformation("Cancelando cita para customerVehicleId {CustomerVehicleId} y phoneNumber {PhoneNumber}", customerVehicleId, phoneNumber);

                var response = await _httpClient.PostAsync($"/Appointment/cancel/{customerVehicleId}/{phoneNumber}", null);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    _logger.LogInformation("Cita cancelada exitosamente");
                    return json;
                }

                _logger.LogWarning("El servicio externo /Appointment/cancel retornó un código de estado: {StatusCode}", response.StatusCode);

                if (_useTestData)
                {
                    _logger.LogInformation("Usando datos de prueba de cancelación desde AppointmentTestData (dataTest=true)");
                    return AppointmentTestData.GetCancelledAppointment();
                }

                throw new HttpRequestException(
                    $"Error al cancelar la cita. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                if (_useTestData)
                {
                    _logger.LogWarning(ex, "No se pudo comunicar con el servicio externo /Appointment/cancel, usando datos de prueba (dataTest=true)");
                    return AppointmentTestData.GetCancelledAppointment();
                }

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al cancelar la cita");
                throw;
            }
        }
    }
}

