using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using UniBazaarLite.Data;
using UniBazaarLite.Models;
using UniBazaarLite.ViewModels;

namespace UniBazaarLite.Pages.Events;

// Razor Page for listing all events
public class IndexModel : PageModel
{
    private readonly IEventRepository _repo;
    private readonly IOptions<SiteOptions> _opt;

    public IndexModel(IEventRepository repo, IOptions<SiteOptions> opt)
    {
        _repo = repo;
        _opt = opt;
    }

    // List of events to show on the page
    public IEnumerable<Event> Events { get; private set; } = [];
    public int CurrentPage { get; private set; }
    public int TotalPages { get; private set; }

    // Handles GET requests to /Events/Index
    public void OnGet(int page = 1)
    {
        var all = _repo.GetAll().ToList(); // get all events
        int pageSize = _opt.Value.MaxItemsPerPage;

        // Calculate pagination
        TotalPages = (int)Math.Ceiling(all.Count / (double)pageSize);
        CurrentPage = Math.Clamp(page, 1, Math.Max(TotalPages, 1));

        // Only show the events for the current page
        Events = all.Skip((CurrentPage - 1) * pageSize)
                    .Take(pageSize);
    }
}