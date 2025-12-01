using System.Text.Json;

namespace UyanycarusaService.ModelsTests
{
    /// <summary>
    /// Datos de prueba para respuestas de contenido de sucursales cuando el servicio externo no responde.
    /// </summary>
    public static class BranchContentTestData
    {
        private static readonly string BranchesJson = @"
        {
          ""branchLocations"": [
            {
              ""branchId"": 101,
              ""branchName"": ""Miami Downtown"",
              ""address1"": ""123 Main Street"",
              ""address2"": ""Suite 100"",
              ""city"": ""Miami"",
              ""state"": ""FL"",
              ""zipCode"": ""33101"",
              ""latitude"": 25.7617,
              ""longitude"": -80.1918,
              ""branchPhone"": ""+13055551001"",
              ""branchEmail"": ""miami@webuyanycarusa.com"",
              ""branchManagerName"": ""John Manager"",
              ""distanceMiles"": 2.5,
              ""operationHours"": [
                {
                  ""type"": ""open"",
                  ""date"": ""2025-01-15"",
                  ""dayOfWeek"": ""Monday"",
                  ""isExceptional"": false,
                  ""openTime"": ""09:00"",
                  ""closeTime"": ""18:00""
                },
                {
                  ""type"": ""open"",
                  ""date"": ""2025-01-16"",
                  ""dayOfWeek"": ""Tuesday"",
                  ""isExceptional"": false,
                  ""openTime"": ""09:00"",
                  ""closeTime"": ""18:00""
                }
              ]
            },
            {
              ""branchId"": 202,
              ""branchName"": ""Los Angeles West"",
              ""address1"": ""456 Sunset Boulevard"",
              ""address2"": null,
              ""city"": ""Los Angeles"",
              ""state"": ""CA"",
              ""zipCode"": ""90210"",
              ""latitude"": 34.0522,
              ""longitude"": -118.2437,
              ""branchPhone"": ""+12135552002"",
              ""branchEmail"": ""la@webuyanycarusa.com"",
              ""branchManagerName"": ""Jane Manager"",
              ""distanceMiles"": 5.3,
              ""operationHours"": [
                {
                  ""type"": ""open"",
                  ""date"": ""2025-01-15"",
                  ""dayOfWeek"": ""Monday"",
                  ""isExceptional"": false,
                  ""openTime"": ""08:00"",
                  ""closeTime"": ""17:00""
                }
              ]
            },
            {
              ""branchId"": 303,
              ""branchName"": ""Mobile Unit - Miami Area"",
              ""address1"": ""Mobile Service"",
              ""address2"": null,
              ""city"": ""Miami"",
              ""state"": ""FL"",
              ""zipCode"": ""33101"",
              ""latitude"": 25.7617,
              ""longitude"": -80.1918,
              ""branchPhone"": ""+13055553003"",
              ""branchEmail"": ""mobile@webuyanycarusa.com"",
              ""branchManagerName"": ""Mobile Team"",
              ""distanceMiles"": null,
              ""operationHours"": null
            }
          ]
        }";

        private static readonly string BranchDetailJson = @"
        {
          ""branchLocation"": {
            ""branchId"": 101,
            ""branchName"": ""Miami Downtown"",
            ""address1"": ""123 Main Street"",
            ""address2"": ""Suite 100"",
            ""city"": ""Miami"",
            ""state"": ""FL"",
            ""zipCode"": ""33101"",
            ""latitude"": 25.7617,
            ""longitude"": -80.1918,
            ""branchPhone"": ""+13055551001"",
            ""branchEmail"": ""miami@webuyanycarusa.com"",
            ""branchManagerName"": ""John Manager"",
            ""distanceMiles"": 2.5,
            ""operationHours"": [
              {
                ""type"": ""open"",
                ""date"": ""2025-01-15"",
                ""dayOfWeek"": ""Monday"",
                ""isExceptional"": false,
                ""openTime"": ""09:00"",
                ""closeTime"": ""18:00""
              },
              {
                ""type"": ""open"",
                ""date"": ""2025-01-16"",
                ""dayOfWeek"": ""Tuesday"",
                ""isExceptional"": false,
                ""openTime"": ""09:00"",
                ""closeTime"": ""18:00""
              },
              {
                ""type"": ""closed"",
                ""date"": ""2025-01-17"",
                ""dayOfWeek"": ""Wednesday"",
                ""isExceptional"": true
              }
            ],
            ""branchDirections"": [
              {
                ""directionType"": ""CardinalDirection"",
                ""directionsFrom"": ""North"",
                ""directions"": ""Take I-95 South to Exit 5. Turn right on Main Street. Branch is on the left."",
                ""sortOrder"": 1
              },
              {
                ""directionType"": ""LandMark"",
                ""directionsFrom"": ""Near Miami Beach"",
                ""directions"": ""Located 2 blocks east of the beach, next to the shopping center."",
                ""sortOrder"": 2
              }
            ],
            ""mapURL"": ""https://maps.google.com/?q=25.7617,-80.1918"",
            ""nearbyLandmarks"": ""Shopping Center, Beach, Downtown Miami"",
            ""arrival"": ""Free parking available in front of the building."",
            ""transportLinks"": ""Bus stop 100 feet away. Metro station 0.5 miles."",
            ""gettingPaid"": ""Payment available via check or direct deposit. Same day payment available."",
            ""branchType"": ""Physical"",
            ""branchImageUrl"": ""https://example.com/branches/miami-downtown.jpg"",
            ""branchImageThumbnail"": ""https://example.com/branches/miami-downtown-thumb.jpg""
          }
        }";

        /// <summary>
        /// Obtiene una respuesta de lista de sucursales de ejemplo.
        /// </summary>
        public static JsonElement GetBranches()
            => JsonSerializer.Deserialize<JsonElement>(BranchesJson);

        /// <summary>
        /// Obtiene una respuesta de detalle de sucursal de ejemplo.
        /// </summary>
        public static JsonElement GetBranchDetail()
            => JsonSerializer.Deserialize<JsonElement>(BranchDetailJson);
    }
}

