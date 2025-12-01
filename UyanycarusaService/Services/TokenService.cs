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
        private const string CacheKey = "AzureAd_AccessToken";

        public TokenService(
            IAuthService authService,
            IMemoryCache memoryCache,
            ILogger<TokenService> logger)
        {
            _authService = authService;
            _memoryCache = memoryCache;
            _logger = logger;
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

            try
            {
                var tokenResponse = await _authService.GetTokenAsync();

                // Extraer el token y el tiempo de expiración
                var accessToken = tokenResponse.GetProperty("access_token").GetString();
                var expiresIn = tokenResponse.TryGetProperty("expires_in", out var expiresInProp)
                    ? expiresInProp.GetInt32()
                    : 3600; // Default: 1 hora si no viene expires_in

                if (string.IsNullOrEmpty(accessToken))
                {
                    throw new InvalidOperationException("El token de acceso recibido está vacío");
                }

                var cacheExpiration = TimeSpan.FromSeconds(expiresIn - 300);
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = cacheExpiration
                };

                _memoryCache.Set(CacheKey, accessToken, cacheOptions);

                return accessToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener token de Azure AD");
                throw;
            }
        }
    }
}

