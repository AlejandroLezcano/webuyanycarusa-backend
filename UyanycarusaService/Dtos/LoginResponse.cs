namespace UyanycarusaService.Dtos
{
    /// <summary>
    /// DTO para respuesta de autenticación
    /// </summary>
    public record LoginResponse
    {
        /// <summary>
        /// Token JWT para autenticación
        /// </summary>
        public string Token { get; init; } = string.Empty;

        /// <summary>
        /// Fecha de expiración del token
        /// </summary>
        public DateTime ExpiresAt { get; init; }
    }
}

