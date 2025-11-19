using System.Text.Json;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Interfaz para el servicio de contenido de sucursales
    /// </summary>
    public interface IBranchContentService
    {
        /// <summary>
        /// Obtiene la lista de sucursales disponibles
        /// </summary>
        /// <param name="zipCode">Código postal opcional (5 dígitos)</param>
        /// <param name="limit">Límite de resultados opcional</param>
        /// <param name="branchType">Tipo de sucursal opcional (Physical, Mobile, All)</param>
        /// <returns>Respuesta con lista de sucursales como JSON</returns>
        Task<JsonElement> GetBranchesAsync(string? zipCode = null, int? limit = null, string? branchType = null);

        /// <summary>
        /// Obtiene el detalle de una sucursal específica
        /// </summary>
        /// <param name="branchId">ID de la sucursal</param>
        /// <returns>Respuesta con detalle de la sucursal como JSON</returns>
        Task<JsonElement> GetBranchDetailAsync(int branchId);
    }
}

