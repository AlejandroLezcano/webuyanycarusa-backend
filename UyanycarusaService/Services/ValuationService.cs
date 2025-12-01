using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Servicio para operaciones de valuación de vehículos
    /// </summary>
    public class ValuationService : IValuationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ValuationService> _logger;
        private readonly ITokenService _tokenService;

        public ValuationService(IHttpClientFactory httpClientFactory, ILogger<ValuationService> logger, IConfiguration configuration, ITokenService tokenService)
        {
            _httpClient = httpClientFactory.CreateClient("WebuyAnyCarApi");
            _logger = logger;
            _tokenService = tokenService;
        }

        /// <inheritdoc />
        public async Task<JsonElement> CreateValuationAsync(JsonElement model)
        {
            try
            {

                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Post, "/Valuation")
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

                _logger.LogWarning("El servicio externo /Valuation retornó un código de estado: {StatusCode}", response.StatusCode);

                throw new HttpRequestException(
                    $"Error al realizar la valuación básica. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al comunicarse con el servicio externo /Valuation");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al realizar la valuación básica");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<JsonElement> CreateValuationWithDamageAsync(JsonElement model)
        {
            try
            {

                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Post, "/Valuation/with-damage")
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

                _logger.LogWarning("El servicio externo /Valuation/with-damage retornó un código de estado: {StatusCode}", response.StatusCode);

                throw new HttpRequestException(
                    $"Error al realizar la valuación con daños. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al comunicarse con el servicio externo /Valuation/with-damage");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al realizar la valuación con daños");
                throw;
            }
        }
    }
}


