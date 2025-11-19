
using System.Net;
using System.Text.Json;

namespace UyanycarusaService.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var problem = new
                {
                    type = "https://httpstatuses.com/500",
                    title = "Internal Server Error",
                    status = 500,
                    traceId = context.TraceIdentifier,
                    detail = appSafeMessage(ex)
                };

                var json = JsonSerializer.Serialize(problem);
                await context.Response.WriteAsync(json);
            }
        }

        private static string appSafeMessage(Exception ex)
        {
            // Evita filtrar detalles sensibles en producción
            return "An unhandled error occurred. Please try again later.";
        }
    }
}
