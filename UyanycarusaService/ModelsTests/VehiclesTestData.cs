namespace UyanycarusaService.ModelsTests
{
    /// <summary>
    /// Datos de prueba para vehículos cuando el servicio externo no responde.
    /// Contiene años, marcas y modelos en un mismo archivo.
    /// </summary>
    public static class VehiclesTestData
    {
        /// <summary>
        /// Lista de años de ejemplo a usar en pruebas o como fallback.
        /// </summary>
        public static readonly IReadOnlyList<int> Years = new List<int>
        {
            2018,
            2019,
            2020,
            2021,
            2022,
            2023,
            2024
        };

        /// <summary>
        /// Lista de marcas de ejemplo a usar en pruebas o como fallback.
        /// </summary>
        public static readonly IReadOnlyList<string> DefaultMakes = new List<string>
        {
            "Toyota",
            "Honda",
            "Ford",
            "Chevrolet",
            "Nissan"
        };

        /// <summary>
        /// Lista de modelos de ejemplo a usar en pruebas o como fallback.
        /// No depende de una marca específica, solo sirve para pruebas.
        /// </summary>
        public static readonly IReadOnlyList<string> DefaultModels = new List<string>
        {
            "Corolla",
            "Civic",
            "Focus",
            "Camaro",
            "Altima"
        };
    }
}


