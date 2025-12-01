using System.Net.Http.Headers;
using System.Text.Json;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Servicio para operaciones de contenido de sucursales
    /// </summary>
    public class BranchContentService : IBranchContentService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BranchContentService> _logger;
        private readonly ITokenService _tokenService;

        public BranchContentService(
            IHttpClientFactory httpClientFactory,
            ILogger<BranchContentService> logger,
            IConfiguration configuration,
            ITokenService tokenService)
        {
            _httpClient = httpClientFactory.CreateClient("WebuyAnyCarApi");
            _logger = logger;
            _tokenService = tokenService;
        }

        /// <inheritdoc />
        public async Task<JsonElement> GetBranchesAsync(string? zipCode = null, int? limit = null, string? branchType = null)
        {
            try
            {

                var queryParams = new List<string>();
                if (!string.IsNullOrEmpty(zipCode))
                {
                    queryParams.Add($"ZipCode={Uri.EscapeDataString(zipCode)}");
                }
                if (limit.HasValue)
                {
                    queryParams.Add($"Limit={limit.Value}");
                }
                if (!string.IsNullOrEmpty(branchType))
                {
                    queryParams.Add($"BranchType={Uri.EscapeDataString(branchType)}");
                }

                var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
                var url = $"/content/branches{queryString}";

                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    return json;
                }

                _logger.LogWarning("El servicio externo /content/branches retornó un código de estado: {StatusCode}", response.StatusCode);

                throw new HttpRequestException(
                    $"Error al obtener lista de sucursales. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al comunicarse con el servicio externo /content/branches");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener lista de sucursales");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<JsonElement> GetBranchDetailAsync(int branchId)
        {
            try
            {

                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Get, $"/content/branches/{branchId}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    return json;
                }

                _logger.LogWarning("El servicio externo /content/branches/{BranchId} retornó un código de estado: {StatusCode}", branchId, response.StatusCode);

                throw new HttpRequestException(
                    $"Error al obtener detalle de sucursal. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al comunicarse con el servicio externo /content/branches");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener detalle de sucursal");
                throw;
            }
        }
    }
}

