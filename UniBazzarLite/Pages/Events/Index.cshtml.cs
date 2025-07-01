using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using UniBazaarLite.Data;
using UniBazaarLite.Models;
using UniBazaarLite.ViewModels;

namespace UniBazaarLite.Pages.Events;

public class IndexModel : PageModel
{
    private readonly IEventRepository _repo;
    private readonly IOptions<SiteOptions> _opt;

    public IndexModel(IEventRepository repo, IOptions<SiteOptions> opt)
    {
        _repo = repo;
        _opt = opt;
    }

    public IEnumerable<Event> Events { get; private set; } = [];
    public int CurrentPage { get; private set; }
    public int TotalPages { get; private set; }

    public void OnGet(int page = 1)
    {
        var all = _repo.GetAll().ToList();               // kronolojik sýrada
        int pageSize = _opt.Value.MaxItemsPerPage;

        TotalPages = (int)Math.Ceiling(all.Count / (double)pageSize);
        CurrentPage = Math.Clamp(page, 1, Math.Max(TotalPages, 1));

        Events = all.Skip((CurrentPage - 1) * pageSize)
                    .Take(pageSize);
    }
}