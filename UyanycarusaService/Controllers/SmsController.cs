using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UyanycarusaService.Dtos;
using UyanycarusaService.Services;

namespace UyanycarusaService.Controllers
{
    /// <summary>
    /// Controller for SMS operations.
    /// All endpoints require a valid JWT token.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Authorize]
    public class SmsController : ControllerBase
    {
        private readonly ISmsService _smsService;
        private readonly ILogger<SmsController> _logger;

        public SmsController(ISmsService smsService, ILogger<SmsController> logger)
        {
            _smsService = smsService;
            _logger = logger;
        }

        /// <summary>
        /// Sends an SMS using the external SMS provider.
        /// </summary>
        /// <param name="request">SMS request payload (customerVehicleId, recipient, message).</param>
        /// <returns>Response from the external SMS service.</returns>
        /// <response code="200">SMS sent successfully.</response>
        /// <response code="400">Invalid request. SMS data is missing or incorrect.</response>
        /// <response code="401">Unauthorized. A valid JWT token is required.</response>
        /// <response code="429">Too many requests. Rate limit exceeded.</response>
        /// <response code="500">Error communicating with external SMS provider.</response>
        [HttpPost("send")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> SendSms([FromBody] SmsSendModel request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new { message = "Request body is required." });
                }

                if (request.CustomerVehicleId <= 0)
                {
                    return BadRequest(new { message = "CustomerVehicleId must be greater than 0." });
                }

                if (string.IsNullOrWhiteSpace(request.Recipient))
                {
                    return BadRequest(new { message = "Recipient is required." });
                }

                if (string.IsNullOrWhiteSpace(request.Message))
                {
                    return BadRequest(new { message = "Message is required." });
                }

                var result = await _smsService.SendSmsAsync(request);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "Error communicating with external SMS provider.");

                return StatusCode(500, new
                {
                    message = "Error communicating with external SMS provider.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error sending SMS.");

                return StatusCode(500, new
                {
                    message = "Unexpected error processing SMS request.",
                    detail = ex.Message
                });
            }
        }
    }
}
