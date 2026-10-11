using System.Security.Cryptography;
using System.Text;

namespace TopEndLibraryHub.Middleware
{
    public class CatalogueApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public CatalogueApiKeyMiddleware(
            RequestDelegate next,
            IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Path.StartsWithSegments(
                    "/api/catalogue",
                    StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            context.Response.Headers.CacheControl = "no-store";

            string? expectedKey =
                _configuration["CatalogueApi:ApiKey"];

            if (string.IsNullOrWhiteSpace(expectedKey))
            {
                context.Response.StatusCode = 503;

                await context.Response.WriteAsJsonAsync(new
                {
                    error = "Catalogue API is not configured."
                });

                return;
            }

            if (!context.Request.Headers.TryGetValue(
                    "X-API-Key", out var suppliedValues))
            {
                context.Response.StatusCode = 401;

                await context.Response.WriteAsJsonAsync(new
                {
                    error = "API key required."
                });

                return;
            }

            string? suppliedKey = suppliedValues.ToString();

            if (string.IsNullOrWhiteSpace(suppliedKey))
            {
                context.Response.StatusCode = 401;

                await context.Response.WriteAsJsonAsync(new
                {
                    error = "Invalid API key."
                });

                return;
            }

            byte[] expectedBytes =
                Encoding.UTF8.GetBytes(expectedKey);

            byte[] suppliedBytes =
                Encoding.UTF8.GetBytes(suppliedKey);

            bool valid =
                expectedBytes.Length == suppliedBytes.Length &&
                CryptographicOperations.FixedTimeEquals(
                    expectedBytes,
                    suppliedBytes);

            if (!valid)
            {
                context.Response.StatusCode = 401;

                await context.Response.WriteAsJsonAsync(new
                {
                    error = "Invalid API key."
                });

                return;
            }

            await _next(context);
        }
    }
}