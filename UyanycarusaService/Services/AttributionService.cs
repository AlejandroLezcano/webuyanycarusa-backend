using System.Net.Http.Json;
using System.Text.Json;
using UyanycarusaService.ModelsTests;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Servicio para operaciones de atribución
    /// </summary>
    public class AttributionService : IAttributionService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AttributionService> _logger;
        private readonly bool _useTestData;

        public AttributionService(
            IHttpClientFactory httpClientFactory,
            ILogger<AttributionService> logger,
            IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("WebuyAnyCarApi");
            _logger = logger;
            _useTestData = configuration.GetValue<bool>("dataTest");
        }

        /// <inheritdoc />
        public async Task<JsonElement> CreateOrGetVisitorAsync(long? oldVisitorId = null)
        {
            try
            {
                _logger.LogInformation("Creando u obteniendo visitor desde el servicio externo /Attribution/visitor");

                var url = "/Attribution/visitor";
                if (oldVisitorId.HasValue)
                {
                    url += $"?oldVisitorId={oldVisitorId.Value}";
                }

                var response = await _httpClient.PostAsync(url, null);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    _logger.LogInformation("Visitor creado u obtenido exitosamente");
                    return json;
                }

                _logger.LogWarning("El servicio externo /Attribution/visitor retornó un código de estado: {StatusCode}", response.StatusCode);

                if (_useTestData)
                {
                    _logger.LogInformation("Usando datos de prueba de visitor desde AttributionTestData (dataTest=true)");
                    return AttributionTestData.GetVisitor();
                }

                throw new HttpRequestException(
                    $"Error al crear u obtener visitor. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                if (_useTestData)
                {
                    _logger.LogWarning(ex, "No se pudo comunicar con el servicio externo /Attribution/visitor, usando datos de prueba (dataTest=true)");
                    return AttributionTestData.GetVisitor();
                }

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
                _logger.LogInformation("Registrando visita para visitor {VisitorId} en el servicio externo", visitorId);

                using var response = await _httpClient.PostAsJsonAsync($"/Attribution/visitor/{visitorId}/visit", model);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    _logger.LogInformation("Visita registrada exitosamente");
                    return json;
                }

                _logger.LogWarning("El servicio externo /Attribution/visitor/visit retornó un código de estado: {StatusCode}", response.StatusCode);

                if (_useTestData)
                {
                    _logger.LogInformation("Usando datos de prueba de visita desde AttributionTestData (dataTest=true)");
                    return AttributionTestData.GetVisit();
                }

                throw new HttpRequestException(
                    $"Error al registrar visita. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                if (_useTestData)
                {
                    _logger.LogWarning(ex, "No se pudo comunicar con el servicio externo /Attribution/visitor/visit, usando datos de prueba (dataTest=true)");
                    return AttributionTestData.GetVisit();
                }

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

