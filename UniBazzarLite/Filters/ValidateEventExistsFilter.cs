using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using UniBazaarLite.Data;

namespace UniBazaarLite.Filters
{
    public sealed class ValidateEventExistsFilter : IAsyncActionFilter
    {
        private readonly IEventRepository _repo;
        public ValidateEventExistsFilter(IEventRepository repo) => _repo = repo;

        public async Task OnActionExecutionAsync(ActionExecutingContext context,
                                                 ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue("id", out var raw) && raw is Guid id)
            {
                if (_repo.Get(id) is null)
                {
                    context.Result = new NotFoundResult();
                    return;
                }
            }

            await next();
        }
    }
}
