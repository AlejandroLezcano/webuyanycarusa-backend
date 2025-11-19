using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UyanycarusaService.Services;

namespace UyanycarusaService.Controllers
{
    /// <summary>
    /// Controlador para operaciones de valuación de vehículos
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Authorize]
    public class ValuationController : ControllerBase
    {
        private readonly IValuationService _valuationService;
        private readonly ILogger<ValuationController> _logger;

        public ValuationController(IValuationService valuationService, ILogger<ValuationController> logger)
        {
            _valuationService = valuationService;
            _logger = logger;
        }

        /// <summary>
        /// Realiza una valuación básica del vehículo
        /// </summary>
        /// <param name="model">Payload enviado al endpoint externo /Valuation</param>
        /// <returns>Respuesta de valuación del servicio externo</returns>
        /// <response code="200">Valuación realizada correctamente</response>
        /// <response code="400">Solicitud inválida para el servicio externo</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="500">Error al consumir el servicio externo</response>
        [HttpPost]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> CreateValuation([FromBody] JsonElement model)
        {
            try
            {
                var result = await _valuationService.CreateValuationAsync(model);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                // _logger.LogWarning(ex, "Error de comunicación con el servicio externo /Valuation");
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de valuación",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "Error inesperado al realizar la valuación");
                return StatusCode(500, new
                {
                    message = "Error inesperado al procesar la solicitud de valuación",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Realiza una valuación del vehículo incluyendo daños
        /// </summary>
        /// <param name="model">Payload enviado al endpoint externo /Valuation/with-damage</param>
        /// <returns>Respuesta de valuación del servicio externo</returns>
        /// <response code="200">Valuación realizada correctamente</response>
        /// <response code="400">Solicitud inválida para el servicio externo</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="500">Error al consumir el servicio externo</response>
        [HttpPost("with-damage")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> CreateValuationWithDamage([FromBody] JsonElement model)
        {
            try
            {
                var result = await _valuationService.CreateValuationWithDamageAsync(model);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                // _logger.LogWarning(ex, "Error de comunicación con el servicio externo /Valuation/with-damage");
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de valuación con daños",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "Error inesperado al realizar la valuación con daños");
                return StatusCode(500, new
                {
                    message = "Error inesperado al procesar la solicitud de valuación con daños",
                    detail = ex.Message
                });
            }
        }
    }
}


