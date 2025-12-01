using System.Net.Http.Headers;
using System.Text.Json;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Servicio para operaciones de contenido de marca y modelo
    /// </summary>
    public class MakeModelContentService : IMakeModelContentService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MakeModelContentService> _logger;
        private readonly ITokenService _tokenService;

        public MakeModelContentService(
            IHttpClientFactory httpClientFactory,
            ILogger<MakeModelContentService> logger,
            IConfiguration configuration,
            ITokenService tokenService)
        {
            _httpClient = httpClientFactory.CreateClient("WebuyAnyCarApi");
            _logger = logger;
            _tokenService = tokenService;
        }

        /// <inheritdoc />
        public async Task<JsonElement> GetMakeContentAsync(string make)
        {
            try
            {

                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Get, $"/content/make-model/{Uri.EscapeDataString(make)}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    return json;
                }

                _logger.LogWarning("El servicio externo /content/make-model/{Make} retornó un código de estado: {StatusCode}", make, response.StatusCode);

                throw new HttpRequestException(
                    $"Error al obtener contenido de marca. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al comunicarse con el servicio externo /content/make-model");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener contenido de marca");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<JsonElement> GetMakeModelContentAsync(string make, string model)
        {
            try
            {

                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Get, $"/content/make-model/{Uri.EscapeDataString(make)}/{Uri.EscapeDataString(model)}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    return json;
                }

                _logger.LogWarning("El servicio externo /content/make-model/{Make}/{Model} retornó un código de estado: {StatusCode}", make, model, response.StatusCode);

                throw new HttpRequestException(
                    $"Error al obtener contenido de marca y modelo. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al comunicarse con el servicio externo /content/make-model");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener contenido de marca y modelo");
                throw;
            }
        }
    }
}

