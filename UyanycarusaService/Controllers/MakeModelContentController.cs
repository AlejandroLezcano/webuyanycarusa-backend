using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UyanycarusaService.Services;

namespace UyanycarusaService.Controllers
{
    /// <summary>
    /// Controller for make/model content operations.
    /// All endpoints require a valid JWT token.
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

        public MakeModelContentController(
            IMakeModelContentService makeModelContentService,
            ILogger<MakeModelContentController> logger)
        {
            _makeModelContentService = makeModelContentService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves content for a specific vehicle make.
        /// </summary>
        /// <param name="make">Vehicle make name.</param>
        /// <returns>Content payload from the external service.</returns>
        /// <response code="200">Make content retrieved successfully.</response>
        /// <response code="401">Unauthorized. A valid JWT token is required.</response>
        /// <response code="404">Make not found.</response>
        /// <response code="500">Error communicating with external service.</response>
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
                _logger.LogWarning(ex, "External service error while fetching make content ({Make})", make);
                return StatusCode(500, new
                {
                    message = "Error communicating with the external make content service.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while processing make content request ({Make})", make);
                return StatusCode(500, new
                {
                    message = "Unexpected error processing make content request.",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Retrieves content for a specific make and model.
        /// </summary>
        /// <param name="make">Vehicle make name.</param>
        /// <param name="model">Vehicle model name.</param>
        /// <returns>Content payload for the make and model.</returns>
        /// <response code="200">Make/model content retrieved successfully.</response>
        /// <response code="401">Unauthorized. A valid JWT token is required.</response>
        /// <response code="404">Make or model not found.</response>
        /// <response code="500">Error communicating with external service.</response>
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
                _logger.LogWarning(ex, "External service error while fetching make/model content ({Make}/{Model})", make, model);
                return StatusCode(500, new
                {
                    message = "Error communicating with the external make/model content service.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while processing make/model content request ({Make}/{Model})", make, model);
                return StatusCode(500, new
                {
                    message = "Unexpected error processing make/model content request.",
                    detail = ex.Message
                });
            }
        }
    }
}
