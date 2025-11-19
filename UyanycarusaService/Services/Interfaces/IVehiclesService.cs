namespace UyanycarusaService.Services
{
    /// <summary>
    /// Interfaz para el servicio de vehículos
    /// </summary>
    public interface IVehiclesService
    {
        /// <summary>
        /// Obtiene la lista de años disponibles de vehículos desde el servicio externo
        /// </summary>
        /// <returns>Lista de años disponibles</returns>
        Task<List<int>> GetYearsAsync();

        /// <summary>
        /// Obtiene la lista de marcas disponibles para un año específico desde el servicio externo
        /// </summary>
        /// <param name="year">Año del vehículo</param>
        /// <returns>Lista de marcas disponibles</returns>
        Task<List<string>> GetMakesAsync(int year);

        /// <summary>
        /// Obtiene la lista de modelos disponibles para un año y marca específicos desde el servicio externo
        /// </summary>
        /// <param name="year">Año del vehículo</param>
        /// <param name="make">Marca del vehículo</param>
        /// <returns>Lista de modelos disponibles</returns>
        Task<List<string>> GetModelsAsync(int year, string make);
    }
}

