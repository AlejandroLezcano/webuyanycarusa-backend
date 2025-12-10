using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UyanycarusaService.Services;
using UyanycarusaService.Dtos;

namespace UyanycarusaService.Controllers
{
    /// <summary>
    /// Controlador para operaciones de Scheduling (OTP)
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/scheduling/otp")]
    [Authorize]
    [Tags("OneTimePassword")]
    public class SchedulingController : ControllerBase
    {
        private readonly ISchedulingService _schedulingService;
        private readonly ILogger<SchedulingController> _logger;

        public SchedulingController(ISchedulingService schedulingService, ILogger<SchedulingController> logger)
        {
            _schedulingService = schedulingService;
            _logger = logger;
        }

        /// <summary>
        /// Requests an OTP code for scheduling
        /// </summary>
        /// <param name="model">OTP request data</param>
        /// <returns>OTP request response from external service</returns>
        /// <response code="202">OTP request accepted</response>
        /// <response code="400">Invalid request for external service</response>
        /// <response code="401">Unauthorized. Valid JWT token required</response>
        /// <response code="404">Recurso no encontrado</response>
        /// <response code="429">Demasiadas solicitudes</response>
        /// <response code="500">Error al consumir el servicio externo</response>
        [HttpPost("request")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> RequestOTP([FromBody] ScheduleOTPRequest model)
        {
            try
            {
                var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var jsonElement = JsonSerializer.SerializeToElement(model, jsonOptions);
                var result = await _schedulingService.RequestOTPAsync(jsonElement);
                return StatusCode(202, result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de OTP",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error inesperado al procesar la solicitud de OTP",
                    detail = ex.Message
                });
            }
        }
    }
}

