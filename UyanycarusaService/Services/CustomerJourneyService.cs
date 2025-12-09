using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Servicio para operaciones de customer journey
    /// </summary>
    public class CustomerJourneyService : ICustomerJourneyService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CustomerJourneyService> _logger;
        private readonly ITokenService _tokenService;

        public CustomerJourneyService(
            IHttpClientFactory httpClientFactory,
            ILogger<CustomerJourneyService> logger,
            IConfiguration configuration,
            ITokenService tokenService)
        {
            _httpClient = httpClientFactory.CreateClient("WebuyAnyCarApi");
            _logger = logger;
            _tokenService = tokenService;
        }

        /// <inheritdoc />
        public async Task<JsonElement> CreateJourneyWithYMMAsync(JsonElement model)
        {
            try
            {
                // Remove null properties before sending to external service
                var cleanedModel = RemoveNullProperties(model);
                
                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Post, "/customer-journey")
                {
                    Content = JsonContent.Create(cleanedModel)
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                using var response = await _httpClient.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    return json;
                }


                throw new HttpRequestException(
                    $"Error creating customer journey with YMM. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error communicating with external service /customer-journey");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating customer journey with YMM");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<JsonElement> CreateJourneyWithVINAsync(JsonElement model)
        {
            try
            {
                // Remove null properties before sending to external service
                var cleanedModel = RemoveNullProperties(model);
                
                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Post, "/customer-journey/vin")
                {
                    Content = JsonContent.Create(cleanedModel)
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                using var response = await _httpClient.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    return json;
                }

                _logger.LogWarning("External service /customer-journey/vin returned status code: {StatusCode}", response.StatusCode);

                throw new HttpRequestException(
                    $"Error creating customer journey with VIN. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error communicating with external service /customer-journey/vin");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating customer journey with VIN");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<JsonElement> CreateJourneyWithPlateAsync(JsonElement model)
        {
            try
            {
                // Remove null properties before sending to external service
                var cleanedModel = RemoveNullProperties(model);
                _logger.LogWarning("Sending plate data to external service: {Model}", cleanedModel.ToString());
                
                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Post, "/customer-journey/plate")
                {
                    Content = JsonContent.Create(cleanedModel)
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                using var response = await _httpClient.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("External service response: StatusCode={StatusCode}, Content={Content}", response.StatusCode, content);

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    return json;
                }

                _logger.LogWarning("External service /customer-journey/plate returned status code: {StatusCode}", response.StatusCode);

                throw new HttpRequestException(
                    $"Error creating customer journey with Plate. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error communicating with external service /customer-journey/plate");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating customer journey with Plate");
                throw;
            }
        }

        /// <summary>
        /// Removes properties with null values from a JsonElement
        /// </summary>
        private JsonElement RemoveNullProperties(JsonElement element)
        {
            if (element.ValueKind != JsonValueKind.Object)
            {
                return element;
            }

            var dictionary = new Dictionary<string, object>();
            foreach (var property in element.EnumerateObject())
            {
                if (property.Value.ValueKind != JsonValueKind.Null)
                {
                    dictionary[property.Name] = property.Value;
                }
            }

            return JsonSerializer.SerializeToElement(dictionary);
        }

        /// <inheritdoc />
        public async Task<JsonElement> GetJourneyByIdAsync(string id)
        {
            try
            {

                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Get, $"/customer-journey/{id}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                _logger.LogInformation("Request: {Request}", request.ToString());
                var response = await _httpClient.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    return json;
                }

                _logger.LogWarning("El servicio externo /customer-journey/{Id} retornó un código de estado: {StatusCode}", id, response.StatusCode);

                throw new HttpRequestException($"Error al obtener customer journey. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al comunicarse con el servicio externo /customer-journey");
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

                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Get, $"/customer-journey/{visitId}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    return json;
                }

                _logger.LogWarning("El servicio externo /customer-journey/{VisitId} retornó un código de estado: {StatusCode}", visitId, response.StatusCode);

                throw new HttpRequestException(
                    $"Error al obtener customer journey. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al comunicarse con el servicio externo /customer-journey");
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

                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Post, $"/customer-journey/{id}/vehicle-details")
                {
                    Content = JsonContent.Create(model)
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                using var response = await _httpClient.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    return json;
                }

                _logger.LogWarning("El servicio externo /customer-journey/vehicle-details retornó un código de estado: {StatusCode}", response.StatusCode);

                throw new HttpRequestException(
                    $"Error al actualizar detalles del vehículo. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al comunicarse con el servicio externo");
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

                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Get, $"/customer-journey/{customerJourneyId}/damage/options");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    return json;
                }

                _logger.LogWarning("El servicio externo /customer-journey/damage/options retornó un código de estado: {StatusCode}", response.StatusCode);

                throw new HttpRequestException(
                    $"Error al obtener opciones de daño. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al comunicarse con el servicio externo");
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
                _logger.LogWarning("Iniciando UpdateVehicleConditionAsync para journey ID: {JourneyId} a las {Timestamp}", id, DateTime.UtcNow);
                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Post, $"/customer-journey/{id}/vehicle-condition")
                {
                    Content = JsonContent.Create(model)
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                _logger.LogWarning("Enviando petición HTTP POST a /customer-journey/{JourneyId}/vehicle-condition", id);
                using var response = await _httpClient.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Respuesta recibida del servicio externo. StatusCode: {StatusCode}, JourneyId: {JourneyId}", response.StatusCode, id);

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    _logger.LogWarning("UpdateVehicleConditionAsync completado exitosamente para journey ID: {JourneyId}", id);
                    return json;
                }

                throw new HttpRequestException(
                    $"Error al actualizar condición del vehículo. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al comunicarse con el servicio externo para journey ID: {JourneyId}", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar condición del vehículo para journey ID: {JourneyId}", id);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<JsonElement> UpdateBodyWorkAsync(string id, JsonElement model)
        {
            try
            {

                var accessToken = await _tokenService.GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Post, $"/customer-journey/{id}/body-work")
                {
                    Content = JsonContent.Create(model)
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                using var response = await _httpClient.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(content);
                    return json;
                }

                throw new HttpRequestException(
                    $"Error al actualizar trabajo de carrocería. StatusCode: {response.StatusCode}, Detail: {content}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al comunicarse con el servicio externo");
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

