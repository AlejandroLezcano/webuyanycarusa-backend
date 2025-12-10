using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UyanycarusaService.Services;

namespace UyanycarusaService.Controllers
{
    /// <summary>
    /// Controller for all vehicle-related operations.
    /// Requires valid JWT authentication for all endpoints.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [AllowAnonymous]
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
        /// Retrieves the available vehicle years from the external service.
        /// </summary>
        /// <returns>List of available years.</returns>
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
                return StatusCode(500, new
                {
                    message = "Error communicating with the external vehicle service.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Unexpected error processing the request.",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Retrieves available makes for a specific year.
        /// </summary>
        /// <param name="year">Vehicle year.</param>
        /// <returns>List of available makes.</returns>
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
                return StatusCode(500, new
                {
                    message = "Error communicating with the external vehicle service.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Unexpected error processing the request.",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Retrieves available models for a given year and make.
        /// </summary>
        /// <param name="year">Vehicle year.</param>
        /// <param name="make">Vehicle make.</param>
        /// <returns>List of available models.</returns>
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
                return StatusCode(500, new
                {
                    message = "Error communicating with the external vehicle service.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Unexpected error processing the request.",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Retrieves available trim information for a given year, make, and model.
        /// </summary>
        /// <param name="year">Vehicle year.</param>
        /// <param name="make">Vehicle make.</param>
        /// <param name="model">Vehicle model (query parameter to handle special characters).</param>
        /// <returns>Trim list (body style, series, images).</returns>
        [HttpGet("trims/{year}/{make}")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> GetTrims(int year, string make, [FromQuery] string model)
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
                    message = "Error communicating with the external vehicle service.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Unexpected error processing the request.",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Retrieves an image from an external URL.
        /// </summary>
        /// <param name="url">External image URL.</param>
        /// <returns>Binary image stream.</returns>
        [HttpGet("image")]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetImage([FromQuery] string url)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(url))
                {
                    return BadRequest(new { message = "Image URL is required." });
                }

                var (imageBytes, contentType) = await _vehiclesService.GetImageAsync(url);

                var stream = new MemoryStream(imageBytes);

                return new FileStreamResult(stream, contentType)
                {
                    EnableRangeProcessing = true
                };
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "Error fetching image from external provider.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Unexpected error processing the image request.",
                    detail = ex.Message
                });
            }
        }
    }
}
