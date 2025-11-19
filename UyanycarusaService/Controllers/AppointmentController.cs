using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UyanycarusaService.Services;

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
        private readonly ILogger<AppointmentController> _logger;

        public AppointmentController(IAppointmentService appointmentService, ILogger<AppointmentController> logger)
        {
            _appointmentService = appointmentService;
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
        public async Task<ActionResult<JsonElement>> BookAppointment([FromBody] JsonElement model)
        {
            try
            {
                var result = await _appointmentService.BookAppointmentAsync(model);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                //_logger.LogWarning(ex, "Error de comunicación con el servicio externo /Appointment/book");
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de reserva de citas",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error inesperado al reservar la cita");
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
        public async Task<ActionResult<JsonElement>> RescheduleAppointment(int existingAppointmentId, [FromBody] JsonElement model)
        {
            try
            {
                var result = await _appointmentService.RescheduleAppointmentAsync(existingAppointmentId, model);
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

