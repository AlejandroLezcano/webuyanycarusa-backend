using System.Text.Json;

namespace UyanycarusaService.ModelsTests
{
    /// <summary>
    /// Datos de prueba para respuestas de citas cuando el servicio externo no responde.
    /// </summary>
    public static class AppointmentTestData
    {
        private static readonly string AvailabilityJson = @"
        {
          ""availableDates"": [
            {
              ""date"": ""2025-01-15"",
              ""timeSlots"": [
                {
                  ""timeSlotId"": 101,
                  ""startTime"": ""09:00"",
                  ""endTime"": ""09:30"",
                  ""isAvailable"": true
                },
                {
                  ""timeSlotId"": 102,
                  ""startTime"": ""10:00"",
                  ""endTime"": ""10:30"",
                  ""isAvailable"": true
                },
                {
                  ""timeSlotId"": 103,
                  ""startTime"": ""14:00"",
                  ""endTime"": ""14:30"",
                  ""isAvailable"": true
                }
              ]
            },
            {
              ""date"": ""2025-01-16"",
              ""timeSlots"": [
                {
                  ""timeSlotId"": 201,
                  ""startTime"": ""09:00"",
                  ""endTime"": ""09:30"",
                  ""isAvailable"": true
                },
                {
                  ""timeSlotId"": 202,
                  ""startTime"": ""11:00"",
                  ""endTime"": ""11:30"",
                  ""isAvailable"": true
                }
              ]
            }
          ],
          ""branchId"": 101,
          ""branchName"": ""Miami Downtown""
        }";

        private static readonly string BookedAppointmentJson = @"
        {
          ""appointmentId"": 12345,
          ""appointmentDate"": ""2025-01-15T14:00:00Z"",
          ""appointmentTime"": ""14:00 - 14:30"",
          ""branchId"": 101,
          ""branchName"": ""Miami Downtown"",
          ""customerVehicleId"": 123456,
          ""status"": ""Confirmed"",
          ""confirmationNumber"": ""APT-2025-001234""
        }";

        private static readonly string RescheduledAppointmentJson = @"
        {
          ""appointmentId"": 12345,
          ""appointmentDate"": ""2025-01-20T10:00:00Z"",
          ""appointmentTime"": ""10:00 - 10:30"",
          ""branchId"": 101,
          ""branchName"": ""Miami Downtown"",
          ""customerVehicleId"": 123456,
          ""status"": ""Rescheduled"",
          ""confirmationNumber"": ""APT-2025-001234"",
          ""previousAppointmentDate"": ""2025-01-15T14:00:00Z""
        }";

        private static readonly string CancelledAppointmentJson = @"
        {
          ""appointmentId"": 12345,
          ""appointmentDate"": ""2025-01-15T14:00:00Z"",
          ""appointmentTime"": ""14:00 - 14:30"",
          ""isCurrent"": true,
          ""status"": ""Cancelled"",
          ""cancellationDate"": ""2025-01-10T12:00:00Z""
        }";

        /// <summary>
        /// Obtiene una respuesta de disponibilidad de citas de ejemplo.
        /// </summary>
        public static JsonElement GetAvailability()
            => JsonSerializer.Deserialize<JsonElement>(AvailabilityJson);

        /// <summary>
        /// Obtiene una respuesta de cita reservada de ejemplo.
        /// </summary>
        public static JsonElement GetBookedAppointment()
            => JsonSerializer.Deserialize<JsonElement>(BookedAppointmentJson);

        /// <summary>
        /// Obtiene una respuesta de cita reprogramada de ejemplo.
        /// </summary>
        public static JsonElement GetRescheduledAppointment()
            => JsonSerializer.Deserialize<JsonElement>(RescheduledAppointmentJson);

        /// <summary>
        /// Obtiene una respuesta de cita cancelada de ejemplo.
        /// </summary>
        public static JsonElement GetCancelledAppointment()
            => JsonSerializer.Deserialize<JsonElement>(CancelledAppointmentJson);
    }
}

