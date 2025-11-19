using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UyanycarusaService.Services;

namespace UyanycarusaService.Controllers
{
    /// <summary>
    /// Controlador para operaciones de atribución
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Authorize]
    [Tags("Attribution")]
    public class AttributionController : ControllerBase
    {
        private readonly IAttributionService _attributionService;
        private readonly ILogger<AttributionController> _logger;

        public AttributionController(IAttributionService attributionService, ILogger<AttributionController> logger)
        {
            _attributionService = attributionService;
            _logger = logger;
        }

        /// <summary>
        /// Crea o obtiene un visitor
        /// </summary>
        /// <param name="oldVisitorId">ID del visitor anterior opcional (para migración)</param>
        /// <returns>Respuesta con información del visitor del servicio externo</returns>
        /// <response code="200">Visitor obtenido correctamente</response>
        /// <response code="201">Visitor creado correctamente</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="500">Error al consumir el servicio externo</response>
        [HttpPost("visitor")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> CreateOrGetVisitor([FromQuery] long? oldVisitorId = null)
        {
            try
            {
                var result = await _attributionService.CreateOrGetVisitorAsync(oldVisitorId);

                // El servicio externo puede retornar 200 (obtenido) o 201 (creado)
                // Simulamos el comportamiento basado en si tiene oldVisitorId
                if (oldVisitorId.HasValue)
                {
                    return Ok(result);
                }
                else
                {
                    return StatusCode(201, result);
                }
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de atribución",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error inesperado al procesar la solicitud de visitor",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Registra una visita para un visitor
        /// </summary>
        /// <param name="visitorId">ID del visitor</param>
        /// <param name="model">Datos de la visita</param>
        /// <returns>Respuesta con información de la visita del servicio externo</returns>
        /// <response code="201">Visita registrada correctamente</response>
        /// <response code="400">Solicitud inválida para el servicio externo</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="404">Visitor no encontrado</response>
        /// <response code="500">Error al consumir el servicio externo</response>
        [HttpPost("visitor/{visitorId}/visit")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> CreateVisit(long visitorId, [FromBody] JsonElement model)
        {
            try
            {
                var result = await _attributionService.CreateVisitAsync(visitorId, model);
                return StatusCode(201, result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de atribución",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error inesperado al procesar la solicitud de visita",
                    detail = ex.Message
                });
            }
        }
    }
}

