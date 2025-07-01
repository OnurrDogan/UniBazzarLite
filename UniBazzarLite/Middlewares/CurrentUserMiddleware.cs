using System.Security.Claims;

namespace UniBazaarLite.Middlewares;

public class CurrentUserMiddleware
{
    private readonly RequestDelegate _next;
    public CurrentUserMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext ctx)
    {
        if (!ctx.User.Identity?.IsAuthenticated ?? true)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "11111111-1111-1111-1111-111111111111"), // <<< EKLENDÝ
                new Claim(ClaimTypes.Name,  "demo@unibazaar.local"),
                new Claim(ClaimTypes.Email, "demo@unibazaar.local"),
                new Claim(ClaimTypes.Role,  "Student")
            };
            ctx.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "Fake"));
        }

        await _next(ctx);
    }
}