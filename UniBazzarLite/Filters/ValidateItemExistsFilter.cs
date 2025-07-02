using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using UniBazaarLite.Data;

namespace UniBazaarLite.Filters
{
    // Custom filter: checks if a classified item exists before showing/editing it
    public sealed class ValidateItemExistsFilter : IAsyncActionFilter
    {
        private readonly IItemRepository _repo;
        public ValidateItemExistsFilter(IItemRepository repo) => _repo = repo;

        // Runs before the action/page
        public async Task OnActionExecutionAsync(ActionExecutingContext context,
                                                 ActionExecutionDelegate next)
        {
            // Check if the 'id' parameter is present and valid
            if (context.ActionArguments.TryGetValue("id", out var raw) && raw is Guid id)
            {
                // If the item doesn't exist, return 404
                if (_repo.Get(id) is null)
                {
                    context.Result = new NotFoundResult();
                    return;
                }
            }

            await next(); // Continue if valid
        }
    }
}
