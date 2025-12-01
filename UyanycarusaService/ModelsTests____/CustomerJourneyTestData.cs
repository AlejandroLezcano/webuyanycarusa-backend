using System.Text.Json;

namespace UyanycarusaService.ModelsTests
{
    /// <summary>
    /// Datos de prueba para respuestas de customer journey cuando el servicio externo no responde.
    /// </summary>
    public static class CustomerJourneyTestData
    {
        private static readonly string JourneyWithYMMJson = @"
        {
          ""customerJourneyId"": ""11111111-1111-1111-1111-111111111111"",
          ""year"": 2020,
          ""make"": ""Toyota"",
          ""model"": ""Camry"",
          ""hasBeenInAccident"": null,
          ""insuranceLoss"": null,
          ""canDriveCarToBranch"": null,
          ""body"": null,
          ""vin"": null,
          ""email"": null,
          ""hasDamage"": null,
          ""optionalPhoneNumber"": null,
          ""publicValuationMethod"": ""YearMakeModelDropdowns"",
          ""mileage"": null,
          ""series"": null,
          ""zipCode"": null,
          ""odometerChanged"": false,
          ""priorUsage"": false,
          ""isVehicleDescriptionComplete"": false
        }";

        private static readonly string JourneyWithVINJson = @"
        {
          ""customerJourneyId"": ""22222222-2222-2222-2222-222222222222"",
          ""year"": 2019,
          ""make"": ""Honda"",
          ""model"": ""Civic"",
          ""hasBeenInAccident"": null,
          ""insuranceLoss"": null,
          ""canDriveCarToBranch"": null,
          ""body"": null,
          ""vin"": ""1HGBH41JXMN109186"",
          ""email"": null,
          ""hasDamage"": null,
          ""optionalPhoneNumber"": null,
          ""publicValuationMethod"": ""Vin"",
          ""mileage"": null,
          ""series"": null,
          ""zipCode"": null,
          ""odometerChanged"": false,
          ""priorUsage"": false,
          ""isVehicleDescriptionComplete"": false
        }";

        private static readonly string JourneyWithPlateJson = @"
        {
          ""customerJourneyId"": ""33333333-3333-3333-3333-333333333333"",
          ""year"": 2021,
          ""make"": ""Ford"",
          ""model"": ""F-150"",
          ""hasBeenInAccident"": null,
          ""insuranceLoss"": null,
          ""canDriveCarToBranch"": null,
          ""body"": null,
          ""vin"": null,
          ""email"": null,
          ""hasDamage"": null,
          ""optionalPhoneNumber"": null,
          ""publicValuationMethod"": ""LicensePlate"",
          ""mileage"": null,
          ""series"": null,
          ""zipCode"": null,
          ""odometerChanged"": false,
          ""priorUsage"": false,
          ""isVehicleDescriptionComplete"": false
        }";

        private static readonly string JourneyByIdJson = @"
        {
          ""customerJourneyId"": ""11111111-1111-1111-1111-111111111111"",
          ""year"": 2020,
          ""make"": ""Toyota"",
          ""model"": ""Camry"",
          ""hasBeenInAccident"": false,
          ""insuranceLoss"": false,
          ""canDriveCarToBranch"": true,
          ""body"": ""Sedan"",
          ""vin"": ""4T1B11HK5KU123456"",
          ""email"": ""customer@example.com"",
          ""hasDamage"": false,
          ""optionalPhoneNumber"": ""+13055550123"",
          ""publicValuationMethod"": ""YearMakeModelDropdowns"",
          ""mileage"": 45000,
          ""series"": ""LE"",
          ""zipCode"": ""33101"",
          ""odometerChanged"": false,
          ""priorUsage"": false,
          ""isVehicleDescriptionComplete"": true,
          ""valuationStatus"": ""Success"",
          ""valuationAmount"": 18500.00,
          ""closestBranchContactInfo"": {
            ""branchId"": 101,
            ""branchName"": ""Miami Downtown"",
            ""phoneNumber"": ""+13055551001"",
            ""branchManagerName"": ""John Manager""
          },
          ""currentAppointment"": {
            ""branchId"": 101,
            ""branchName"": ""Miami Downtown"",
            ""appointmentDate"": ""2025-01-15T14:30:00Z"",
            ""timeSlot"": ""14:30 - 15:00""
          },
          ""isConditionalOffer"": true,
          ""conditionalOfferExpiresOn"": ""2025-12-31T23:59:59Z"",
          ""customerVehicleId"": 123456
        }";

