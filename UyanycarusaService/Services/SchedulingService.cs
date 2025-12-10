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
                // _logger.LogInformation("Requesting OTP code from external service /scheduling/otp/request");
                _logger.LogWarning("About to consume external service /scheduling/otp/request with the following body: {Body}", model);
                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Post, "/scheduling/otp/request")
                {
                    Content = JsonContent.Create(model)
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                using var response = await _httpClient.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();
                _logger.LogWarning(content, "Received the following content from external service /scheduling/otp/request: {Content}", content);
                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    return json;
                }

                _logger.LogWarning("External service /scheduling/otp/request returned status code: {StatusCode}", response.StatusCode);

                throw new HttpRequestException(
                    $"Error requesting OTP code. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error communicating with external service /scheduling/otp/request");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error requesting OTP code");
                throw;
            }
        }
    }
}

