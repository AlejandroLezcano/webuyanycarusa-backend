using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UyanycarusaService.Services;
using UyanycarusaService.Dtos;

namespace UyanycarusaService.Controllers
{
    /// <summary>
    /// Controller for appointment operations
    /// Controller for all appointment operations.
    /// Requires a valid JWT for every request.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [AllowAnonymous] // Allow guest booking
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ISchedulingService _schedulingService;
        private readonly ILogger<AppointmentController> _logger;

        public AppointmentController(
            IAppointmentService appointmentService,
            ISchedulingService schedulingService,
            ILogger<AppointmentController> logger)
        {
            _appointmentService = appointmentService;
            _schedulingService = schedulingService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves appointment availability for a specific ZIP code and customer vehicle ID.
        /// </summary>
        /// <param name="zipCode">ZIP code (5 digits)</param>
        /// <param name="customerVehicleId">Customer vehicle record ID</param>
        /// <returns>Availability response from external scheduling service</returns>
        /// <response code="200">Availability retrieved successfully.</response>
        /// <response code="401">Unauthorized. Valid JWT token required.</response>
        /// <response code="500">Error consuming external service.</response>
        [HttpGet("availability/{zipCode}/{customerVehicleId}")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> GetAvailability(string zipCode, int customerVehicleId)
        {
            try
            {
                var result = await _appointmentService.GetAvailabilityAsync(zipCode, customerVehicleId);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "Error communicating with external availability service.");
                return StatusCode(500, new
                {
                    message = "Error communicating with external availability service.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching appointment availability.");
                return StatusCode(500, new
                {
                    message = "Unexpected server error while processing availability request.",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Books a new appointment for a customer.
        /// </summary>
        /// <param name="model">Appointment booking details</param>
        /// <returns>External service booking response</returns>
        [HttpPost("book")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> BookAppointment([FromBody] AppointmentBookingModel model)
        {
            try
            {
                var jsonModel = PrepareAppointmentPayload(model);

                var result = await _appointmentService.BookAppointmentAsync(jsonModel);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "External appointment booking service communication failure.");
                return StatusCode(500, new
                {
                    message = "Error communicating with external appointment booking service.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while booking appointment.");
                return StatusCode(500, new
                {
                    message = "Unexpected server error while processing booking request.",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Reschedules an existing appointment.
        /// </summary>
        /// <param name="existingAppointmentId">Existing appointment ID</param>
        /// <param name="model">New appointment details</param>
        /// <returns>External service rescheduling response</returns>
        [HttpPost("{existingAppointmentId}/reschedule")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> RescheduleAppointment(int existingAppointmentId, [FromBody] AppointmentBookingModel model)
        {
            try
            {
                var jsonModel = PrepareAppointmentPayload(model);

                var result = await _appointmentService.RescheduleAppointmentAsync(existingAppointmentId, jsonModel);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "External appointment rescheduling service failure.");
                return StatusCode(500, new
                {
                    message = "Error communicating with external appointment rescheduling service.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while rescheduling appointment.");
                return StatusCode(500, new
                {
                    message = "Unexpected server error while processing rescheduling request.",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Cancels an existing appointment for a specific vehicle using phone validation.
        /// </summary>
        /// <param name="customerVehicleId">Customer vehicle ID</param>
        /// <param name="phoneNumber">Customer phone number</param>
        /// <returns>External cancellation service response</returns>
        [HttpPost("cancel/{customerVehicleId}/{phoneNumber}")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> CancelAppointment(int customerVehicleId, long phoneNumber)
        {
            try
            {
                var result = await _appointmentService.CancelAppointmentAsync(customerVehicleId, phoneNumber);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "External cancellation service communication error.");
                return StatusCode(500, new
                {
                    message = "Error communicating with external appointment cancellation service.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while cancelling appointment.");
                return StatusCode(500, new
                {
                    message = "Unexpected server error while processing cancellation request.",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Normalizes the appointment booking model into a clean JSON payload with yyyy-MM-dd date formatting.
        /// </summary>
        private static JsonElement PrepareAppointmentPayload(AppointmentBookingModel model)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            var payload = new
            {
                customerVehicleId = model.CustomerVehicleId,
                branchId = model.BranchId,
                date = model.Date.ToString("yyyy-MM-dd"),
                timeSlotId = model.TimeSlotId,
                customerPhoneNumber = model.CustomerPhoneNumber,
                customerFirstName = model.CustomerFirstName,
                customerLastName = model.CustomerLastName,
                email = model.Email,
                address1 = model.Address1,
                address2 = model.Address2,
                city = model.City,
                visitId = model.VisitId,
                smsOptIn = model.SmsOptIn,
                otpCode = model.OtpCode
            };

            return JsonSerializer.SerializeToElement(payload, options);
        }
    }
}
