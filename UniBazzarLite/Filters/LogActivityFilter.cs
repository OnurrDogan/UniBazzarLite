using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace UniBazaarLite.Filters
{
    // Custom filter that logs info about every request (for debugging and demo)
    public sealed class LogActivityFilter : IAsyncActionFilter
    {
        private readonly ILogger<LogActivityFilter> _logger;
        public LogActivityFilter(ILogger<LogActivityFilter> logger) => _logger = logger;

        // This runs before and after every action/page
        public async Task OnActionExecutionAsync(ActionExecutingContext context,
                                                 ActionExecutionDelegate next)
        {
            var http = context.HttpContext;
            var sw = Stopwatch.StartNew(); // Start timing

            // Log the incoming request
            _logger.LogInformation("[REQ] {method} {path} -> {action}",
                                   http.Request.Method,
                                   http.Request.Path,
                                   context.ActionDescriptor.DisplayName);

            var executedContext = await next(); // Run the action/page

            sw.Stop();
            // Log the response and how long it took
            _logger.LogInformation("[RES] {status} {elapsed} ms <- {action}",
                                   http.Response.StatusCode,
                                   sw.ElapsedMilliseconds,
                                   context.ActionDescriptor.DisplayName);
        }
    }
}
