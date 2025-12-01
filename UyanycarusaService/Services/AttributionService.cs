using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Servicio para operaciones de atribución
    /// </summary>
    public class AttributionService : IAttributionService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AttributionService> _logger;
        private readonly ITokenService _tokenService;

        public AttributionService(
            IHttpClientFactory httpClientFactory,
            ILogger<AttributionService> logger,
            IConfiguration configuration,
            ITokenService tokenService)
        {
            _httpClient = httpClientFactory.CreateClient("WebuyAnyCarApi");
            _logger = logger;
            _tokenService = tokenService;
        }

        /// <inheritdoc />
        public async Task<JsonElement> CreateOrGetVisitorAsync(long? oldVisitorId = null)
        {
            try
            {

                var url = "/Attribution/visitor";
                if (oldVisitorId.HasValue)
                {
                    url += $"?oldVisitorId={oldVisitorId.Value}";
                }

                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    return json;
                }

                _logger.LogWarning("El servicio externo /Attribution/visitor retornó un código de estado: {StatusCode}", response.StatusCode);

                throw new HttpRequestException(
                    $"Error al crear u obtener visitor. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al comunicarse con el servicio externo /Attribution/visitor");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear u obtener visitor");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<JsonElement> CreateVisitAsync(long visitorId, JsonElement model)
        {
            try
            {

                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Post, $"/Attribution/visitor/{visitorId}/visit")
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

                _logger.LogWarning("El servicio externo /Attribution/visitor/visit retornó un código de estado: {StatusCode}", response.StatusCode);

                throw new HttpRequestException(
                    $"Error al registrar visita. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al comunicarse con el servicio externo /Attribution/visitor/visit");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al registrar visita");
                throw;
            }
        }
    }
}

