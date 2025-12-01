using System.Net.Http.Headers;
using System.Text.Json;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Servicio para operaciones de contenido
    /// </summary>
    public class ContentService : IContentService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ContentService> _logger;
        private readonly ITokenService _tokenService;

        public ContentService(
            IHttpClientFactory httpClientFactory,
            ILogger<ContentService> logger,
            IConfiguration configuration,
            ITokenService tokenService)
        {
            _httpClient = httpClientFactory.CreateClient("WebuyAnyCarApi");
            _logger = logger;
            _tokenService = tokenService;
        }

        /// <inheritdoc />
        public async Task<JsonElement> GetFaqsAsync()
        {
            try
            {

                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Get, "/content/faqs");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    return json;
                }

                _logger.LogWarning("El servicio externo /content/faqs retornó un código de estado: {StatusCode}", response.StatusCode);

                throw new HttpRequestException(
                    $"Error al obtener lista de FAQs. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al comunicarse con el servicio externo /content/faqs");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener lista de FAQs");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<JsonElement> GetFaqsBySlugAsync(string slug)
        {
            try
            {

                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Get, $"/content/faqs/{Uri.EscapeDataString(slug)}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    return json;
                }

                _logger.LogWarning("El servicio externo /content/faqs/{Slug} retornó un código de estado: {StatusCode}", slug, response.StatusCode);

                throw new HttpRequestException(
                    $"Error al obtener FAQs por slug. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al comunicarse con el servicio externo /content/faqs");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener FAQs por slug");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<JsonElement> GetLandingPagesAsync()
        {
            try
            {

                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Get, "/content/landing-page");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    return json;
                }

                _logger.LogWarning("El servicio externo /content/landing-page retornó un código de estado: {StatusCode}", response.StatusCode);

                throw new HttpRequestException(
                    $"Error al obtener lista de páginas de landing. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al comunicarse con el servicio externo /content/landing-page");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener lista de páginas de landing");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<JsonElement> GetLandingPageBySlugAsync(string slug)
        {
            try
            {

                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Get, $"/content/landing-page/{Uri.EscapeDataString(slug)}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    return json;
                }

                _logger.LogWarning("El servicio externo /content/landing-page/{Slug} retornó un código de estado: {StatusCode}", slug, response.StatusCode);

                throw new HttpRequestException(
                    $"Error al obtener página de landing por slug. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al comunicarse con el servicio externo /content/landing-page");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener página de landing por slug");
                throw;
            }
        }

    }
}

