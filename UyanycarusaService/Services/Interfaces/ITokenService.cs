namespace UyanycarusaService.Services
{
    /// <summary>
    /// Interfaz para el servicio de gestión de tokens de Azure AD
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Obtiene un token de acceso válido (desde cache o solicitando uno nuevo)
        /// </summary>
        /// <returns>Token de acceso de Azure AD</returns>
        Task<string> GetAccessTokenAsync();
    }
}

