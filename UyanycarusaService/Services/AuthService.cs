using System.Net.Http.Headers;
using System.Text.Json;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Servicio para operaciones de autenticación con Azure AD
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthService> _logger;
        private readonly IConfiguration _configuration;

        public AuthService(
            IHttpClientFactory httpClientFactory,
            ILogger<AuthService> logger,
            IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
            _configuration = configuration;
        }

        /// <inheritdoc />
        public async Task<JsonElement> GetTokenAsync()
        {
            try
            {


                var tenantId = _configuration["AzureAd:TenantId"]
                    ?? throw new InvalidOperationException("AzureAd:TenantId no está configurado");
                var clientId = _configuration["AzureAd:ClientId"]
                    ?? throw new InvalidOperationException("AzureAd:ClientId no está configurado");
                var clientSecret = _configuration["AzureAd:ClientSecret"]
                    ?? throw new InvalidOperationException("AzureAd:ClientSecret no está configurado");
                var scope = _configuration["AzureAd:Scope"]
                    ?? throw new InvalidOperationException("AzureAd:Scope no está configurado");

                var tokenUrl = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";

                var formData = new Dictionary<string, string>
                {
                    { "grant_type", "client_credentials" },
                    { "client_id", clientId },
                    { "scope", scope },
                    { "client_secret", clientSecret }
                };

                var formContent = new FormUrlEncodedContent(formData);
                formContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                var response = await _httpClient.PostAsync(tokenUrl, formContent);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    return json;
                }

                _logger.LogError("Error al obtener token de Azure AD. StatusCode: {StatusCode}, Response: {Response}",
                    response.StatusCode, content);

                throw new HttpRequestException(
                    $"Error al obtener token de Azure AD. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error de comunicación al obtener token de Azure AD");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener token de Azure AD");
                throw;
            }
        }
    }
}

