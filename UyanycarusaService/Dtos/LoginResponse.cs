namespace UyanycarusaService.Dtos
{
    /// <summary>
    /// DTO for authentication response
    /// </summary>
    public record LoginResponse
    {
        /// <summary>
        /// JWT token for authentication
        /// </summary>
        public string Token { get; init; } = string.Empty;

        /// <summary>
        /// Token expiration date
        /// </summary>
        public DateTime ExpiresAt { get; init; }
    }
}

