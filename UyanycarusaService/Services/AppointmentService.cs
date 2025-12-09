using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Service for appointment operations
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

                _logger.LogWarning("External service /Appointment/availability returned status code: {StatusCode}", response.StatusCode);

                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Error getting appointment availability. StatusCode: {response.StatusCode}, Detail: {errorContent}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error communicating with external service /Appointment/availability");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error getting appointment availability");
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

                _logger.LogWarning("External service /Appointment/book returned status code: {StatusCode}", response.StatusCode);

                throw new HttpRequestException(
                    $"Error booking appointment. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error communicating with external service /Appointment/book");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error booking appointment");
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

                _logger.LogWarning("External service /Appointment/reschedule returned status code: {StatusCode}", response.StatusCode);

                throw new HttpRequestException(
                    $"Error rescheduling appointment. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error communicating with external service /Appointment/reschedule");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error rescheduling appointment");
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

                _logger.LogWarning("External service /Appointment/cancel returned status code: {StatusCode}", response.StatusCode);

                throw new HttpRequestException(
                    $"Error canceling appointment. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error communicating with external service /Appointment/cancel");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error canceling appointment");
                throw;
            }
        }
    }
}

