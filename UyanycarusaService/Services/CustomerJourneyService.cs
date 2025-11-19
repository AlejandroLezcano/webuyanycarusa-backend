using System.Net.Http.Json;
using System.Text.Json;
using UyanycarusaService.ModelsTests;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Servicio para operaciones de customer journey
    /// </summary>
    public class CustomerJourneyService : ICustomerJourneyService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CustomerJourneyService> _logger;
        private readonly bool _useTestData;

        public CustomerJourneyService(
            IHttpClientFactory httpClientFactory,
            ILogger<CustomerJourneyService> logger,
            IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("WebuyAnyCarApi");
            _logger = logger;
            _useTestData = configuration.GetValue<bool>("dataTest");
        }

        /// <inheritdoc />
        public async Task<JsonElement> CreateJourneyWithYMMAsync(JsonElement model)
        {
            try
            {
                _logger.LogInformation("Creando customer journey con YMM en el servicio externo /customer-journey");

                using var response = await _httpClient.PostAsJsonAsync("/customer-journey", model);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    _logger.LogInformation("Customer journey con YMM creado exitosamente");
                    return json;
                }

                _logger.LogWarning("El servicio externo /customer-journey retornó un código de estado: {StatusCode}", response.StatusCode);

                if (_useTestData)
                {
                    _logger.LogInformation("Usando datos de prueba de journey YMM desde CustomerJourneyTestData (dataTest=true)");
                    return CustomerJourneyTestData.GetJourneyWithYMM();
                }

                throw new HttpRequestException(
                    $"Error al crear customer journey con YMM. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                if (_useTestData)
                {
                    _logger.LogWarning(ex, "No se pudo comunicar con el servicio externo /customer-journey, usando datos de prueba (dataTest=true)");
                    return CustomerJourneyTestData.GetJourneyWithYMM();
                }

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear customer journey con YMM");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<JsonElement> CreateJourneyWithVINAsync(JsonElement model)
        {
            try
            {
                _logger.LogInformation("Creando customer journey con VIN en el servicio externo /customer-journey/vin");

                using var response = await _httpClient.PostAsJsonAsync("/customer-journey/vin", model);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    _logger.LogInformation("Customer journey con VIN creado exitosamente");
                    return json;
                }

                _logger.LogWarning("El servicio externo /customer-journey/vin retornó un código de estado: {StatusCode}", response.StatusCode);

                if (_useTestData)
                {
                    _logger.LogInformation("Usando datos de prueba de journey VIN desde CustomerJourneyTestData (dataTest=true)");
                    return CustomerJourneyTestData.GetJourneyWithVIN();
                }

                throw new HttpRequestException(
                    $"Error al crear customer journey con VIN. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                if (_useTestData)
                {
                    _logger.LogWarning(ex, "No se pudo comunicar con el servicio externo /customer-journey/vin, usando datos de prueba (dataTest=true)");
                    return CustomerJourneyTestData.GetJourneyWithVIN();
                }

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear customer journey con VIN");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<JsonElement> CreateJourneyWithPlateAsync(JsonElement model)
        {
            try
            {
                _logger.LogInformation("Creando customer journey con Plate en el servicio externo /customer-journey/plate");

                using var response = await _httpClient.PostAsJsonAsync("/customer-journey/plate", model);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    _logger.LogInformation("Customer journey con Plate creado exitosamente");
                    return json;
                }

                _logger.LogWarning("El servicio externo /customer-journey/plate retornó un código de estado: {StatusCode}", response.StatusCode);

                if (_useTestData)
                {
                    _logger.LogInformation("Usando datos de prueba de journey Plate desde CustomerJourneyTestData (dataTest=true)");
                    return CustomerJourneyTestData.GetJourneyWithPlate();
                }

                throw new HttpRequestException(
                    $"Error al crear customer journey con Plate. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                if (_useTestData)
                {
                    _logger.LogWarning(ex, "No se pudo comunicar con el servicio externo /customer-journey/plate, usando datos de prueba (dataTest=true)");
                    return CustomerJourneyTestData.GetJourneyWithPlate();
                }

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear customer journey con Plate");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<JsonElement> GetJourneyByIdAsync(string id)
        {
            try
            {
                _logger.LogInformation("Obteniendo customer journey por ID {Id} desde el servicio externo", id);

                var response = await _httpClient.GetAsync($"/customer-journey/{id}");

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    _logger.LogInformation("Customer journey obtenido exitosamente");
                    return json;
                }

                _logger.LogWarning("El servicio externo /customer-journey/{Id} retornó un código de estado: {StatusCode}", id, response.StatusCode);

                if (_useTestData)
                {
                    _logger.LogInformation("Usando datos de prueba de journey desde CustomerJourneyTestData (dataTest=true)");
                    return CustomerJourneyTestData.GetJourneyById();
                }

                throw new HttpRequestException(
                    $"Error al obtener customer journey. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                if (_useTestData)
                {
                    _logger.LogWarning(ex, "No se pudo comunicar con el servicio externo /customer-journey, usando datos de prueba (dataTest=true)");
                    return CustomerJourneyTestData.GetJourneyById();
                }

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener customer journey");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<JsonElement> GetJourneyByVisitIdAsync(int visitId)
        {
            try
            {
                _logger.LogInformation("Obteniendo customer journey por visitId {VisitId} desde el servicio externo", visitId);

                var response = await _httpClient.GetAsync($"/customer-journey/{visitId}");

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    _logger.LogInformation("Customer journey obtenido exitosamente");
                    return json;
                }

                _logger.LogWarning("El servicio externo /customer-journey/{VisitId} retornó un código de estado: {StatusCode}", visitId, response.StatusCode);

                if (_useTestData)
                {
                    _logger.LogInformation("Usando datos de prueba de journey desde CustomerJourneyTestData (dataTest=true)");
                    return CustomerJourneyTestData.GetJourneyByVisitId();
                }

                throw new HttpRequestException(
                    $"Error al obtener customer journey. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                if (_useTestData)
                {
                    _logger.LogWarning(ex, "No se pudo comunicar con el servicio externo /customer-journey, usando datos de prueba (dataTest=true)");
                    return CustomerJourneyTestData.GetJourneyByVisitId();
                }

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener customer journey");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<JsonElement> UpdateVehicleDetailsAsync(string id, JsonElement model)
        {
            try
            {
                _logger.LogInformation("Actualizando detalles del vehículo para journey {Id} en el servicio externo", id);

                using var response = await _httpClient.PostAsJsonAsync($"/customer-journey/{id}/vehicle-details", model);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    _logger.LogInformation("Detalles del vehículo actualizados exitosamente");
                    return json;
                }

                _logger.LogWarning("El servicio externo /customer-journey/vehicle-details retornó un código de estado: {StatusCode}", response.StatusCode);

                if (_useTestData)
                {
                    _logger.LogInformation("Usando datos de prueba desde CustomerJourneyTestData (dataTest=true)");
                    return CustomerJourneyTestData.GetUpdatedJourney();
                }

                throw new HttpRequestException(
                    $"Error al actualizar detalles del vehículo. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                if (_useTestData)
                {
                    _logger.LogWarning(ex, "No se pudo comunicar con el servicio externo, usando datos de prueba (dataTest=true)");
                    return CustomerJourneyTestData.GetUpdatedJourney();
                }

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar detalles del vehículo");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<JsonElement> GetDamageOptionsAsync(string customerJourneyId)
        {
            try
            {
                _logger.LogInformation("Obteniendo opciones de daño para journey {CustomerJourneyId} desde el servicio externo", customerJourneyId);

                var response = await _httpClient.GetAsync($"/customer-journey/{customerJourneyId}/damage/options");

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    _logger.LogInformation("Opciones de daño obtenidas exitosamente");
                    return json;
                }

                _logger.LogWarning("El servicio externo /customer-journey/damage/options retornó un código de estado: {StatusCode}", response.StatusCode);

                if (_useTestData)
                {
                    _logger.LogInformation("Usando datos de prueba de opciones de daño desde CustomerJourneyTestData (dataTest=true)");
                    return CustomerJourneyTestData.GetDamageOptions();
                }

                throw new HttpRequestException(
                    $"Error al obtener opciones de daño. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                if (_useTestData)
                {
                    _logger.LogWarning(ex, "No se pudo comunicar con el servicio externo, usando datos de prueba (dataTest=true)");
                    return CustomerJourneyTestData.GetDamageOptions();
                }

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener opciones de daño");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<JsonElement> UpdateVehicleConditionAsync(string id, JsonElement model)
        {
            try
            {
                _logger.LogInformation("Actualizando condición del vehículo para journey {Id} en el servicio externo", id);

                using var response = await _httpClient.PostAsJsonAsync($"/customer-journey/{id}/vehicle-condition", model);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    _logger.LogInformation("Condición del vehículo actualizada exitosamente");
                    return json;
                }

                _logger.LogWarning("El servicio externo /customer-journey/vehicle-condition retornó un código de estado: {StatusCode}", response.StatusCode);

                if (_useTestData)
                {
                    _logger.LogInformation("Usando datos de prueba desde CustomerJourneyTestData (dataTest=true)");
                    return CustomerJourneyTestData.GetUpdatedJourney();
                }

                throw new HttpRequestException(
                    $"Error al actualizar condición del vehículo. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                if (_useTestData)
                {
                    _logger.LogWarning(ex, "No se pudo comunicar con el servicio externo, usando datos de prueba (dataTest=true)");
                    return CustomerJourneyTestData.GetUpdatedJourney();
                }

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar condición del vehículo");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<JsonElement> UpdateBodyWorkAsync(string id, JsonElement model)
        {
            try
            {
                _logger.LogInformation("Actualizando trabajo de carrocería para journey {Id} en el servicio externo", id);

                using var response = await _httpClient.PostAsJsonAsync($"/customer-journey/{id}/body-work", model);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    _logger.LogInformation("Trabajo de carrocería actualizado exitosamente");
                    return json;
                }

                _logger.LogWarning("El servicio externo /customer-journey/body-work retornó un código de estado: {StatusCode}", response.StatusCode);

                if (_useTestData)
                {
                    _logger.LogInformation("Usando datos de prueba desde CustomerJourneyTestData (dataTest=true)");
                    return CustomerJourneyTestData.GetUpdatedJourney();
                }

                throw new HttpRequestException(
                    $"Error al actualizar trabajo de carrocería. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                if (_useTestData)
                {
                    _logger.LogWarning(ex, "No se pudo comunicar con el servicio externo, usando datos de prueba (dataTest=true)");
                    return CustomerJourneyTestData.GetUpdatedJourney();
                }

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar trabajo de carrocería");
                throw;
            }
        }
    }
}

