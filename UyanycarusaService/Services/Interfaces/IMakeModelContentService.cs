using System.Text.Json;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Interfaz para el servicio de contenido de marca y modelo
    /// </summary>
    public interface IMakeModelContentService
    {
        /// <summary>
        /// Obtiene el contenido de una marca específica
        /// </summary>
        /// <param name="make">Nombre de la marca</param>
        /// <returns>Respuesta con contenido de la marca como JSON</returns>
        Task<JsonElement> GetMakeContentAsync(string make);

        /// <summary>
        /// Obtiene el contenido de una marca y modelo específicos
        /// </summary>
        /// <param name="make">Nombre de la marca</param>
        /// <param name="model">Nombre del modelo</param>
        /// <returns>Respuesta con contenido de la marca y modelo como JSON</returns>
        Task<JsonElement> GetMakeModelContentAsync(string make, string model);
    }
}

