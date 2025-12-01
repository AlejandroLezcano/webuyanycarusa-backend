using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UyanycarusaService.Services;
using UyanycarusaService.Dtos;

namespace UyanycarusaService.Controllers
{
    /// <summary>
    /// Controlador para operaciones de citas (appointments)
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Authorize]
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
        /// Obtiene la disponibilidad de citas para un código postal y vehículo específico
        /// </summary>
        /// <param name="zipCode">Código postal (5 dígitos)</param>
        /// <param name="customerVehicleId">ID del vehículo del cliente</param>
        /// <returns>Respuesta de disponibilidad del servicio externo</returns>
        /// <response code="200">Disponibilidad obtenida correctamente</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="500">Error al consumir el servicio externo</response>
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
                //_logger.LogWarning(ex, "Error de comunicación con el servicio externo /Appointment/availability");
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de disponibilidad",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error inesperado al obtener disponibilidad de citas");
                return StatusCode(500, new
                {
                    message = "Error inesperado al procesar la solicitud de disponibilidad",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Reserva una cita para un vehículo
        /// </summary>
        /// <param name="model">Datos de la reserva de cita</param>
        /// <returns>Respuesta de reserva del servicio externo</returns>
        /// <response code="200">Cita reservada correctamente</response>
        /// <response code="400">Solicitud inválida para el servicio externo</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="500">Error al consumir el servicio externo</response>
        [HttpPost("book")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> BookAppointment([FromBody] AppointmentBookingModel model)
        {
            try
            {
                // Paso 1: Solicitar código OTP primero
                var otpRequest = new ScheduleOTPRequest
                {
                    CustomerVehicleId = model.CustomerVehicleId,
                    BranchId = model.BranchId,
                    TargetPhoneNumber = model.CustomerPhoneNumber
                };

                var otpOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                };
                var otpRequestJson = JsonSerializer.SerializeToElement(otpRequest, otpOptions);
                _logger.LogWarning("se va a consumir el servicio externo /scheduling/otp/request con el siguiente body: {Body}", otpRequestJson);
                var otpResponse = await _schedulingService.RequestOTPAsync(otpRequestJson);

                // Paso 2: Si el OTP fue exitoso, continuar con la reserva de la cita
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                };
                // Formatear la fecha como YYYY-MM-DD (solo fecha, sin hora)
                var modelCopy = new
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
                    smsOptIn = model.smsOptIn,
                    otpCode = model.OtpCode
                };
                var jsonElement = JsonSerializer.SerializeToElement(modelCopy, options);
                var result = await _appointmentService.BookAppointmentAsync(jsonElement);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "Error de comunicación con el servicio externo durante el proceso de reserva de cita");

                // Verificar si el error es del OTP o del booking
                if (ex.Message.Contains("OTP") || ex.Message.Contains("otp"))
                {
                    return StatusCode(500, new
                    {
                        message = "Error al solicitar código OTP. No se pudo continuar con la reserva de la cita",
                        detail = ex.Message
                    });
                }

                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de reserva de citas",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al reservar la cita");
                return StatusCode(500, new
                {
                    message = "Error inesperado al procesar la solicitud de reserva",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Reprograma una cita existente
        /// </summary>
        /// <param name="existingAppointmentId">ID de la cita existente</param>
        /// <param name="model">Datos de la nueva reserva de cita</param>
        /// <returns>Respuesta de reprogramación del servicio externo</returns>
        /// <response code="200">Cita reprogramada correctamente</response>
        /// <response code="400">Solicitud inválida para el servicio externo</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="500">Error al consumir el servicio externo</response>
        [HttpPost("{existingAppointmentId}/reschedule")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> RescheduleAppointment(int existingAppointmentId, [FromBody] AppointmentBookingModel model)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                };
                // Formatear la fecha como YYYY-MM-DD (solo fecha, sin hora)
                var modelCopy = new
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
                    smsOptIn = model.smsOptIn,
                    otpCode = model.OtpCode
                };
                var jsonElement = JsonSerializer.SerializeToElement(modelCopy, options);
                var result = await _appointmentService.RescheduleAppointmentAsync(existingAppointmentId, jsonElement);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                //_logger.LogWarning(ex, "Error de comunicación con el servicio externo /Appointment/reschedule");
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de reprogramación de citas",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error inesperado al reprogramar la cita");
                return StatusCode(500, new
                {
                    message = "Error inesperado al procesar la solicitud de reprogramación",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Cancela una cita existente
        /// </summary>
        /// <param name="customerVehicleId">ID del vehículo del cliente</param>
        /// <param name="phoneNumber">Número de teléfono del cliente</param>
        /// <returns>Respuesta de cancelación del servicio externo</returns>
        /// <response code="200">Cita cancelada correctamente</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="404">Cita no encontrada</response>
        /// <response code="500">Error al consumir el servicio externo</response>
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
                //_logger.LogWarning(ex, "Error de comunicación con el servicio externo /Appointment/cancel");
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de cancelación de citas",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error inesperado al cancelar la cita");
                return StatusCode(500, new
                {
                    message = "Error inesperado al procesar la solicitud de cancelación",
                    detail = ex.Message
                });
            }
        }
    }
}

