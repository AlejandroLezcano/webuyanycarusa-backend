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

        public VehiclesService(
            IHttpClientFactory httpClientFactory,
            ILogger<VehiclesService> logger,
            IConfiguration configuration,
            ITokenService tokenService)
        {
            _httpClient = httpClientFactory.CreateClient("WebuyAnyCarApi");
            _logger = logger;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Obtiene la lista de años disponibles de vehículos desde el servicio externo
        /// </summary>
        /// <returns>Lista de años disponibles</returns>
        public async Task<List<int>> GetYearsAsync()
        {
            try
            {
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

                if (response.IsSuccessStatusCode){
                    var content = await response.Content.ReadAsStringAsync();
                    var json = JsonSerializer.Deserialize<JsonElement>(content);

                    return json;
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
            try
            {
                var httpClient = new HttpClient();
                var responseImage = await httpClient.GetAsync(imageUrl);

                if (responseImage.IsSuccessStatusCode)
                {
                    var imageContent = await responseImage.Content.ReadAsByteArrayAsync();
                    var contentType = responseImage.Content.Headers.ContentType?.MediaType ?? "image/jpeg";


                    return (imageContent, contentType);
                }
                else
                {
                    var errorContent = await responseImage.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Error al obtener la imagen desde {imageUrl}. StatusCode: {responseImage.StatusCode}, Detail: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al comunicarse con el servicio externo para obtener la imagen desde {ImageUrl}", imageUrl);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener la imagen desde {ImageUrl}", imageUrl);
                throw;
            }
        }
    }
}

