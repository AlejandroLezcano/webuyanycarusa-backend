namespace UyanycarusaService.Services
{
    /// <summary>
    /// Interface for Azure AD token management service
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Gets a valid access token (from cache or requesting a new one)
        /// </summary>
        /// <returns>Azure AD access token</returns>
        Task<string> GetAccessTokenAsync();
    }
}

