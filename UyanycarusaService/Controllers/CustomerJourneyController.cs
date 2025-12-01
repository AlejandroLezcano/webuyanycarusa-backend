using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UyanycarusaService.Services;
using UyanycarusaService.Dtos;

namespace UyanycarusaService.Controllers
{
    /// <summary>
    /// Controlador para operaciones de customer journey
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/customer-journey")]
    [Authorize]
    public class CustomerJourneyController : ControllerBase
    {
        private readonly ICustomerJourneyService _customerJourneyService;
        private readonly ILogger<CustomerJourneyController> _logger;

        public CustomerJourneyController(ICustomerJourneyService customerJourneyService, ILogger<CustomerJourneyController> logger)
        {
            _customerJourneyService = customerJourneyService;
            _logger = logger;
        }


        /// <summary>
        /// Obtiene un customer journey por su ID (UUID)
        /// </summary>
        /// <param name="id">ID del journey (UUID)</param>
        /// <returns>Respuesta del journey del servicio externo</returns>
        /// <response code="200">Journey obtenido correctamente</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="404">Journey no encontrado</response>
        /// <response code="500">Error al consumir el servicio externo</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> GetJourneyById(Guid id)
        {
            try
            {
                var result = await _customerJourneyService.GetJourneyByIdAsync(id.ToString());
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
        /// Obtiene un customer journey por visitId (integer)
        /// </summary>
        /// <param name="visitId">ID de la visita (integer)</param>
        /// <returns>Respuesta del journey del servicio externo</returns>
        /// <response code="200">Journey obtenido correctamente</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="404">Journey no encontrado</response>
        /// <response code="500">Error al consumir el servicio externo</response>
        [HttpGet("{visitId:int}")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> GetJourneyByVisitId(int visitId)
        {
            try
            {
                var result = await _customerJourneyService.GetJourneyByVisitIdAsync(visitId);
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
        /// <param name="model">Datos del vehículo (YMM)</param>
        /// <returns>Respuesta del journey del servicio externo</returns>
        /// <response code="200">Journey creado correctamente</response>
        /// <response code="400">Solicitud inválida para el servicio externo</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="500">Error al consumir el servicio externo</response>
        [HttpPost]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> CreateJourneyWithYMM([FromBody] CustomerJourneyStep1YMMModel model)
        {
            try
            {
                var jsonElement = JsonSerializer.SerializeToElement(model);
                var result = await _customerJourneyService.CreateJourneyWithYMMAsync(jsonElement);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                //_logger.LogWarning(ex, "Error de comunicación con el servicio externo /customer-journey");
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de customer journey",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error inesperado al crear customer journey con YMM");
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
        /// <param name="model">Datos del vehículo (VIN)</param>
        /// <returns>Respuesta del journey del servicio externo</returns>
        /// <response code="200">Journey creado correctamente</response>
        /// <response code="400">Solicitud inválida para el servicio externo</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="500">Error al consumir el servicio externo</response>
        [HttpPost("vin")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> CreateJourneyWithVIN([FromBody] CustomerJourneyStep1VINModel model)
        {
            try
            {
                var jsonElement = JsonSerializer.SerializeToElement(model);
                var result = await _customerJourneyService.CreateJourneyWithVINAsync(jsonElement);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                //_logger.LogWarning(ex, "Error de comunicación con el servicio externo /customer-journey/vin");
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de customer journey",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error inesperado al crear customer journey con VIN");
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
        /// <param name="model">Datos del vehículo (Plate)</param>
        /// <returns>Respuesta del journey del servicio externo</returns>
        /// <response code="200">Journey creado correctamente</response>
        /// <response code="400">Solicitud inválida para el servicio externo</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="500">Error al consumir el servicio externo</response>
        [HttpPost("plate")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> CreateJourneyWithPlate([FromBody] CustomerJourneyStep1PlateModel model)
        {
            try
            {
                var jsonElement = JsonSerializer.SerializeToElement(model);
                var result = await _customerJourneyService.CreateJourneyWithPlateAsync(jsonElement);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                //_logger.LogWarning(ex, "Error de comunicación con el servicio externo /customer-journey/plate");
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de customer journey",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error inesperado al crear customer journey con Plate");
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
        /// <param name="id">ID del journey (UUID)</param>
        /// <param name="model">Detalles del vehículo</param>
        /// <returns>Respuesta del journey del servicio externo</returns>
        /// <response code="200">Detalles actualizados correctamente</response>
        /// <response code="400">Solicitud inválida para el servicio externo</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="404">Journey no encontrado</response>
        /// <response code="500">Error al consumir el servicio externo</response>
        [HttpPost("{id}/vehicle-details")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> UpdateVehicleDetails(string id, [FromBody] CustomerJourneyStep2Model model)
        {
            try
            {
                var jsonElement = JsonSerializer.SerializeToElement(model);
                var result = await _customerJourneyService.UpdateVehicleDetailsAsync(id, jsonElement);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                //_logger.LogWarning(ex, "Error de comunicación con el servicio externo /customer-journey/vehicle-details");
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
        /// <param name="customerJourneyId">ID del journey (UUID)</param>
        /// <returns>Respuesta con opciones de daño del servicio externo</returns>
        /// <response code="200">Opciones de daño obtenidas correctamente</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="404">Journey no encontrado</response>
        /// <response code="500">Error al consumir el servicio externo</response>
        [HttpGet("{customerJourneyId}/damage/options")]
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
                //_logger.LogWarning(ex, "Error de comunicación con el servicio externo /customer-journey/damage/options");
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de opciones de daño",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error inesperado al obtener opciones de daño");
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
        /// <param name="id">ID del journey (UUID)</param>
        /// <param name="model">Condición del vehículo</param>
        /// <returns>Respuesta del journey del servicio externo</returns>
        /// <response code="200">Condición actualizada correctamente</response>
        /// <response code="400">Solicitud inválida para el servicio externo</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="404">Journey no encontrado</response>
        /// <response code="500">Error al consumir el servicio externo</response>
        [HttpPost("{id}/vehicle-condition")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> UpdateVehicleCondition(string id, [FromBody] CustomerJourneyStep3Model model)
        {
            try
            {
                var jsonElement = JsonSerializer.SerializeToElement(model);
                var result = await _customerJourneyService.UpdateVehicleConditionAsync(id, jsonElement);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                //_logger.LogWarning(ex, "Error de comunicación con el servicio externo /customer-journey/vehicle-condition");
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de customer journey",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error inesperado al actualizar condición del vehículo");
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
        /// <param name="id">ID del journey (UUID)</param>
        /// <param name="model">Trabajo de carrocería</param>
        /// <returns>Respuesta del journey del servicio externo</returns>
        /// <response code="200">Trabajo de carrocería actualizado correctamente</response>
        /// <response code="400">Solicitud inválida para el servicio externo</response>
        /// <response code="401">No autorizado. Se requiere un token JWT válido</response>
        /// <response code="404">Journey no encontrado</response>
        /// <response code="500">Error al consumir el servicio externo</response>
        [HttpPost("{id}/body-work")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> UpdateBodyWork(string id, [FromBody] CustomerJourneyStep4Model model)
        {
            try
            {
                var jsonElement = JsonSerializer.SerializeToElement(model);
                var result = await _customerJourneyService.UpdateBodyWorkAsync(id, jsonElement);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                //_logger.LogWarning(ex, "Error de comunicación con el servicio externo /customer-journey/body-work");
                return StatusCode(500, new
                {
                    message = "Error al comunicarse con el servicio externo de customer journey",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error inesperado al actualizar trabajo de carrocería");
                return StatusCode(500, new
                {
                    message = "Error inesperado al procesar la solicitud",
                    detail = ex.Message
                });
            }
        }
    }
}

