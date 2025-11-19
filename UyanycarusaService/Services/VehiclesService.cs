using System.Net.Http.Json;
using UyanycarusaService.ModelsTests;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Servicio para operaciones relacionadas con vehículos
    /// </summary>
    public class VehiclesService : IVehiclesService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<VehiclesService> _logger;
        private readonly bool _useTestData;

        public VehiclesService(IHttpClientFactory httpClientFactory, ILogger<VehiclesService> logger, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("WebuyAnyCarApi");
            _logger = logger;
            _useTestData = configuration.GetValue<bool>("dataTest");
        }

        /// <summary>
        /// Obtiene la lista de años disponibles de vehículos desde el servicio externo
        /// </summary>
        /// <returns>Lista de años disponibles</returns>
        public async Task<List<int>> GetYearsAsync()
        {
            try
            {
                _logger.LogInformation("Solicitando años de vehículos desde el servicio externo");

                var response = await _httpClient.GetAsync("/Vehicles/years");

                if (response.IsSuccessStatusCode)
                {
                    var years = await response.Content.ReadFromJsonAsync<List<int>>();
                    _logger.LogInformation("Se obtuvieron {Count} años exitosamente", years?.Count ?? 0);
                    return years ?? new List<int>();
                }

                _logger.LogWarning("El servicio externo retornó un código de estado: {StatusCode}", response.StatusCode);

                if (_useTestData)
                {
                    _logger.LogInformation("Usando datos de prueba de años desde VehiclesTestData (dataTest=true)");
                    return VehiclesTestData.Years.ToList();
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error al obtener años del servicio externo. StatusCode: {response.StatusCode}, Detail: {errorContent}");
            }
            catch (HttpRequestException ex)
            {
                if (_useTestData)
                {
                    _logger.LogWarning(ex, "No se pudo comunicar con el servicio externo de años, usando datos de prueba (dataTest=true)");
                    return VehiclesTestData.Years.ToList();
                }

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener años de vehículos");
                throw;
            }
        }

        /// <summary>
        /// Obtiene la lista de marcas disponibles para un año específico desde el servicio externo
        /// </summary>
        /// <param name="year">Año del vehículo</param>
        /// <returns>Lista de marcas disponibles</returns>
        public async Task<List<string>> GetMakesAsync(int year)
        {
            try
            {
                _logger.LogInformation("Solicitando marcas de vehículos para el año {Year} desde el servicio externo", year);

                var response = await _httpClient.GetAsync($"/Vehicles/makes/{year}");

                if (response.IsSuccessStatusCode)
                {
                    var makes = await response.Content.ReadFromJsonAsync<List<string>>();
                    _logger.LogInformation("Se obtuvieron {Count} marcas exitosamente para el año {Year}", makes?.Count ?? 0, year);
                    return makes ?? new List<string>();
                }

                _logger.LogWarning("El servicio externo retornó un código de estado: {StatusCode} para el año {Year}", response.StatusCode, year);

                if (_useTestData)
                {
                    _logger.LogInformation("Usando datos de prueba de marcas desde VehiclesTestData (dataTest=true)");
                    return VehiclesTestData.DefaultMakes.ToList();
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error al obtener marcas del servicio externo para el año {year}. StatusCode: {response.StatusCode}, Detail: {errorContent}");
            }
            catch (HttpRequestException ex)
            {
                if (_useTestData)
                {
                    _logger.LogWarning(ex, "No se pudo comunicar con el servicio externo de marcas, usando datos de prueba (dataTest=true)");
                    return VehiclesTestData.DefaultMakes.ToList();
                }

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener marcas de vehículos para el año {Year}", year);
                throw;
            }
        }

        /// <summary>
        /// Obtiene la lista de modelos disponibles para un año y marca específicos desde el servicio externo
        /// </summary>
        /// <param name="year">Año del vehículo</param>
        /// <param name="make">Marca del vehículo</param>
        /// <returns>Lista de modelos disponibles</returns>
        public async Task<List<string>> GetModelsAsync(int year, string make)
        {
            try
            {
                _logger.LogInformation("Solicitando modelos de vehículos para el año {Year} y marca {Make} desde el servicio externo", year, make);

                var response = await _httpClient.GetAsync($"/Vehicles/models/{year}/{Uri.EscapeDataString(make)}");

                if (response.IsSuccessStatusCode)
                {
                    var models = await response.Content.ReadFromJsonAsync<List<string>>();
                    _logger.LogInformation("Se obtuvieron {Count} modelos exitosamente para el año {Year} y marca {Make}", models?.Count ?? 0, year, make);
                    return models ?? new List<string>();
                }

                _logger.LogWarning("El servicio externo retornó un código de estado: {StatusCode} para el año {Year} y marca {Make}", response.StatusCode, year, make);

                if (_useTestData)
                {
                    _logger.LogInformation("Usando datos de prueba de modelos desde VehiclesTestData (dataTest=true)");
                    return VehiclesTestData.DefaultModels.ToList();
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error al obtener modelos del servicio externo para el año {year} y marca {make}. StatusCode: {response.StatusCode}, Detail: {errorContent}");
            }
            catch (HttpRequestException ex)
            {
                if (_useTestData)
                {
                    _logger.LogWarning(ex, "No se pudo comunicar con el servicio externo de modelos, usando datos de prueba (dataTest=true)");
                    return VehiclesTestData.DefaultModels.ToList();
                }

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener modelos de vehículos para el año {Year} y marca {Make}", year, make);
                throw;
            }
        }
    }
}

