using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Servicio para operaciones de Scheduling (OTP)
    /// </summary>
    public class SchedulingService : ISchedulingService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SchedulingService> _logger;
        private readonly ITokenService _tokenService;

        public SchedulingService(
            IHttpClientFactory httpClientFactory,
            ILogger<SchedulingService> logger,
            IConfiguration configuration,
            ITokenService tokenService)
        {
            _httpClient = httpClientFactory.CreateClient("WebuyAnyCarApi");
            _logger = logger;
            _tokenService = tokenService;
        }

        /// <inheritdoc />
        public async Task<JsonElement> RequestOTPAsync(JsonElement model)
        {
            try
            {
                // _logger.LogInformation("Solicitando código OTP en el servicio externo /scheduling/otp/request");
                _logger.LogWarning("se va a consumir el servicio externo /scheduling/otp/request con el siguiente body: {Body}", model);
                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Post, "/scheduling/otp/request")
                {
                    Content = JsonContent.Create(model)
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                using var response = await _httpClient.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();
                _logger.LogWarning(content, "se obtuvo el siguiente contenido del servicio externo /scheduling/otp/request: {Content}", content);
                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    return json;
                }

                _logger.LogWarning("El servicio externo /scheduling/otp/request retornó un código de estado: {StatusCode}", response.StatusCode);

                throw new HttpRequestException(
                    $"Error al solicitar código OTP. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al comunicarse con el servicio externo /scheduling/otp/request");
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

