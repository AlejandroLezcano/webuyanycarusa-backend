using System.Text.Json;

namespace UyanycarusaService.ModelsTests
{
    /// <summary>
    /// Datos de prueba para respuestas de contenido cuando el servicio externo no responde.
    /// </summary>
    public static class ContentTestData
    {
        private static readonly string FaqsJson = @"
        {
          ""faqCategories"": [
            {
              ""categoryId"": 1,
              ""categoryTitle"": ""General Questions"",
              ""categorySlug"": ""general-questions"",
              ""questionCount"": 15
            },
            {
              ""categoryId"": 2,
              ""categoryTitle"": ""Valuation Process"",
              ""categorySlug"": ""valuation-process"",
              ""questionCount"": 12
            },
            {
              ""categoryId"": 3,
              ""categoryTitle"": ""Payment & Pricing"",
              ""categorySlug"": ""payment-pricing"",
              ""questionCount"": 8
            },
            {
              ""categoryId"": 4,
              ""categoryTitle"": ""Vehicle Requirements"",
              ""categorySlug"": ""vehicle-requirements"",
              ""questionCount"": 10
            },
            {
              ""categoryId"": 5,
              ""categoryTitle"": ""Appointments"",
              ""categorySlug"": ""appointments"",
              ""questionCount"": 7
            }
          ]
        }";

        private static readonly string FaqsBySlugJson = @"
        [
          {
            ""questionText"": ""How does the valuation process work?"",
            ""answerHtml"": ""<p>The valuation process is simple and straightforward. First, you provide us with your vehicle's information including year, make, model, mileage, and condition. Our system then calculates an estimated value based on current market data. You can then schedule an appointment at one of our branches for a final inspection and offer.</p>""
          },
          {
            ""questionText"": ""How long does the valuation take?"",
            ""answerHtml"": ""<p>The initial online valuation takes just a few minutes. Once you visit our branch, the physical inspection typically takes 15-30 minutes, and you'll receive your final offer immediately.</p>""
          },
          {
            ""questionText"": ""Is the online valuation accurate?"",
            ""answerHtml"": ""<p>Our online valuation provides an estimated value based on the information you provide. The final offer will be determined after a physical inspection of your vehicle at one of our branches. The online estimate is typically within 5-10% of the final offer.</p>""
          },
          {
            ""questionText"": ""Can I get a valuation without visiting a branch?"",
            ""answerHtml"": ""<p>While we provide an initial online estimate, a physical inspection is required to provide you with a final, binding offer. This ensures accuracy and fairness for both parties.</p>""
          },
          {
            ""questionText"": ""What information do I need for a valuation?"",
            ""answerHtml"": ""<p>You'll need your vehicle's year, make, model, current mileage, and information about its condition including any damage or accidents. Having your VIN or license plate number can also help speed up the process.</p>""
          }
        ]";

        private static readonly string LandingPagesJson = @"
        [
          {
            ""slug"": ""sell-your-car"",
            ""title"": ""Sell Your Car""
          },
          {
            ""slug"": ""how-it-works"",
            ""title"": ""How It Works""
          },
          {
            ""slug"": ""why-choose-us"",
            ""title"": ""Why Choose Us""
          },
          {
            ""slug"": ""instant-valuation"",
            ""title"": ""Get an Instant Valuation""
          },
          {
            ""slug"": ""faq"",
            ""title"": ""Frequently Asked Questions""
          },
          {
            ""slug"": ""contact-us"",
            ""title"": ""Contact Us""
          }
        ]";

        private static readonly string LandingPageBySlugJson = @"
        {
          ""slug"": ""sell-your-car"",
          ""title"": ""Sell Your Car - Fast, Easy, and Fair"",
          ""metaDescription"": ""Sell your car quickly and easily with WeBuyAnyCar USA. Get an instant valuation and receive payment the same day."",
          ""h1Title"": ""Sell Your Car in 3 Simple Steps"",
          ""h2Title"": ""Get Your Free Valuation Today"",
          ""content"": ""<div><h2>Why Sell to Us?</h2><p>We make selling your car simple, fast, and stress-free. No haggling, no hidden fees, just a fair price for your vehicle.</p><h3>Our Process</h3><ol><li>Get your free online valuation in minutes</li><li>Schedule an appointment at a branch near you</li><li>Receive your payment the same day</li></ol><h3>What We Accept</h3><p>We buy cars in any condition - running or not, with or without damage, and regardless of age or mileage.</p></div>"",
          ""heroImage"": ""https://example.com/images/sell-your-car-hero.jpg"",
          ""ctaButtonText"": ""Get Started"",
          ""ctaButtonLink"": ""/valuation""
        }";

        /// <summary>
        /// Obtiene una respuesta de lista de FAQs de ejemplo.
        /// </summary>
        public static JsonElement GetFaqs()
            => JsonSerializer.Deserialize<JsonElement>(FaqsJson);

        /// <summary>
        /// Obtiene una respuesta de FAQs por slug de ejemplo.
        /// </summary>
        public static JsonElement GetFaqsBySlug()
            => JsonSerializer.Deserialize<JsonElement>(FaqsBySlugJson);

        /// <summary>
        /// Obtiene una respuesta de lista de landing pages de ejemplo.
        /// </summary>
        public static JsonElement GetLandingPages()
            => JsonSerializer.Deserialize<JsonElement>(LandingPagesJson);

        /// <summary>
        /// Obtiene una respuesta de landing page por slug de ejemplo.
        /// </summary>
        public static JsonElement GetLandingPageBySlug()
            => JsonSerializer.Deserialize<JsonElement>(LandingPageBySlugJson);

    }
}

