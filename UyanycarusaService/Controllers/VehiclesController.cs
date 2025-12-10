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
    // OJO: sin [Authorize] aquí → todo el controller es público
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
        [HttpGet("years")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<int>), StatusCodes.Status200OK)]
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
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo",
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

        /// <summary>
        /// Obtiene la lista de marcas para un año
        /// </summary>
        [HttpGet("makes/{year}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
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
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo",
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

        /// <summary>
        /// Obtiene la lista de modelos para un año y marca
        /// </summary>
        [HttpGet("models/{year}/{make}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
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
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo",
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

        /// <summary>
        /// Obtiene trims (versiones) para un año, marca y modelo
        /// </summary>
        [HttpGet("trims/{year}/{make}/{model}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
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
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo",
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

        /// <summary>
        /// Proxy para traer imágenes externas
        /// </summary>
        [HttpGet("image")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
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
