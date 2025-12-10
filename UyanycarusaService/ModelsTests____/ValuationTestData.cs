using System.Text.Json;

namespace UyanycarusaService.ModelsTests
{
    /// <summary>
    /// Test data for valuation responses when external service doesn't respond.
    /// </summary>
    public static class ValuationTestData
    {
        private static readonly string BasicValuationJson = @"
        {
          ""status"": ""Success"",
          ""customerJourneyId"": ""11111111-1111-1111-1111-111111111111"",
          ""wasSmsOptedIn"": true,
          ""customerVehicleId"": 123456,
          ""valuationAmount"": 15250.75,
          ""isConditionalOffer"": true,
          ""conditionalOfferExpiresOn"": ""2025-12-31T23:59:59Z"",
          ""valuationId"": 987654,
          ""currentAppointment"": {
            ""branchId"": 101,
            ""branchName"": ""Miami Downtown"",
            ""appointmentDate"": ""2025-01-15T14:30:00Z"",
            ""timeSlot"": ""14:30 - 15:00""
          }
        }";

        private static readonly string WithDamageValuationJson = @"
        {
          ""status"": ""Success"",
          ""customerJourneyId"": ""22222222-2222-2222-2222-222222222222"",
          ""wasSmsOptedIn"": false,
          ""customerVehicleId"": 654321,
          ""valuationAmount"": 9800.00,
          ""isConditionalOffer"": false,
          ""conditionalOfferExpiresOn"": null,
          ""valuationId"": 123987,
          ""currentAppointment"": {
            ""branchId"": 202,
            ""branchName"": ""Los Angeles West"",
            ""appointmentDate"": ""2025-02-10T10:00:00Z"",
            ""timeSlot"": ""10:00 - 10:30""
          }
        }";

        /// <summary>
        /// Gets a sample basic valuation response.
        /// </summary>
        public static JsonElement GetBasicValuation()
            => JsonSerializer.Deserialize<JsonElement>(BasicValuationJson);

        /// <summary>
        /// Gets a sample valuation with damage response.
        /// </summary>
        public static JsonElement GetValuationWithDamage()
            => JsonSerializer.Deserialize<JsonElement>(WithDamageValuationJson);
    }
}


