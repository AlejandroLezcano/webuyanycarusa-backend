using System.Net.Http.Json;
using System.Text.Json;
using UyanycarusaService.ModelsTests;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Servicio para operaciones de Scheduling (OTP)
    /// </summary>
    public class SchedulingService : ISchedulingService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SchedulingService> _logger;
        private readonly bool _useTestData;

        public SchedulingService(
            IHttpClientFactory httpClientFactory,
            ILogger<SchedulingService> logger,
            IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("WebuyAnyCarApi");
            _logger = logger;
            _useTestData = configuration.GetValue<bool>("dataTest");
        }

        /// <inheritdoc />
        public async Task<JsonElement> RequestOTPAsync(JsonElement model)
        {
            try
            {
                _logger.LogInformation("Solicitando código OTP en el servicio externo /scheduling/otp/request");

                using var response = await _httpClient.PostAsJsonAsync("/scheduling/otp/request", model);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    _logger.LogInformation("Código OTP solicitado exitosamente");
                    return json;
                }

                _logger.LogWarning("El servicio externo /scheduling/otp/request retornó un código de estado: {StatusCode}", response.StatusCode);

                if (_useTestData)
                {
                    _logger.LogInformation("Usando datos de prueba de OTP desde SchedulingTestData (dataTest=true)");
                    return SchedulingTestData.GetRequestOTP();
                }

                throw new HttpRequestException(
                    $"Error al solicitar código OTP. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                if (_useTestData)
                {
                    _logger.LogWarning(ex, "No se pudo comunicar con el servicio externo /scheduling/otp/request, usando datos de prueba (dataTest=true)");
                    return SchedulingTestData.GetRequestOTP();
                }

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al solicitar código OTP");
                throw;
            }
        }
    }
}

