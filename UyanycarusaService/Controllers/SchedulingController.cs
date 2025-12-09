using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UyanycarusaService.Services;
using UyanycarusaService.Dtos;

namespace UyanycarusaService.Controllers
{
    /// <summary>
    /// Controller for Scheduling (OTP) operations.
    /// All endpoints require a valid JWT token.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/scheduling/otp")]
    [Authorize]
    [Tags("OneTimePassword")]
    public class SchedulingController : ControllerBase
    {
        private readonly ISchedulingService _schedulingService;
        private readonly ILogger<SchedulingController> _logger;

        public SchedulingController(ISchedulingService schedulingService, ILogger<SchedulingController> logger)
        {
            _schedulingService = schedulingService;
            _logger = logger;
        }

        /// <summary>
        /// Requests an OTP code for scheduling.
        /// </summary>
        /// <param name="model">OTP request payload.</param>
        /// <returns>OTP response from the external service.</returns>
        /// <response code="202">OTP request accepted.</response>
        /// <response code="400">Invalid request sent to the external service.</response>
        /// <response code="401">Unauthorized. A valid JWT token is required.</response>
        /// <response code="404">Resource not found by external provider.</response>
        /// <response code="429">Too many requests (rate limited).</response>
        /// <response code="500">Unexpected error communicating with external OTP provider.</response>
        [HttpPost("request")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> RequestOTP([FromBody] ScheduleOTPRequest model)
        {
            try
            {
                var jsonElement = JsonSerializer.SerializeToElement(model);
                var result = await _schedulingService.RequestOTPAsync(jsonElement);

                return StatusCode(202, result);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "Error while communicating with external OTP service.");
                return StatusCode(500, new
                {
                    message = "Error communicating with external OTP service.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while processing OTP request.");
                return StatusCode(500, new
                {
                    message = "Unexpected error processing OTP request.",
                    detail = ex.Message
                });
            }
        }
    }
}
