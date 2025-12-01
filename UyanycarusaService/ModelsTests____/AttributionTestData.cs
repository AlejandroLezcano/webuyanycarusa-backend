using System.Text.Json;

namespace UyanycarusaService.ModelsTests
{
    /// <summary>
    /// Datos de prueba para respuestas de atribución cuando el servicio externo no responde.
    /// </summary>
    public static class AttributionTestData
    {
        private static readonly string VisitorJson = @"
        {
          ""visitorId"": 123456789012345
        }";

        private static readonly string VisitJson = @"
        {
          ""visitId"": 987654321098765
        }";

        /// <summary>
        /// Obtiene una respuesta de visitor de ejemplo.
        /// </summary>
        public static JsonElement GetVisitor()
            => JsonSerializer.Deserialize<JsonElement>(VisitorJson);

        /// <summary>
        /// Obtiene una respuesta de visita de ejemplo.
        /// </summary>
        public static JsonElement GetVisit()
            => JsonSerializer.Deserialize<JsonElement>(VisitJson);
    }
}

