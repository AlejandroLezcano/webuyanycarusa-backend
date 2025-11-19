using UyanycarusaService.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UyanycarusaService.Controllers
{
    /// <summary>
    /// Controlador para autenticación y generación de tokens JWT
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration configuration, ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Autentica un usuario y genera un token JWT
        /// </summary>
        /// <param name="request">Credenciales de autenticación</param>
        /// <returns>Token JWT y fecha de expiración</returns>
        /// <response code="200">Autenticación exitosa. Retorna el token JWT</response>
        /// <response code="400">Solicitud inválida. Las credenciales son incorrectas</response>
        /// <response code="429">Demasiadas solicitudes. Se ha excedido el límite de rate limiting</response>
        /// <remarks>
        /// Ejemplo de solicitud:
        ///
        ///     POST /api/v1/auth/login
        ///     {
        ///         "username": "admin",
        ///         "password": "password123"
        ///     }
        ///
        /// **Nota:** En un entorno de producción, las credenciales deben validarse contra una base de datos o servicio de autenticación.
        /// Este es un ejemplo simplificado que acepta cualquier credencial.
        /// </remarks>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Username y Password son requeridos" });
            }

            // En producción, validar contra base de datos o servicio de autenticación
            // Por ahora, aceptamos cualquier credencial para demostración
            // TODO: Implementar validación real de credenciales

            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is not configured");
            var issuer = jwtSettings["Issuer"] ?? "UyanycarusaService";
            var audience = jwtSettings["Audience"] ?? "UyanycarusaServiceUsers";
            var expirationMinutes = int.Parse(jwtSettings["ExpirationInMinutes"] ?? "60");

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, request.Username),
                new Claim(ClaimTypes.NameIdentifier, request.Username),
                new Claim(JwtRegisteredClaimNames.Sub, request.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(expirationMinutes);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            _logger.LogInformation("Token generado para usuario: {Username}", request.Username);

            return Ok(new LoginResponse
            {
                Token = tokenString,
                ExpiresAt = expires
            });
        }
    }
}

