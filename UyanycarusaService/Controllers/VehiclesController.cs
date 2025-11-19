using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UyanycarusaService.Services;

namespace UyanycarusaService.Controllers
{
    /// <summary>
    /// Controlador para operaciones relacionadas con vehículos
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Authorize]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehiclesService _vehiclesService;
        private readonly ILogger<VehiclesController> _logger;

        public VehiclesController(IVehiclesService vehiclesService, ILogger<VehiclesController> logger)
        {
            _vehiclesService = vehiclesService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene la lista de años disponibles de vehículos desde el servicio externo
        /// </summary>
        /// <returns>Lista de años disponibles (enteros)</returns>
        /// <response code="200">Retorna la lista de años exitosamente</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="429">Demasiadas solicitudes. Se ha excedido el límite de rate limiting</response>
        /// <response code="500">Error al consumir el servicio externo</response>
        [HttpGet("years")]
        [ProducesResponseType(typeof(List<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<int>>> GetYears()
        {
            try
            {
                var years = await _vehiclesService.GetYearsAsync();
                return Ok(years);
            }
            catch (HttpRequestException ex)
            {
                // _logger.LogError(ex, "Error de comunicación con el servicio externo");
                return StatusCode(500, new {
                    message = "Error al comunicarse con el servicio externo",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "Error inesperado al obtener años de vehículos");
                return StatusCode(500, new {
                    message = "Error inesperado al procesar la solicitud",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Obtiene la lista de marcas disponibles para un año específico desde el servicio externo
        /// </summary>
        /// <param name="year">Año del vehículo</param>
        /// <returns>Lista de marcas disponibles (strings)</returns>
        /// <response code="200">Retorna la lista de marcas exitosamente</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="429">Demasiadas solicitudes. Se ha excedido el límite de rate limiting</response>
        /// <response code="500">Error al consumir el servicio externo</response>
        [HttpGet("makes/{year}")]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<string>>> GetMakes(int year)
        {
            try
            {
                var makes = await _vehiclesService.GetMakesAsync(year);
                return Ok(makes);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new {
                    message = "Error al comunicarse con el servicio externo",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {
                    message = "Error inesperado al procesar la solicitud",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Obtiene la lista de modelos disponibles para un año y marca específicos desde el servicio externo
        /// </summary>
        /// <param name="year">Año del vehículo</param>
        /// <param name="make">Marca del vehículo</param>
        /// <returns>Lista de modelos disponibles (strings)</returns>
        /// <response code="200">Retorna la lista de modelos exitosamente</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="429">Demasiadas solicitudes. Se ha excedido el límite de rate limiting</response>
        /// <response code="500">Error al consumir el servicio externo</response>
        [HttpGet("models/{year}/{make}")]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<string>>> GetModels(int year, string make)
        {
            try
            {
                var models = await _vehiclesService.GetModelsAsync(year, make);
                return Ok(models);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new {
                    message = "Error al comunicarse con el servicio externo",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {
                    message = "Error inesperado al procesar la solicitud",
                    detail = ex.Message
                });
            }
        }
    }
}

