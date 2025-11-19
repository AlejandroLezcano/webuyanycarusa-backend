using System.Text.Json;
using UyanycarusaService.ModelsTests;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Servicio para operaciones de contenido
    /// </summary>
    public class ContentService : IContentService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ContentService> _logger;
        private readonly bool _useTestData;

        public ContentService(
            IHttpClientFactory httpClientFactory,
            ILogger<ContentService> logger,
            IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("WebuyAnyCarApi");
            _logger = logger;
            _useTestData = configuration.GetValue<bool>("dataTest");
        }

        /// <inheritdoc />
        public async Task<JsonElement> GetFaqsAsync()
        {
            try
            {
                _logger.LogInformation("Solicitando lista de FAQs desde el servicio externo /content/faqs");

                var response = await _httpClient.GetAsync("/content/faqs");

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    _logger.LogInformation("Lista de FAQs obtenida exitosamente");
                    return json;
                }

                _logger.LogWarning("El servicio externo /content/faqs retornó un código de estado: {StatusCode}", response.StatusCode);

                if (_useTestData)
                {
                    _logger.LogInformation("Usando datos de prueba de FAQs desde ContentTestData (dataTest=true)");
                    return ContentTestData.GetFaqs();
                }

                throw new HttpRequestException(
                    $"Error al obtener lista de FAQs. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                if (_useTestData)
                {
                    _logger.LogWarning(ex, "No se pudo comunicar con el servicio externo /content/faqs, usando datos de prueba (dataTest=true)");
                    return ContentTestData.GetFaqs();
                }

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
                _logger.LogInformation("Solicitando FAQs por slug {Slug} desde el servicio externo", slug);

                var response = await _httpClient.GetAsync($"/content/faqs/{Uri.EscapeDataString(slug)}");

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    _logger.LogInformation("FAQs por slug obtenidas exitosamente");
                    return json;
                }

                _logger.LogWarning("El servicio externo /content/faqs/{Slug} retornó un código de estado: {StatusCode}", slug, response.StatusCode);

                if (_useTestData)
                {
                    _logger.LogInformation("Usando datos de prueba de FAQs por slug desde ContentTestData (dataTest=true)");
                    return ContentTestData.GetFaqsBySlug();
                }

                throw new HttpRequestException(
                    $"Error al obtener FAQs por slug. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                if (_useTestData)
                {
                    _logger.LogWarning(ex, "No se pudo comunicar con el servicio externo /content/faqs, usando datos de prueba (dataTest=true)");
                    return ContentTestData.GetFaqsBySlug();
                }

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
                _logger.LogInformation("Solicitando lista de páginas de landing desde el servicio externo /content/landing-page");

                var response = await _httpClient.GetAsync("/content/landing-page");

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    _logger.LogInformation("Lista de páginas de landing obtenida exitosamente");
                    return json;
                }

                _logger.LogWarning("El servicio externo /content/landing-page retornó un código de estado: {StatusCode}", response.StatusCode);

                if (_useTestData)
                {
                    _logger.LogInformation("Usando datos de prueba de landing pages desde ContentTestData (dataTest=true)");
                    return ContentTestData.GetLandingPages();
                }

                throw new HttpRequestException(
                    $"Error al obtener lista de páginas de landing. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                if (_useTestData)
                {
                    _logger.LogWarning(ex, "No se pudo comunicar con el servicio externo /content/landing-page, usando datos de prueba (dataTest=true)");
                    return ContentTestData.GetLandingPages();
                }

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
                _logger.LogInformation("Solicitando página de landing por slug {Slug} desde el servicio externo", slug);

                var response = await _httpClient.GetAsync($"/content/landing-page/{Uri.EscapeDataString(slug)}");

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    _logger.LogInformation("Página de landing por slug obtenida exitosamente");
                    return json;
                }

                _logger.LogWarning("El servicio externo /content/landing-page/{Slug} retornó un código de estado: {StatusCode}", slug, response.StatusCode);

                if (_useTestData)
                {
                    _logger.LogInformation("Usando datos de prueba de landing page por slug desde ContentTestData (dataTest=true)");
                    return ContentTestData.GetLandingPageBySlug();
                }

                throw new HttpRequestException(
                    $"Error al obtener página de landing por slug. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                if (_useTestData)
                {
                    _logger.LogWarning(ex, "No se pudo comunicar con el servicio externo /content/landing-page, usando datos de prueba (dataTest=true)");
                    return ContentTestData.GetLandingPageBySlug();
                }

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