        private static readonly string JourneyByVisitIdJson = @"
        {
          ""customerJourneyId"": ""44444444-4444-4444-4444-444444444444"",
          ""year"": 2018,
          ""make"": ""Chevrolet"",
          ""model"": ""Silverado"",
          ""hasBeenInAccident"": true,
          ""insuranceLoss"": false,
          ""canDriveCarToBranch"": true,
          ""body"": ""Truck"",
          ""vin"": ""1GCVKREC8JZ123456"",
          ""email"": ""truck.owner@example.com"",
          ""hasDamage"": true,
          ""optionalPhoneNumber"": ""+13055550234"",
          ""publicValuationMethod"": ""YearMakeModelDropdowns"",
          ""mileage"": 75000,
          ""series"": ""LT"",
          ""zipCode"": ""90210"",
          ""odometerChanged"": false,
          ""priorUsage"": false,
          ""isVehicleDescriptionComplete"": true,
          ""valuationStatus"": ""RequiresDamageEntry"",
          ""valuationAmount"": null,
          ""closestBranchContactInfo"": {
            ""branchId"": 202,
            ""branchName"": ""Los Angeles West"",
            ""phoneNumber"": ""+12135552002"",
            ""branchManagerName"": ""Jane Manager""
          },
          ""currentAppointment"": null,
          ""isConditionalOffer"": false,
          ""conditionalOfferExpiresOn"": null,
          ""customerVehicleId"": 654321
        }";

        private static readonly string UpdatedJourneyJson = @"
        {
          ""customerJourneyId"": ""11111111-1111-1111-1111-111111111111"",
          ""year"": 2020,
          ""make"": ""Toyota"",
          ""model"": ""Camry"",
          ""hasBeenInAccident"": false,
          ""insuranceLoss"": false,
          ""canDriveCarToBranch"": true,
          ""body"": ""Sedan"",
          ""vin"": ""4T1B11HK5KU123456"",
          ""email"": ""customer@example.com"",
          ""hasDamage"": false,
          ""optionalPhoneNumber"": ""+13055550123"",
          ""publicValuationMethod"": ""YearMakeModelDropdowns"",
          ""mileage"": 45000,
          ""series"": ""LE"",
          ""zipCode"": ""33101"",
          ""odometerChanged"": false,
          ""priorUsage"": false,
          ""isVehicleDescriptionComplete"": true
        }";

        private static readonly string DamageOptionsJson = @"
        {
          ""damageOptions"": [
            {
              ""zoneId"": 1,
              ""zoneName"": ""Front"",
              ""componentId"": 10,
              ""componentName"": ""Bumper"",
              ""faultId"": 100,
              ""faultName"": ""Minor Scratches""
            },
            {
              ""zoneId"": 1,
              ""zoneName"": ""Front"",
              ""componentId"": 10,
              ""componentName"": ""Bumper"",
              ""faultId"": 101,
              ""faultName"": ""Major Damage""
            },
            {
              ""zoneId"": 2,
              ""zoneName"": ""Rear"",
              ""componentId"": 20,
              ""componentName"": ""Trunk"",
              ""faultId"": 200,
              ""faultName"": ""Dent""
            }
          ],
          ""damageQuestions"": [
            {
              ""questionName"": ""HasAccident"",
              ""question"": ""Has this vehicle been in an accident?"",
              ""preselectedAnswer"": false
            },
            {
              ""questionName"": ""HasDamage"",
              ""question"": ""Does this vehicle have any visible damage?"",
              ""preselectedAnswer"": false
            }
          ]
        }";

        /// <summary>
        /// Obtiene una respuesta de journey con YMM de ejemplo.
        /// </summary>
        public static JsonElement GetJourneyWithYMM()
            => JsonSerializer.Deserialize<JsonElement>(JourneyWithYMMJson);

        /// <summary>
        /// Obtiene una respuesta de journey con VIN de ejemplo.
        /// </summary>
        public static JsonElement GetJourneyWithVIN()
            => JsonSerializer.Deserialize<JsonElement>(JourneyWithVINJson);

        /// <summary>
        /// Obtiene una respuesta de journey con Plate de ejemplo.
        /// </summary>
        public static JsonElement GetJourneyWithPlate()
            => JsonSerializer.Deserialize<JsonElement>(JourneyWithPlateJson);

        /// <summary>
        /// Obtiene una respuesta de journey por ID de ejemplo.
        /// </summary>
        public static JsonElement GetJourneyById()
            => JsonSerializer.Deserialize<JsonElement>(JourneyByIdJson);

        /// <summary>
        /// Obtiene una respuesta de journey por visitId de ejemplo.
        /// </summary>
        public static JsonElement GetJourneyByVisitId()
            => JsonSerializer.Deserialize<JsonElement>(JourneyByVisitIdJson);

        /// <summary>
        /// Obtiene una respuesta de journey actualizado de ejemplo.
        /// </summary>
        public static JsonElement GetUpdatedJourney()
            => JsonSerializer.Deserialize<JsonElement>(UpdatedJourneyJson);

        /// <summary>
        /// Obtiene una respuesta de opciones de daño de ejemplo.
        /// </summary>
        public static JsonElement GetDamageOptions()
            => JsonSerializer.Deserialize<JsonElement>(DamageOptionsJson);
    }
}

