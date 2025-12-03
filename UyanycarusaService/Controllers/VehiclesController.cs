using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using UyanycarusaService.Services;
using System.IO;

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

        /// <summary>
        /// Obtiene la lista de trims (versiones/equipamientos) disponibles para un año, marca y modelo específicos desde el servicio externo
        /// </summary>
        /// <param name="year">Año del vehículo</param>
        /// <param name="make">Marca del vehículo</param>
        /// <param name="model">Modelo del vehículo</param>
        /// <returns>Lista de trims disponibles (array de objetos con bodystyle, series, imageUrl)</returns>
        /// <response code="200">Retorna la lista de trims exitosamente</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="429">Demasiadas solicitudes. Se ha excedido el límite de rate limiting</response>
        /// <response code="500">Error al consumir el servicio externo</response>
        [HttpGet("trims/{year}/{make}/{model}")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> GetTrims(int year, string make, string model)
        {
            try
            {
                var trims = await _vehiclesService.GetTrimsAsync(year, make, model);
                return Ok(trims);
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
        /// Obtiene una imagen desde una URL externa
        /// </summary>
        /// <param name="url">URL de la imagen a obtener</param>
        /// <returns>Archivo de imagen</returns>
        /// <response code="200">Retorna la imagen exitosamente</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="500">Error al obtener la imagen desde la URL</response>
        [HttpGet("image")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
       public async Task<IActionResult> GetImage([FromQuery] string url)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(url))
                {
                    return BadRequest(new { message = "La URL es requerida" });
                }

                var (imageContent, contentType) = await _vehiclesService.GetImageAsync(url);

                var stream = new MemoryStream(imageContent);

                return new FileStreamResult(stream, contentType)
                {
                    EnableRangeProcessing = true
                };
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo para obtener la imagen",
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

