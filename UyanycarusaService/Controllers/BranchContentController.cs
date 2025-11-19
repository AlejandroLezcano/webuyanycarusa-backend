using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UyanycarusaService.Services;

namespace UyanycarusaService.Controllers
{
    /// <summary>
    /// Controlador para operaciones de contenido de sucursales
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/content")]
    [Authorize]
    [Tags("BranchContent")]
    public class BranchContentController : ControllerBase
    {
        private readonly IBranchContentService _branchContentService;
        private readonly ILogger<BranchContentController> _logger;

        public BranchContentController(IBranchContentService branchContentService, ILogger<BranchContentController> logger)
        {
            _branchContentService = branchContentService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene la lista de sucursales disponibles
        /// </summary>
        /// <param name="zipCode">Código postal opcional (5 dígitos)</param>
        /// <param name="limit">Límite de resultados opcional</param>
        /// <param name="branchType">Tipo de sucursal opcional (Physical, Mobile, All)</param>
        /// <returns>Respuesta con lista de sucursales del servicio externo</returns>
        /// <response code="200">Lista de sucursales obtenida correctamente</response>
        /// <response code="400">Solicitud inválida para el servicio externo</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="500">Error al consumir el servicio externo</response>
        [HttpGet("branches")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> GetBranches(
            [FromQuery] string? zipCode = null,
            [FromQuery] int? limit = null,
            [FromQuery] string? branchType = null)
        {
            try
            {
                var result = await _branchContentService.GetBranchesAsync(zipCode, limit, branchType);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de sucursales",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error inesperado al procesar la solicitud de sucursales",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Obtiene el detalle de una sucursal específica
        /// </summary>
        /// <param name="branchId">ID de la sucursal</param>
        /// <returns>Respuesta con detalle de la sucursal del servicio externo</returns>
        /// <response code="200">Detalle de sucursal obtenido correctamente</response>
        /// <response code="400">Solicitud inválida para el servicio externo</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="404">Sucursal no encontrada</response>
        /// <response code="500">Error al consumir el servicio externo</response>
        [HttpGet("branches/{branchId}")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> GetBranchDetail(int branchId)
        {
            try
            {
                var result = await _branchContentService.GetBranchDetailAsync(branchId);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de sucursales",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error inesperado al procesar la solicitud de detalle de sucursal",
                    detail = ex.Message
                });
            }
        }
    }
}

