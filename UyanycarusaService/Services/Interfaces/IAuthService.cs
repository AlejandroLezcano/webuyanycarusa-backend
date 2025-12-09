using System.Text.Json;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Interfaz para el servicio de autenticación con Azure AD
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Obtiene un token de acceso de Azure AD usando el flujo de credenciales de cliente (client credentials)
        /// </summary>
        /// <returns>Respuesta del token como JSON</returns>
        Task<JsonElement> GetTokenAsync();
    }
}

