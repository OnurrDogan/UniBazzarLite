using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace UniBazzarLite.Filters
{
    public sealed class LogActivityFilter : IAsyncActionFilter
    {
        private readonly ILogger<LogActivityFilter> _logger;
        public LogActivityFilter(ILogger<LogActivityFilter> logger) => _logger = logger;

        public async Task OnActionExecutionAsync(ActionExecutingContext context,
                                                 ActionExecutionDelegate next)
        {
            var http = context.HttpContext;
            var sw = Stopwatch.StartNew();

            _logger.LogInformation("[REQ] {method} {path} -> {action}",
                                   http.Request.Method,
                                   http.Request.Path,
                                   context.ActionDescriptor.DisplayName);

            var executedContext = await next();

            sw.Stop();
            _logger.LogInformation("[RES] {status} {elapsed} ms <- {action}",
                                   http.Response.StatusCode,
                                   sw.ElapsedMilliseconds,
                                   context.ActionDescriptor.DisplayName);
        }
    }
}
