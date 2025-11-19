using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UyanycarusaService.Services;

namespace UyanycarusaService.Controllers
{
    /// <summary>
    /// Controlador para operaciones de contenido de marca y modelo
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/content")]
    [Authorize]
    [Tags("MakeModelContent")]
    public class MakeModelContentController : ControllerBase
    {
        private readonly IMakeModelContentService _makeModelContentService;
        private readonly ILogger<MakeModelContentController> _logger;

        public MakeModelContentController(IMakeModelContentService makeModelContentService, ILogger<MakeModelContentController> logger)
        {
            _makeModelContentService = makeModelContentService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene el contenido de una marca específica
        /// </summary>
        /// <param name="make">Nombre de la marca</param>
        /// <returns>Respuesta con contenido de la marca del servicio externo</returns>
        /// <response code="200">Contenido de marca obtenido correctamente</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="404">Marca no encontrada</response>
        /// <response code="500">Error al consumir el servicio externo</response>
        [HttpGet("make-model/{make}")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> GetMakeContent(string make)
        {
            try
            {
                var result = await _makeModelContentService.GetMakeContentAsync(make);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de contenido de marca",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error inesperado al procesar la solicitud de contenido de marca",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Obtiene el contenido de una marca y modelo específicos
        /// </summary>
        /// <param name="make">Nombre de la marca</param>
        /// <param name="model">Nombre del modelo</param>
        /// <returns>Respuesta con contenido de la marca y modelo del servicio externo</returns>
        /// <response code="200">Contenido de marca y modelo obtenido correctamente</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="404">Marca o modelo no encontrado</response>
        /// <response code="500">Error al consumir el servicio externo</response>
        [HttpGet("make-model/{make}/{model}")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> GetMakeModelContent(string make, string model)
        {
            try
            {
                var result = await _makeModelContentService.GetMakeModelContentAsync(make, model);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de contenido de marca y modelo",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error inesperado al procesar la solicitud de contenido de marca y modelo",
                    detail = ex.Message
                });
            }
        }
    }
}

