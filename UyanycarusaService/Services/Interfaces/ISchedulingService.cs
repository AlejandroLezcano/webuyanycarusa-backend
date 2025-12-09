using System.Text.Json;

namespace UyanycarusaService.Services
{
    /// <summary>
    /// Interface for Scheduling service (OTP)
    /// </summary>
    public interface ISchedulingService
    {
        /// <summary>
        /// Requests an OTP code for scheduling
        /// </summary>
        /// <param name="model">OTP request data</param>
        /// <returns>OTP request response as JSON</returns>
        Task<JsonElement> RequestOTPAsync(JsonElement model);
    }
}

