using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UyanycarusaService.Services;

namespace UyanycarusaService.Controllers
{
    /// <summary>
    /// Controller for branch content operations.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/content")]
    [AllowAnonymous]
    [Tags("BranchContent")]
    public class BranchContentController : ControllerBase
    {
        private readonly IBranchContentService _branchContentService;
        private readonly ILogger<BranchContentController> _logger;

        public BranchContentController(
            IBranchContentService branchContentService,
            ILogger<BranchContentController> logger)
        {
            _branchContentService = branchContentService;
            _logger = logger;
        }

        /// <summary>
        /// Gets the list of available branches.
        /// </summary>
        /// <param name="zipCode">Optional ZIP code (5 digits)</param>
        /// <param name="limit">Optional result limit</param>
        /// <param name="branchType">Optional branch type (Physical, Mobile, All)</param>
        /// <returns>List of branches returned by the external service</returns>
        /// <response code="200">Branches retrieved successfully</response>
        /// <response code="400">Invalid request for the external service</response>
        /// <response code="401">Unauthorized. Valid JWT token required</response>
        /// <response code="500">Error consuming external service</response>
        [HttpGet("branches")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> GetBranches(
            [FromQuery] string? zipCode = null,
            [FromQuery] int? limit = null,
            [FromQuery] string? branchType = null)
        {
            try
            {
                var result = await _branchContentService.GetBranchesAsync(zipCode, limit, branchType);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error calling external branch service");
                return StatusCode(500, new
                {
                    message = "Error communicating with the external branch service",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error processing branch list request");
                return StatusCode(500, new
                {
                    message = "Unexpected error processing branch list request",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Gets branch details for a specific branch ID.
        /// </summary>
        /// <param name="branchId">Branch ID</param>
        /// <returns>Branch details returned by the external service</returns>
        /// <response code="200">Branch details retrieved successfully</response>
        /// <response code="400">Invalid request for the external service</response>
        /// <response code="401">Unauthorized. Valid JWT token required</response>
        /// <response code="404">Branch not found</response>
        /// <response code="500">Error consuming external service</response>
        [HttpGet("branches/{branchId}")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> GetBranchDetail(int branchId)
        {
            try
            {
                var result = await _branchContentService.GetBranchDetailAsync(branchId);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error calling external branch detail service for BranchId {BranchId}", branchId);
                return StatusCode(500, new
                {
                    message = "Error communicating with the external branch detail service",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error processing branch detail request for BranchId {BranchId}", branchId);
                return StatusCode(500, new
                {
                    message = "Unexpected error processing branch detail request",
                    detail = ex.Message
                });
            }
        }
    }
}
