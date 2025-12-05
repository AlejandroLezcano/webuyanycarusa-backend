using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using UyanycarusaService.Dtos;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Servicio para operaciones relacionadas con SMS
    /// </summary>
    public class SmsService : ISmsService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SmsService> _logger;
        private readonly ITokenService _tokenService;

        public SmsService(
            IHttpClientFactory httpClientFactory,
            ILogger<SmsService> logger,
            IConfiguration configuration,
            ITokenService tokenService)
        {
            _httpClient = httpClientFactory.CreateClient("WebuyAnyCarApi");
            _logger = logger;
            _tokenService = tokenService;
        }

        /// <inheritdoc />
        public async Task<JsonElement> SendSmsAsync(SmsSendModel request)
        {
            try
            {
                var accessToken = await _tokenService.GetAccessTokenAsync();
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/Sms/send");
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                requestMessage.Content = JsonContent.Create(request);

                var response = await _httpClient.SendAsync(requestMessage);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    return json;
                }

                _logger.LogWarning("El servicio externo /Sms/send retornó un código de estado: {StatusCode}", response.StatusCode);

                throw new HttpRequestException(
                    $"Error al enviar SMS en el servicio externo. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al enviar SMS");
                throw new HttpRequestException("Error inesperado al comunicarse con el servicio externo para enviar SMS", ex);
            }
        }
    }
}

