using System.Text.Json;
using UyanycarusaService.ModelsTests;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Servicio para operaciones de contenido de marca y modelo
    /// </summary>
    public class MakeModelContentService : IMakeModelContentService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MakeModelContentService> _logger;
        private readonly bool _useTestData;

        public MakeModelContentService(
            IHttpClientFactory httpClientFactory,
            ILogger<MakeModelContentService> logger,
            IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("WebuyAnyCarApi");
            _logger = logger;
            _useTestData = configuration.GetValue<bool>("dataTest");
        }

        /// <inheritdoc />
        public async Task<JsonElement> GetMakeContentAsync(string make)
        {
            try
            {
                _logger.LogInformation("Solicitando contenido de marca {Make} desde el servicio externo", make);

                var response = await _httpClient.GetAsync($"/content/make-model/{Uri.EscapeDataString(make)}");

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    _logger.LogInformation("Contenido de marca obtenido exitosamente");
                    return json;
                }

                _logger.LogWarning("El servicio externo /content/make-model/{Make} retornó un código de estado: {StatusCode}", make, response.StatusCode);

                if (_useTestData)
                {
                    _logger.LogInformation("Usando datos de prueba de contenido de marca desde MakeModelContentTestData (dataTest=true)");
                    return MakeModelContentTestData.GetMakeContent();
                }

                throw new HttpRequestException(
                    $"Error al obtener contenido de marca. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                if (_useTestData)
                {
                    _logger.LogWarning(ex, "No se pudo comunicar con el servicio externo /content/make-model, usando datos de prueba (dataTest=true)");
                    return MakeModelContentTestData.GetMakeContent();
                }

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
                _logger.LogInformation("Solicitando contenido de marca {Make} y modelo {Model} desde el servicio externo", make, model);

                var response = await _httpClient.GetAsync($"/content/make-model/{Uri.EscapeDataString(make)}/{Uri.EscapeDataString(model)}");

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    _logger.LogInformation("Contenido de marca y modelo obtenido exitosamente");
                    return json;
                }

                _logger.LogWarning("El servicio externo /content/make-model/{Make}/{Model} retornó un código de estado: {StatusCode}", make, model, response.StatusCode);

                if (_useTestData)
                {
                    _logger.LogInformation("Usando datos de prueba de contenido de marca y modelo desde MakeModelContentTestData (dataTest=true)");
                    return MakeModelContentTestData.GetMakeModelContent();
                }

                throw new HttpRequestException(
                    $"Error al obtener contenido de marca y modelo. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                if (_useTestData)
                {
                    _logger.LogWarning(ex, "No se pudo comunicar con el servicio externo /content/make-model, usando datos de prueba (dataTest=true)");
                    return MakeModelContentTestData.GetMakeModelContent();
                }

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

