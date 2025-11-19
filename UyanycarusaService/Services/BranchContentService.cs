using System.Text.Json;
using UyanycarusaService.ModelsTests;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Servicio para operaciones de contenido de sucursales
    /// </summary>
    public class BranchContentService : IBranchContentService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BranchContentService> _logger;
        private readonly bool _useTestData;

        public BranchContentService(
            IHttpClientFactory httpClientFactory,
            ILogger<BranchContentService> logger,
            IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("WebuyAnyCarApi");
            _logger = logger;
            _useTestData = configuration.GetValue<bool>("dataTest");
        }

        /// <inheritdoc />
        public async Task<JsonElement> GetBranchesAsync(string? zipCode = null, int? limit = null, string? branchType = null)
        {
            try
            {
                _logger.LogInformation("Solicitando lista de sucursales desde el servicio externo /content/branches");

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

                var response = await _httpClient.GetAsync(url);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    _logger.LogInformation("Lista de sucursales obtenida exitosamente");
                    return json;
                }

                _logger.LogWarning("El servicio externo /content/branches retornó un código de estado: {StatusCode}", response.StatusCode);

                if (_useTestData)
                {
                    _logger.LogInformation("Usando datos de prueba de sucursales desde BranchContentTestData (dataTest=true)");
                    return BranchContentTestData.GetBranches();
                }

                throw new HttpRequestException(
                    $"Error al obtener lista de sucursales. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                if (_useTestData)
                {
                    _logger.LogWarning(ex, "No se pudo comunicar con el servicio externo /content/branches, usando datos de prueba (dataTest=true)");
                    return BranchContentTestData.GetBranches();
                }

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
                _logger.LogInformation("Solicitando detalle de sucursal {BranchId} desde el servicio externo", branchId);

                var response = await _httpClient.GetAsync($"/content/branches/{branchId}");

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    _logger.LogInformation("Detalle de sucursal obtenido exitosamente");
                    return json;
                }

                _logger.LogWarning("El servicio externo /content/branches/{BranchId} retornó un código de estado: {StatusCode}", branchId, response.StatusCode);

                if (_useTestData)
                {
                    _logger.LogInformation("Usando datos de prueba de detalle de sucursal desde BranchContentTestData (dataTest=true)");
                    return BranchContentTestData.GetBranchDetail();
                }

                throw new HttpRequestException(
                    $"Error al obtener detalle de sucursal. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                if (_useTestData)
                {
                    _logger.LogWarning(ex, "No se pudo comunicar con el servicio externo /content/branches, usando datos de prueba (dataTest=true)");
                    return BranchContentTestData.GetBranchDetail();
                }

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

