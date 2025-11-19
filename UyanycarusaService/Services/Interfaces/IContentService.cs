using System.Text.Json;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Interfaz para el servicio de contenido
    /// </summary>
    public interface IContentService
    {
        /// <summary>
        /// Obtiene la lista de categorías de FAQs
        /// </summary>
        /// <returns>Respuesta con lista de categorías de FAQs como JSON</returns>
        Task<JsonElement> GetFaqsAsync();

        /// <summary>
        /// Obtiene el detalle de FAQs por slug
        /// </summary>
        /// <param name="slug">Slug de la categoría de FAQ</param>
        /// <returns>Respuesta con FAQs detalladas como JSON</returns>
        Task<JsonElement> GetFaqsBySlugAsync(string slug);

        /// <summary>
        /// Obtiene la lista de páginas de landing disponibles
        /// </summary>
        /// <returns>Respuesta con lista de páginas de landing como JSON</returns>
        Task<JsonElement> GetLandingPagesAsync();

        /// <summary>
        /// Obtiene el contenido de una página de landing por slug
        /// </summary>
        /// <param name="slug">Slug de la página de landing</param>
        /// <returns>Respuesta con contenido de la página de landing como JSON</returns>
        Task<JsonElement> GetLandingPageBySlugAsync(string slug);
    }
}

