using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Servicio para operaciones de citas (appointments)
    /// </summary>
    public class AppointmentService : IAppointmentService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AppointmentService> _logger;
        private readonly ITokenService _tokenService;

        public AppointmentService(
            IHttpClientFactory httpClientFactory,
            ILogger<AppointmentService> logger,
            IConfiguration configuration,
            ITokenService tokenService)
        {
            _httpClient = httpClientFactory.CreateClient("WebuyAnyCarApi");
            _logger = logger;
            _tokenService = tokenService;
        }

        /// <inheritdoc />
        public async Task<JsonElement> GetAvailabilityAsync(string zipCode, int customerVehicleId)
        {
            try
            {
                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Get, $"/Appointment/availability/{zipCode}/{customerVehicleId}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    return json;
                }

                _logger.LogWarning("El servicio externo /Appointment/availability retornó un código de estado: {StatusCode}", response.StatusCode);

                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Error al obtener disponibilidad de citas. StatusCode: {response.StatusCode}, Detail: {errorContent}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al comunicarse con el servicio externo /Appointment/availability");
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
                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Post, "/Appointment/book")
                {
                    Content = JsonContent.Create(model)
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                using var response = await _httpClient.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    return json;
                }

                _logger.LogWarning("El servicio externo /Appointment/book retornó un código de estado: {StatusCode}", response.StatusCode);

                throw new HttpRequestException(
                    $"Error al reservar la cita. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al comunicarse con el servicio externo /Appointment/book");
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
                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Post, $"/Appointment/{existingAppointmentId}/reschedule")
                {
                    Content = JsonContent.Create(model)
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                using var response = await _httpClient.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    return json;
                }

                _logger.LogWarning("El servicio externo /Appointment/reschedule retornó un código de estado: {StatusCode}", response.StatusCode);

                throw new HttpRequestException(
                    $"Error al reprogramar la cita. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al comunicarse con el servicio externo /Appointment/reschedule");
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
                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Post, $"/Appointment/cancel/{customerVehicleId}/{phoneNumber}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    return json;
                }

                _logger.LogWarning("El servicio externo /Appointment/cancel retornó un código de estado: {StatusCode}", response.StatusCode);

                throw new HttpRequestException(
                    $"Error al cancelar la cita. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al comunicarse con el servicio externo /Appointment/cancel");
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

