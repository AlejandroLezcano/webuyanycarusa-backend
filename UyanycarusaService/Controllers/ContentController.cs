using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UyanycarusaService.Services;

namespace UyanycarusaService.Controllers
{
    /// <summary>
    /// Controlador para operaciones de contenido
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/content")]
    [Authorize]
    [Tags("Content")]
    public class ContentController : ControllerBase
    {
        private readonly IContentService _contentService;
        private readonly ILogger<ContentController> _logger;

        public ContentController(IContentService contentService, ILogger<ContentController> logger)
        {
            _contentService = contentService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene la lista de categorías de FAQs
        /// </summary>
        /// <returns>Respuesta con lista de categorías de FAQs del servicio externo</returns>
        /// <response code="200">Lista de FAQs obtenida correctamente</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="500">Error al consumir el servicio externo</response>
        [HttpGet("faqs")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> GetFaqs()
        {
            try
            {
                var result = await _contentService.GetFaqsAsync();
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de FAQs",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error inesperado al procesar la solicitud de FAQs",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Obtiene el detalle de FAQs por slug
        /// </summary>
        /// <param name="slug">Slug de la categoría de FAQ</param>
        /// <returns>Respuesta con FAQs detalladas del servicio externo</returns>
        /// <response code="200">FAQs obtenidas correctamente</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="404">FAQ no encontrada</response>
        /// <response code="500">Error al consumir el servicio externo</response>
        [HttpGet("faqs/{slug}")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> GetFaqsBySlug(string slug)
        {
            try
            {
                var result = await _contentService.GetFaqsBySlugAsync(slug);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de FAQs",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error inesperado al procesar la solicitud de FAQs",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Obtiene la lista de páginas de landing disponibles
        /// </summary>
        /// <returns>Respuesta con lista de páginas de landing del servicio externo</returns>
        /// <response code="200">Lista de landing pages obtenida correctamente</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="500">Error al consumir el servicio externo</response>
        [HttpGet("landing-page")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> GetLandingPages()
        {
            try
            {
                var result = await _contentService.GetLandingPagesAsync();
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de landing pages",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error inesperado al procesar la solicitud de landing pages",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Obtiene el contenido de una página de landing por slug
        /// </summary>
        /// <param name="slug">Slug de la página de landing</param>
        /// <returns>Respuesta con contenido de la página de landing del servicio externo</returns>
        /// <response code="200">Landing page obtenida correctamente</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="404">Landing page no encontrada</response>
        /// <response code="500">Error al consumir el servicio externo</response>
        [HttpGet("landing-page/{slug}")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> GetLandingPageBySlug(string slug)
        {
            try
            {
                var result = await _contentService.GetLandingPageBySlugAsync(slug);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de landing pages",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error inesperado al procesar la solicitud de landing page",
                    detail = ex.Message
                });
            }
        }

    }
}

