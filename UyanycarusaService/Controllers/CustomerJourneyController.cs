using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UyanycarusaService.Dtos;
using UyanycarusaService.Services;

namespace UyanycarusaService.Controllers
{
    /// <summary>
    /// Controller for customer journey operations
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/customer-journey")]
    // IMPORTANTE: no [Authorize] a nivel de clase
    public class CustomerJourneyController : ControllerBase
    {
        private readonly ICustomerJourneyService _customerJourneyService;
        private readonly ILogger<CustomerJourneyController> _logger;

        public CustomerJourneyController(
            ICustomerJourneyService customerJourneyService,
            ILogger<CustomerJourneyController> logger)
        {
            _customerJourneyService = customerJourneyService;
            _logger = logger;
        }

        /// <summary>
        /// Gets a customer journey by its ID (UUID)
        /// GET /api/customer-journey/{id:guid}
        /// </summary>
        [HttpGet("{id:guid}")]
        [AllowAnonymous] // público
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> GetJourneyById(Guid id)
        {
            try
            {
                var result = await _customerJourneyService.GetJourneyByIdAsync(id.ToString());
                if (result.ValueKind == JsonValueKind.Undefined ||
                    result.ValueKind == JsonValueKind.Null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "Error communicating with external customer journey service",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Unexpected error processing request",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Gets a customer journey by visitId (integer)
        /// GET /api/customer-journey/{visitId:int}
        /// </summary>
        [HttpGet("{visitId:int}")]
        [AllowAnonymous] // público (lo que llama tu front: /api/customer-journey/260141965)
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> GetJourneyByVisitId(int visitId)
        {
            try
            {
                var result = await _customerJourneyService.GetJourneyByVisitIdAsync(visitId);
                if (result.ValueKind == JsonValueKind.Undefined ||
                    result.ValueKind == JsonValueKind.Null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de customer journey",
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
        /// Inicia un customer journey usando Year, Make, Model
        /// </summary>
        [HttpPost]
        [Authorize] // protegido
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> CreateJourneyWithYMM(
            [FromBody] CustomerJourneyStep1YMMModel model)
        {
            try
            {
                var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var jsonElement = JsonSerializer.SerializeToElement(model, jsonOptions);
                var result = await _customerJourneyService.CreateJourneyWithYMMAsync(jsonElement);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de customer journey",
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
        /// Inicia un customer journey usando VIN
        /// </summary>
        [HttpPost("vin")]
        [Authorize]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> CreateJourneyWithVIN(
            [FromBody] CustomerJourneyStep1VINModel model)
        {
            try
            {
                var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var jsonElement = JsonSerializer.SerializeToElement(model, jsonOptions);
                var result = await _customerJourneyService.CreateJourneyWithVINAsync(jsonElement);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de customer journey",
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
        /// Inicia un customer journey usando License Plate
        /// </summary>
        [HttpPost("plate")]
        [Authorize]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> CreateJourneyWithPlate(
            [FromBody] CustomerJourneyStep1PlateModel model)
        {
            try
            {
                var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var jsonElement = JsonSerializer.SerializeToElement(model, jsonOptions);
                var result = await _customerJourneyService.CreateJourneyWithPlateAsync(jsonElement);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de customer journey",
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
        /// Actualiza los detalles del vehículo en el journey (Paso 2)
        /// </summary>
        [HttpPost("{id}/vehicle-details")]
        [Authorize]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> UpdateVehicleDetails(
            string id,
            [FromBody] CustomerJourneyStep2Model model)
        {
            try
            {
                var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var jsonElement = JsonSerializer.SerializeToElement(model, jsonOptions);
                var result = await _customerJourneyService.UpdateVehicleDetailsAsync(id, jsonElement);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de customer journey",
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
        /// Obtiene las opciones de daño disponibles para un journey
        /// </summary>
        [HttpGet("{customerJourneyId}/damage/options")]
        [Authorize]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> GetDamageOptions(string customerJourneyId)
        {
            try
            {
                var result = await _customerJourneyService.GetDamageOptionsAsync(customerJourneyId);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de opciones de daño",
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
        /// Actualiza la condición del vehículo en el journey (Paso 3)
        /// </summary>
        [HttpPost("{id}/vehicle-condition")]
        [Authorize]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> UpdateVehicleCondition(
            string id,
            [FromBody] CustomerJourneyStep3Model model)
        {
            try
            {
                _logger.LogInformation(
                    "UpdateVehicleCondition llamado para journey ID: {JourneyId}, RequestId: {RequestId}",
                    id,
                    HttpContext.TraceIdentifier);

                var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var jsonElement = JsonSerializer.SerializeToElement(model, jsonOptions);
                var result = await _customerJourneyService.UpdateVehicleConditionAsync(id, jsonElement);

                _logger.LogInformation(
                    "UpdateVehicleCondition completado exitosamente para journey ID: {JourneyId}, RequestId: {RequestId}",
                    id,
                    HttpContext.TraceIdentifier);

                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de customer journey",
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
        /// Actualiza el trabajo de carrocería en el journey (Paso 4)
        /// </summary>
        [HttpPost("{id}/body-work")]
        [Authorize]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> UpdateBodyWork(
            string id,
            [FromBody] CustomerJourneyStep4Model model)
        {
            try
            {
                var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var jsonElement = JsonSerializer.SerializeToElement(model, jsonOptions);
                var result = await _customerJourneyService.UpdateBodyWorkAsync(id, jsonElement);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de customer journey",
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
