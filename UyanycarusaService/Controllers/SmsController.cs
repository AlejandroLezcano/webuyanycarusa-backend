using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UyanycarusaService.Dtos;
using UyanycarusaService.Services;

namespace UyanycarusaService.Controllers
{
    /// <summary>
    /// Controlador para operaciones relacionadas con SMS
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Authorize]
    public class SmsController : ControllerBase
    {
        private readonly ISmsService _smsService;
        private readonly ILogger<SmsController> _logger;

        public SmsController(ISmsService smsService, ILogger<SmsController> logger)
        {
            _smsService = smsService;
            _logger = logger;
        }

        /// <summary>
        /// Envía un SMS a través del servicio externo
        /// </summary>
        /// <param name="request">Datos del SMS a enviar (customerVehicleId, recipient, message)</param>
        /// <returns>Respuesta del servicio externo</returns>
        /// <response code="200">SMS enviado exitosamente</response>
        /// <response code="400">Solicitud inválida. Los datos del SMS son incorrectos</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="429">Demasiadas solicitudes. Se ha excedido el límite de rate limiting</response>
        /// <response code="500">Error al comunicarse con el servicio externo</response>
        [HttpPost("send")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> SendSms([FromBody] SmsSendModel request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new { message = "El cuerpo de la solicitud es requerido" });
                }

                if (request.CustomerVehicleId <= 0)
                {
                    return BadRequest(new { message = "CustomerVehicleId debe ser mayor a 0" });
                }

                if (string.IsNullOrWhiteSpace(request.Recipient))
                {
                    return BadRequest(new { message = "Recipient es requerido" });
                }

                if (string.IsNullOrWhiteSpace(request.Message))
                {
                    return BadRequest(new { message = "Message es requerido" });
                }

                var result = await _smsService.SendSmsAsync(request);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo para enviar SMS",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error inesperado al procesar la solicitud",
                    detail = ex.Message
                });
            }
        }
    }
}

