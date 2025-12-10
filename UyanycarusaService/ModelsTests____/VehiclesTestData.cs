namespace UyanycarusaService.ModelsTests
{
    /// <summary>
    /// Test data for vehicles when external service doesn't respond.
    /// Contains years, makes, and models in a single file.
    /// </summary>
    public static class VehiclesTestData
    {
        /// <summary>
        /// Sample list of years to use in tests or as fallback.
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
        /// Sample list of makes to use in tests or as fallback.
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
        /// Sample list of models to use in tests or as fallback.
        /// Not dependent on a specific make, only for testing purposes.
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


