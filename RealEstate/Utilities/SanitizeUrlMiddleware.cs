using System.Text.RegularExpressions;

namespace RealEstate.Utilities
{
    public class SanitizeUrlMiddleware
    {
        private readonly RequestDelegate _next;

        public SanitizeUrlMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var originalUrl = context.Request.Path.Value;

            // Check for invalid characters (e.g., %E2%81%A9 or other unwanted Unicode characters)
            if (originalUrl != null && ContainsInvalidCharacters(originalUrl))
            {
                var sanitizedUrl = SanitizeUrl(originalUrl);

                // Redirect to the sanitized URL
                context.Response.Redirect(sanitizedUrl);
                return;
            }

            await _next(context);
        }

        private bool ContainsInvalidCharacters(string url)
        {
            // Define invalid character detection logic
            return Regex.IsMatch(url, @"[\u2060-\u206F]"); // Match Unicode control characters
        }

        private string SanitizeUrl(string url)
        {
            // Remove unwanted Unicode characters
            return Regex.Replace(url, @"[\u2060-\u206F]", string.Empty);
        }
    }
}
