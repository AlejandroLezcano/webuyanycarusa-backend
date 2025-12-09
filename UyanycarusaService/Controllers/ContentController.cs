using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UyanycarusaService.Services;

namespace UyanycarusaService.Controllers
{
    /// <summary>
    /// Controller for dynamic website content (FAQs, landing pages, etc.).
    /// All endpoints require a valid JWT token.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/content")]
    [Authorize]
    [Tags("Content")]
    public class ContentController : ControllerBase
    {
        private readonly IContentService _contentService;
        private readonly ILogger<ContentController> _logger;

        public ContentController(
            IContentService contentService,
            ILogger<ContentController> logger)
        {
            _contentService = contentService;
            _logger = logger;
        }

        // --------------------------------------------------------------------
        // FAQs
        // --------------------------------------------------------------------

        /// <summary>
        /// Retrieves a list of FAQ categories.
        /// </summary>
        [HttpGet("faqs")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> GetFaqs()
        {
            try
            {
                var result = await _contentService.GetFaqsAsync();
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "Communication error calling external FAQ service.");
                return StatusCode(500, new
                {
                    message = "Error communicating with the external FAQ service.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error retrieving FAQs.");
                return StatusCode(500, new
                {
                    message = "Unexpected error while processing the FAQs request.",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Retrieves detailed FAQs for a specific slug.
        /// </summary>
        [HttpGet("faqs/{slug}")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> GetFaqsBySlug(string slug)
        {
            try
            {
                var result = await _contentService.GetFaqsBySlugAsync(slug);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex,
                    "Communication error calling external FAQ service for slug: {Slug}",
                    slug);

                return StatusCode(500, new
                {
                    message = "Error communicating with the external FAQ service.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unexpected error retrieving FAQ detail for slug: {Slug}",
                    slug);

                return StatusCode(500, new
                {
                    message = "Unexpected error while processing the FAQ detail request.",
                    detail = ex.Message
                });
            }
        }

        // --------------------------------------------------------------------
        // Landing Pages
        // --------------------------------------------------------------------

        /// <summary>
        /// Retrieves a list of available landing pages.
        /// </summary>
        [HttpGet("landing-page")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> GetLandingPages()
        {
            try
            {
                var result = await _contentService.GetLandingPagesAsync();
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex,
                    "Communication error calling external landing page service.");

                return StatusCode(500, new
                {
                    message = "Error communicating with the external landing page service.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error retrieving landing pages.");
                return StatusCode(500, new
                {
                    message = "Unexpected error while processing the landing pages request.",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Retrieves the content of a landing page by slug.
        /// </summary>
        [HttpGet("landing-page/{slug}")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonElement>> GetLandingPageBySlug(string slug)
        {
            try
            {
                var result = await _contentService.GetLandingPageBySlugAsync(slug);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex,
                    "Communication error calling external landing page service for slug: {Slug}",
                    slug);

                return StatusCode(500, new
                {
                    message = "Error communicating with the external landing page service.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unexpected error retrieving landing page content for slug: {Slug}",
                    slug);

                return StatusCode(500, new
                {
                    message = "Unexpected error while processing the landing page content request.",
                    detail = ex.Message
                });
            }
        }
    }
}
    