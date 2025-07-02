using System.Security.Claims;

namespace UniBazaarLite.Middlewares;

// Middleware to simulate a logged-in user for demo/testing
public class CurrentUserMiddleware
{
    private readonly RequestDelegate _next;
    public CurrentUserMiddleware(RequestDelegate next) => _next = next;

    // This runs on every HTTP request
    public async Task InvokeAsync(HttpContext ctx)
    {
        // If the user is not authenticated, fake a user
        if (!ctx.User.Identity?.IsAuthenticated ?? true)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "11111111-1111-1111-1111-111111111111"), // Fake user ID
                new Claim(ClaimTypes.Name,  "demo@unibazaar.local"), // Fake email
                new Claim(ClaimTypes.Email, "demo@unibazaar.local"),
                new Claim(ClaimTypes.Role,  "Student")
            };
            ctx.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "Fake"));
        }

        await _next(ctx); // Continue to the next middleware/request handler
    }
}