using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UyanycarusaService.Services;
using UyanycarusaService.Dtos;

namespace UyanycarusaService.Controllers
{
    /// <summary>
    /// Controller responsible for attribution-related operations.
    /// All endpoints require a valid JWT.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [AllowAnonymous]
    [Tags("Attribution")]
    public class AttributionController : ControllerBase
    {
        private readonly IAttributionService _attributionService;
        private readonly ILogger<AttributionController> _logger;

        public AttributionController(
            IAttributionService attributionService,
            ILogger<AttributionController> logger)
        {
            _attributionService = attributionService;
            _logger = logger;
        }

        /// <summary>
        /// Creates or retrieves a visitor record.
        /// </summary>
        /// <param name="oldVisitorId">Optional previous visitor ID used for migration fallback</param>
        /// <returns>Visitor information from the external attribution service</returns>
        /// <response code="200">Visitor retrieved successfully</response>
        /// <response code="201">Visitor created successfully</response>
        /// <response code="401">Unauthorized – valid JWT required</response>
        /// <response code="500">External attribution service error</response>
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

                // External behavior:
                // - oldVisitorId present → "retrieve" → 200 OK
                // - no prior ID → "create" → 201 Created
                return oldVisitorId.HasValue
                    ? Ok(result)
                    : StatusCode(201, result);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "Error calling external attribution service (/visitor).");
                return StatusCode(500, new
                {
                    message = "Error communicating with external attribution service.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error processing visitor request.");
                return StatusCode(500, new
                {
                    message = "Unexpected server error while processing visitor request.",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Registers a visit event for a specific visitor.
        /// </summary>
        /// <param name="visitorId">The visitor's unique ID</param>
        /// <param name="model">Visit details payload</param>
        /// <returns>Visit event response from external attribution system</returns>
        /// <response code="201">Visit recorded successfully</response>
        /// <response code="400">Invalid request body</response>
        /// <response code="401">Unauthorized – valid JWT required</response>
        /// <response code="404">Visitor not found</response>
        /// <response code="500">External attribution service error</response>
        [HttpPost("visitor/{visitorId}/visit")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> CreateVisit(long visitorId, [FromBody] VisitRequestModel model)
        {
            try
            {
                var jsonElement = JsonSerializer.SerializeToElement(model);
                var result = await _attributionService.CreateVisitAsync(visitorId, jsonElement);

                return StatusCode(201, result);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, $"External attribution service communication error while creating visit for visitor {visitorId}.");
                return StatusCode(500, new
                {
                    message = "Error communicating with external attribution service.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error while creating visit for visitor {visitorId}.");
                return StatusCode(500, new
                {
                    message = "Unexpected server error while processing visit request.",
                    detail = ex.Message
                });
            }
        }
    }
}
