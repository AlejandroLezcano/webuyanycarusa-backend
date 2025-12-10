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
    /// Controller for authentication and JWT token generation
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
        /// Authenticates a user and generates a JWT token
        /// </summary>
        /// <param name="request">Authentication credentials</param>
        /// <returns>JWT token and expiration date</returns>
        /// <response code="200">Successful authentication. Returns the JWT token</response>
        /// <response code="400">Invalid request. Credentials are incorrect</response>
        /// <response code="429">Too many requests. Rate limit exceeded</response>
        /// <remarks>
        /// Request example:
        ///
        ///     POST /api/v1/auth/login
        ///     {
        ///         "username": "admin",
        ///         "password": "password123"
        ///     }
        ///
        /// **Note:** In a production environment, credentials should be validated against a database or authentication service.
        /// This is a simplified example that accepts any credentials.
        /// </remarks>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Username and Password are required" });
            }

            // In production, validate against database or authentication service
            // For now, we accept any credentials for demonstration
            // TODO: Implement real credential validation

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

            return Ok(new LoginResponse
            {
                Token = tokenString,
                ExpiresAt = expires
            });
        }
    }
}

