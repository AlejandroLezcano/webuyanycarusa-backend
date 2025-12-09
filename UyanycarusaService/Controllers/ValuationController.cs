using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UyanycarusaService.Services;
using UyanycarusaService.Dtos;

namespace UyanycarusaService.Controllers
{
    /// <summary>
    /// Controller for vehicle valuation operations.
    /// All endpoints require a valid JWT token.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class ValuationController : ControllerBase
    {
        private readonly IValuationService _valuationService;
        private readonly ILogger<ValuationController> _logger;

        public ValuationController(IValuationService valuationService, ILogger<ValuationController> logger)
        {
            _valuationService = valuationService;
            _logger = logger;
        }

        /// <summary>
        /// Performs a basic vehicle valuation.
        /// </summary>
        /// <param name="model">Payload sent to the external /Valuation endpoint.</param>
        /// <returns>Valuation response from the external service.</returns>
        /// <response code="200">Valuation completed successfully.</response>
        /// <response code="400">Invalid request for external service.</response>
        /// <response code="401">Unauthorized. Valid JWT token required.</response>
        /// <response code="500">Error communicating with external valuation provider.</response>
        [HttpPost]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> CreateValuation([FromBody] ValuationModel model)
        {
            try
            {
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var jsonElement = JsonSerializer.SerializeToElement(model, options);
                var result = await _valuationService.CreateValuationAsync(jsonElement);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "Error communicating with external valuation service.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Unexpected error processing valuation request.",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Performs a vehicle valuation including damage information.
        /// </summary>
        /// <param name="model">Payload sent to the external /Valuation/with-damage endpoint.</param>
        /// <returns>Valuation response from the external service.</returns>
        /// <response code="200">Valuation completed successfully.</response>
        /// <response code="400">Invalid request for external service.</response>
        /// <response code="401">Unauthorized. Valid JWT token required.</response>
        /// <response code="500">Error communicating with external valuation provider.</response>
        [HttpPost("with-damage")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> CreateValuationWithDamage([FromBody] ValuationWithDamageModel model)
        {
            try
            {
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var jsonElement = JsonSerializer.SerializeToElement(model, options);
                var result = await _valuationService.CreateValuationWithDamageAsync(jsonElement);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "Error communicating with external valuation-with-damage service.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Unexpected error processing valuation-with-damage request.",
                    detail = ex.Message
                });
            }
        }
    }
}
