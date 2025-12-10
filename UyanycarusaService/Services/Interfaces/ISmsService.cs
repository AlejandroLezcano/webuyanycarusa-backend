using System.Text.Json;
using UyanycarusaService.Dtos;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Interfaz para el servicio de SMS
    /// </summary>
    public interface ISmsService
    {
        /// <summary>
        /// Envía un SMS a través del servicio externo
        /// </summary>
        /// <param name="request">Datos del SMS a enviar</param>
        /// <returns>Respuesta del servicio externo como JSON</returns>
        Task<JsonElement> SendSmsAsync(SmsSendModel request);
    }
}

