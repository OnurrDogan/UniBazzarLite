using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UniBazaarLite.Data;
using UniBazaarLite.Filters;
using UniBazaarLite.Models;

namespace UniBazaarLite.Pages.Events;

[Authorize]
[ServiceFilter(typeof(ValidateEventExistsFilter))]
public class EditModel : PageModel
{
    private readonly IEventRepository _repo;
    public EditModel(IEventRepository repo) => _repo = repo;

    [BindProperty]
    public Event Event { get; set; } = default!;

    public IActionResult OnGet(Guid id)
    {
        Event? e = _repo.Get(id);
        if (e is null) return NotFound();
        Event = e;
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();

        if (!_repo.Update(Event))
            return NotFound();

        TempData["Message"] = "Event updated!";
        return RedirectToPage("Index");
    }

    public IActionResult OnPostDelete()
    {
        if (Event == null || Event.Id == Guid.Empty)
            return NotFound();
        _repo.Delete(Event.Id);
        TempData["Message"] = "Event deleted.";
        return RedirectToPage("Index");
    }
}