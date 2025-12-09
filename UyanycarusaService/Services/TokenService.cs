using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Servicio para gestionar tokens de Azure AD con cache
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly IAuthService _authService;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<TokenService> _logger;
        private readonly SemaphoreSlim _tokenSemaphore;
        private const string CacheKey = "AzureAd_AccessToken";

        public TokenService(
            IAuthService authService,
            IMemoryCache memoryCache,
            ILogger<TokenService> logger)
        {
            _authService = authService;
            _memoryCache = memoryCache;
            _logger = logger;
            _tokenSemaphore = new SemaphoreSlim(1, 1); // Solo permite una solicitud de token a la vez
        }

        /// <inheritdoc />
        public async Task<string> GetAccessTokenAsync()
        {
            // Intentar obtener el token del cache
            if (_memoryCache.TryGetValue(CacheKey, out string? cachedToken) && !string.IsNullOrEmpty(cachedToken))
            {
                _logger.LogDebug("Token obtenido desde cache");
                return cachedToken;
            }

            // Si no está en cache o expiró, obtener uno nuevo
            // Usar semáforo para evitar múltiples solicitudes concurrentes
            await _tokenSemaphore.WaitAsync();
            try
            {
                // Verificar nuevamente el cache después de adquirir el lock
                // (otro hilo pudo haber obtenido el token mientras esperábamos)
                if (_memoryCache.TryGetValue(CacheKey, out string? cachedTokenAfterLock) && !string.IsNullOrEmpty(cachedTokenAfterLock))
                {
                    _logger.LogDebug("Token obtenido desde cache después de adquirir lock (otro hilo lo obtuvo)");
                    return cachedTokenAfterLock;
                }

                _logger.LogInformation("Getting new Azure AD token (empty cache)");
                var tokenResponse = await _authService.GetTokenAsync();

                // Extract token and expiration time
                var accessToken = tokenResponse.GetProperty("access_token").GetString();
                var expiresIn = tokenResponse.TryGetProperty("expires_in", out var expiresInProp)
                    ? expiresInProp.GetInt32()
                    : 3600; // Default: 1 hour if expires_in is not provided

                if (string.IsNullOrEmpty(accessToken))
                {
                    throw new InvalidOperationException("Received access token is empty");
                }

                var cacheExpiration = TimeSpan.FromSeconds(expiresIn - 300);
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = cacheExpiration
                };

                _memoryCache.Set(CacheKey, accessToken, cacheOptions);
                _logger.LogInformation("Token de Azure AD obtenido y almacenado en cache. Expira en {Expiration} segundos", expiresIn - 300);

                return accessToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener token de Azure AD");
                throw;
            }
            finally
            {
                _tokenSemaphore.Release();
            }
        }
    }
}

