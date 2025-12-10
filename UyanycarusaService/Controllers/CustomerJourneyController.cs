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
    /// Controller for all Customer Journey operations.
    /// All endpoints require a valid JWT token.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/customer-journey")]
    // IMPORTANTE: no [Authorize] a nivel de clase
    [AllowAnonymous] // JWT enforced globally for this controller
    [Tags("CustomerJourney")]
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

        // --------------------------------------------------------------------
        // QUERY ENDPOINTS
        // --------------------------------------------------------------------

        /// <summary>
        /// Retrieves a customer journey by its UUID.
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
                    message = "Error communicating with the external customer journey service.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Unexpected error processing request.",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Gets a customer journey by visitId (integer)
        /// GET /api/customer-journey/{visitId:int}
        /// Retrieves a customer journey by visitId (integer).
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
                    message = "Error communicating with the external customer journey service.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Unexpected error processing request.",
                    detail = ex.Message
                });
            }
        }

        // --------------------------------------------------------------------
        // STEP 1: CREATE (YMM, VIN, PLATE)
        // --------------------------------------------------------------------

        /// <summary>
        /// Starts a customer journey using Year/Make/Model.
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
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.SerializeToElement(model, options);
                var result = await _customerJourneyService.CreateJourneyWithYMMAsync(json);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "External service error while creating YMM journey.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating customer journey with YMM.");
                return StatusCode(500, new
                {
                    message = "Unexpected error processing request.",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Starts a customer journey using a VIN.
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
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.SerializeToElement(model, options);
                var result = await _customerJourneyService.CreateJourneyWithVINAsync(json);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "External service error while creating VIN journey.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating customer journey with VIN.");
                return StatusCode(500, new
                {
                    message = "Unexpected error processing request.",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Starts a customer journey using a license plate.
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
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.SerializeToElement(model, options);
                var result = await _customerJourneyService.CreateJourneyWithPlateAsync(json);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "External service error while creating Plate journey.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating journey with plate.");
                return StatusCode(500, new
                {
                    message = "Unexpected error processing request.",
                    detail = ex.Message
                });
            }
        }

        // --------------------------------------------------------------------
        // STEP 2: VEHICLE DETAILS
        // --------------------------------------------------------------------

        /// <summary>
        /// Updates the vehicle details in the customer journey.
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
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.SerializeToElement(model, options);
                var result = await _customerJourneyService.UpdateVehicleDetailsAsync(id, json);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "External service error while updating vehicle details.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Unexpected error processing request.",
                    detail = ex.Message
                });
            }
        }

        // --------------------------------------------------------------------
        // STEP 2B: DAMAGE OPTIONS
        // --------------------------------------------------------------------

        /// <summary>
        /// Retrieves the available damage options for a given customer journey.
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
                    message = "External service error retrieving damage options.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error retrieving damage options.");
                return StatusCode(500, new
                {
                    message = "Unexpected error processing request.",
                    detail = ex.Message
                });
            }
        }

        // --------------------------------------------------------------------
        // STEP 3: VEHICLE CONDITION
        // --------------------------------------------------------------------

        /// <summary>
        /// Updates vehicle condition details.
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
                _logger.LogInformation("Updating vehicle condition for journey {JourneyId}", id);

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.SerializeToElement(model, options);
                var result = await _customerJourneyService.UpdateVehicleConditionAsync(id, json);

                _logger.LogInformation("Vehicle condition updated successfully for journey {JourneyId}", id);

                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "External service error while updating vehicle condition.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating vehicle condition.");
                return StatusCode(500, new
                {
                    message = "Unexpected error processing request.",
                    detail = ex.Message
                });
            }
        }

        // --------------------------------------------------------------------
        // STEP 4: BODY WORK
        // --------------------------------------------------------------------

        /// <summary>
        /// Updates body work information for the customer journey.
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
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.SerializeToElement(model, options);
                var result = await _customerJourneyService.UpdateBodyWorkAsync(id, json);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "External service error while updating body work.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating body work.");
                return StatusCode(500, new
                {
                    message = "Unexpected error processing request.",
                    detail = ex.Message
                });
            }
        }
    }
}
