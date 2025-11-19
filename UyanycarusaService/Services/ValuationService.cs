using System.Net.Http.Json;
using System.Text.Json;
using UyanycarusaService.ModelsTests;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Servicio para operaciones de valuación de vehículos
    /// </summary>
    public class ValuationService : IValuationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ValuationService> _logger;
        private readonly bool _useTestData;

        public ValuationService(IHttpClientFactory httpClientFactory, ILogger<ValuationService> logger, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("WebuyAnyCarApi");
            _logger = logger;
            _useTestData = configuration.GetValue<bool>("dataTest");
        }

        /// <inheritdoc />
        public async Task<JsonElement> CreateValuationAsync(JsonElement model)
        {
            try
            {
                _logger.LogInformation("Enviando valuación básica al servicio externo /Valuation");

                using var response = await _httpClient.PostAsJsonAsync("/Valuation", model);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    _logger.LogInformation("Valuación básica completada con éxito");
                    return json;
                }

                _logger.LogWarning("El servicio externo /Valuation retornó un código de estado: {StatusCode}", response.StatusCode);

                if (_useTestData)
                {
                    _logger.LogInformation("Usando datos de prueba de valuación básica desde ValuationTestData (dataTest=true)");
                    return ValuationTestData.GetBasicValuation();
                }

                throw new HttpRequestException(
                    $"Error al realizar la valuación básica. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                if (_useTestData)
                {
                    _logger.LogWarning(ex, "No se pudo comunicar con el servicio externo /Valuation, usando datos de prueba (dataTest=true)");
                    return ValuationTestData.GetBasicValuation();
                }

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
                _logger.LogInformation("Enviando valuación con daños al servicio externo /Valuation/with-damage");

                using var response = await _httpClient.PostAsJsonAsync("/Valuation/with-damage", model);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    _logger.LogInformation("Valuación con daños completada con éxito");
                    return json;
                }

                _logger.LogWarning("El servicio externo /Valuation/with-damage retornó un código de estado: {StatusCode}", response.StatusCode);

                if (_useTestData)
                {
                    _logger.LogInformation("Usando datos de prueba de valuación con daños desde ValuationTestData (dataTest=true)");
                    return ValuationTestData.GetValuationWithDamage();
                }

                throw new HttpRequestException(
                    $"Error al realizar la valuación con daños. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                if (_useTestData)
                {
                    _logger.LogWarning(ex, "No se pudo comunicar con el servicio externo /Valuation/with-damage, usando datos de prueba (dataTest=true)");
                    return ValuationTestData.GetValuationWithDamage();
                }

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


