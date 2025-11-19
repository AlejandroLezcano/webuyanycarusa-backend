using System.Text.Json;

namespace UyanycarusaService.ModelsTests
{
    /// <summary>
    /// Datos de prueba para respuestas de Scheduling cuando el servicio externo no responde.
    /// </summary>
    public static class SchedulingTestData
    {
        private static readonly string RequestOTPJson = @"
        {
          ""otpRequestId"": ""550e8400-e29b-41d4-a716-446655440000"",
          ""phoneNumber"": ""+15551234567"",
          ""expiresIn"": 300,
          ""message"": ""OTP code has been sent to your phone number"",
          ""requestedAt"": ""2025-01-15T10:30:00Z"",
          ""status"": ""Pending""
        }";

        /// <summary>
        /// Obtiene una respuesta de solicitud OTP de ejemplo.
        /// </summary>
        public static JsonElement GetRequestOTP()
            => JsonSerializer.Deserialize<JsonElement>(RequestOTPJson);
    }
}

