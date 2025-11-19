namespace UyanycarusaService.Dtos
{
    /// <summary>
    /// DTO para solicitud de autenticación
    /// </summary>
    public record LoginRequest
    {
        /// <summary>
        /// Nombre de usuario
        /// </summary>
        /// <example>admin</example>
        public string Username { get; init; } = string.Empty;

        /// <summary>
        /// Contraseña del usuario
        /// </summary>
        /// <example>password123</example>
        public string Password { get; init; } = string.Empty;
    }
}

