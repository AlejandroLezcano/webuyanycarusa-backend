namespace UyanycarusaService.Dtos
{
    /// <summary>
    /// Modelo para enviar un SMS
    /// </summary>
    public class SmsSendModel
    {
        /// <summary>
        /// ID del vehículo del cliente
        /// </summary>
        public int CustomerVehicleId { get; set; }

        /// <summary>
        /// Número de teléfono del destinatario
        /// </summary>
        public string Recipient { get; set; } = string.Empty;

        /// <summary>
        /// Mensaje a enviar
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}

