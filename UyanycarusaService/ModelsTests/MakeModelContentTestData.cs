using System.Text.Json;

namespace UyanycarusaService.ModelsTests
{
    /// <summary>
    /// Datos de prueba para respuestas de contenido de marca y modelo cuando el servicio externo no responde.
    /// </summary>
    public static class MakeModelContentTestData
    {
        private static readonly string MakeContentJson = @"
        {
          ""makeName"": ""Toyota"",
          ""modelName"": null,
          ""headTitle"": ""Sell Your Toyota - WeBuyAnyCar USA"",
          ""metaDescription"": ""Get the best price for your Toyota. We buy Toyotas in any condition. Free valuation and same-day payment."",
          ""h1PageTitle"": ""Sell Your Toyota Today"",
          ""h2PageTitle"": ""Get Top Dollar for Your Toyota Vehicle"",
          ""manufacturer"": ""Toyota Motor Corporation"",
          ""pageContentHtml"": ""<div><h2>Why Sell Your Toyota to Us?</h2><p>Toyota vehicles are known for their reliability and value retention. We offer competitive prices for all Toyota models, from Camry to Corolla, from RAV4 to Highlander.</p><h3>Popular Toyota Models We Buy</h3><ul><li>Camry</li><li>Corolla</li><li>RAV4</li><li>Highlander</li><li>Prius</li><li>Tacoma</li><li>Tundra</li></ul><h3>Get Your Free Valuation</h3><p>Enter your Toyota's details and get an instant valuation. No obligation, completely free.</p></div>"",
          ""models"": [
            {
              ""modelName"": ""Camry"",
              ""slug"": ""toyota-camry""
            },
            {
              ""modelName"": ""Corolla"",
              ""slug"": ""toyota-corolla""
            },
            {
              ""modelName"": ""RAV4"",
              ""slug"": ""toyota-rav4""
            },
            {
              ""modelName"": ""Highlander"",
              ""slug"": ""toyota-highlander""
            },
            {
              ""modelName"": ""Prius"",
              ""slug"": ""toyota-prius""
            }
          ]
        }";

        private static readonly string MakeModelContentJson = @"
        {
          ""makeName"": ""Toyota"",
          ""modelName"": ""Camry"",
          ""headTitle"": ""Sell Your Toyota Camry - WeBuyAnyCar USA"",
          ""metaDescription"": ""Get the best price for your Toyota Camry. We buy Camrys in any condition, any year. Free valuation and same-day payment."",
          ""h1PageTitle"": ""Sell Your Toyota Camry"",
          ""h2PageTitle"": ""Get Top Dollar for Your Camry"",
          ""manufacturer"": ""Toyota Motor Corporation"",
          ""pageContentHtml"": ""<div><h2>About the Toyota Camry</h2><p>The Toyota Camry is one of America's best-selling sedans, known for its reliability, fuel efficiency, and comfortable ride. Whether you have a 2020 Camry or an older model, we're interested in buying it.</p><h3>Why Sell Your Camry to Us?</h3><ul><li>Competitive pricing for all Camry years and trims</li><li>We accept Camrys in any condition</li><li>Fast, hassle-free process</li><li>Same-day payment available</li></ul><h3>Popular Camry Trims</h3><ul><li>LE</li><li>SE</li><li>XLE</li><li>XSE</li></ul><h3>Get Your Free Camry Valuation</h3><p>Enter your Camry's year, mileage, and condition to get an instant valuation. No obligation required.</p></div>"",
          ""models"": [
            {
              ""modelName"": ""Camry"",
              ""slug"": ""toyota-camry""
            }
          ]
        }";

        /// <summary>
        /// Obtiene una respuesta de contenido de marca de ejemplo.
        /// </summary>
        public static JsonElement GetMakeContent()
            => JsonSerializer.Deserialize<JsonElement>(MakeContentJson);

        /// <summary>
        /// Obtiene una respuesta de contenido de marca y modelo de ejemplo.
        /// </summary>
        public static JsonElement GetMakeModelContent()
            => JsonSerializer.Deserialize<JsonElement>(MakeModelContentJson);
    }
}

