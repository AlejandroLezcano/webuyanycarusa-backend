using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Servicio para operaciones relacionadas con vehículos
    /// </summary>
    public class VehiclesService : IVehiclesService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<VehiclesService> _logger;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly bool _useMockData;

        public VehiclesService(
            IHttpClientFactory httpClientFactory,
            ILogger<VehiclesService> logger,
            IConfiguration configuration,
            ITokenService tokenService)
        {
            _httpClient = httpClientFactory.CreateClient("WebuyAnyCarApi");
            _logger = logger;
            _tokenService = tokenService;
            _configuration = configuration;
            _useMockData = _configuration.GetValue<bool>("DevelopmentMode:UseMockData", false);
        }

        /// <summary>
        /// Obtiene la lista de años disponibles de vehículos desde el servicio externo
        /// </summary>
        /// <returns>Lista de años disponibles</returns>
        public async Task<List<int>> GetYearsAsync()
        {
            try
            {
                // If in development mode with mock data, return mock years
                if (_useMockData)
                {
                    _logger.LogInformation("Using mock data for years");
                    return GetMockYears();
                }

                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Get, "/Vehicles/years");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode){
                    var years = await response.Content.ReadFromJsonAsync<List<int>>();
                    return years ?? new List<int>();
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error al obtener años del servicio externo. StatusCode: {response.StatusCode}, Detail: {errorContent}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al comunicarse con el servicio externo de años");
                
                // Fallback to mock data if external service fails
                if (_useMockData)
                {
                    _logger.LogWarning("Falling back to mock data due to external service error");
                    return GetMockYears();
                }
                
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener años de vehículos");
                
                // Fallback to mock data if any error occurs in development
                if (_useMockData)
                {
                    _logger.LogWarning("Falling back to mock data due to unexpected error");
                    return GetMockYears();
                }
                
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
                // If in development mode with mock data, return mock makes
                if (_useMockData)
                {
                    _logger.LogInformation("Using mock data for makes for year {Year}", year);
                    return GetMockMakes();
                }

                // Obtener token de Azure AD y crear request con el header
                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Get, $"/Vehicles/makes/{year}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var makes = await response.Content.ReadFromJsonAsync<List<string>>();
                    return makes ?? new List<string>();
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error al obtener marcas del servicio externo para el año {year}. StatusCode: {response.StatusCode}, Detail: {errorContent}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al comunicarse con el servicio externo de marcas");
                
                // Fallback to mock data if external service fails
                if (_useMockData)
                {
                    _logger.LogWarning("Falling back to mock data for makes due to external service error");
                    return GetMockMakes();
                }
                
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener marcas de vehículos para el año {Year}", year);
                
                // Fallback to mock data if any error occurs in development
                if (_useMockData)
                {
                    _logger.LogWarning("Falling back to mock data for makes due to unexpected error");
                    return GetMockMakes();
                }
                
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
                // Obtener token de Azure AD y crear request con el header
                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Get, $"/Vehicles/models/{year}/{Uri.EscapeDataString(make)}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var models = await response.Content.ReadFromJsonAsync<List<string>>();
                    return models ?? new List<string>();
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error al obtener modelos del servicio externo para el año {year} y marca {make}. StatusCode: {response.StatusCode}, Detail: {errorContent}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al comunicarse con el servicio externo de modelos");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener modelos de vehículos para el año {Year} y marca {Make}", year, make);
                throw;
            }
        }

        /// <summary>
        /// Obtiene la lista de trims (versiones/equipamientos) disponibles para un año, marca y modelo específicos desde el servicio externo
        /// </summary>
        /// <param name="year">Año del vehículo</param>
        /// <param name="make">Marca del vehículo</param>
        /// <param name="model">Modelo del vehículo</param>
        /// <returns>Lista de trims disponibles como JSON</returns>
        public async Task<JsonElement> GetTrimsAsync(int year, string make, string model)
        {
            try
            {
                // Obtener token de Azure AD y crear request con el header
                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Get, $"/Vehicles/trims/{year}/{Uri.EscapeDataString(make)}/{Uri.EscapeDataString(model)}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    return json;
                }

                // If external service returns NotFound, return empty array (vehicle not in their database)
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogWarning("External service returned NotFound for trims: Year={Year}, Make={Make}, Model={Model}. Returning empty array.", year, make, model);
                    return JsonSerializer.Deserialize<JsonElement>("[]");
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error al obtener trims del servicio externo para el año {year}, marca {make} y modelo {model}. StatusCode: {response.StatusCode}, Detail: {errorContent}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al comunicarse con el servicio externo de trims");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener trims de vehículos para el año {Year}, marca {Make} y modelo {Model}", year, make, model);
                throw;
            }
        }

        /// <summary>
        /// Obtiene una imagen desde una URL externa
        /// </summary>
        /// <param name="imageUrl">URL de la imagen a obtener</param>
        /// <returns>Tupla con el contenido de la imagen (bytes) y el tipo de contenido (content type)</returns>
        public async Task<(byte[] content, string contentType)> GetImageAsync(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                throw new ArgumentException("imageUrl es requerido", nameof(imageUrl));

            var response = await _httpClient.GetAsync(imageUrl);

            if (!response.IsSuccessStatusCode)
            {
                // esto lo va a atrapar el controller
                throw new HttpRequestException(
                    $"No se pudo obtener la imagen. StatusCode: {(int)response.StatusCode}");
            }

            var bytes = await response.Content.ReadAsByteArrayAsync();

            // intenta tomar el content-type real, si no, usa image/jpeg por defecto
            var contentType = response.Content.Headers.ContentType?.MediaType ?? "image/jpeg";

            return (bytes, contentType);
        }

        #region Mock Data Methods

        private List<int> GetMockYears()
        {
            var currentYear = DateTime.Now.Year;
            var years = new List<int>();
            
            // Generate years from 1990 to current year + 1
            for (int year = 1990; year <= currentYear + 1; year++)
            {
                years.Add(year);
            }
            
            return years.OrderByDescending(y => y).ToList();
        }

        private List<string> GetMockMakes()
        {
            return new List<string>
            {
                "Acura", "Audi", "BMW", "Buick", "Cadillac", "Chevrolet", "Chrysler", 
                "Dodge", "Ford", "GMC", "Honda", "Hyundai", "Infiniti", "Jeep", 
                "Kia", "Lexus", "Lincoln", "Mazda", "Mercedes-Benz", "Mitsubishi", 
                "Nissan", "Ram", "Subaru", "Tesla", "Toyota", "Volkswagen", "Volvo"
            };
        }

        private List<string> GetMockModels(string make)
        {
            var mockModels = new Dictionary<string, List<string>>
            {
                ["Toyota"] = new List<string> { "Camry", "Corolla", "RAV4", "Highlander", "Prius", "Tacoma", "Tundra" },
                ["Honda"] = new List<string> { "Civic", "Accord", "CR-V", "Pilot", "Odyssey", "Ridgeline" },
                ["Ford"] = new List<string> { "F-150", "Mustang", "Explorer", "Escape", "Focus", "Fusion" },
                ["Chevrolet"] = new List<string> { "Silverado", "Equinox", "Malibu", "Cruze", "Tahoe", "Suburban" },
                ["BMW"] = new List<string> { "3 Series", "5 Series", "X3", "X5", "7 Series", "i3" },
                ["Mercedes-Benz"] = new List<string> { "C-Class", "E-Class", "S-Class", "GLE", "GLC", "A-Class" }
            };

            return mockModels.ContainsKey(make) 
                ? mockModels[make] 
                : new List<string> { "Model 1", "Model 2", "Model 3" };
        }

        #endregion
    }
}

