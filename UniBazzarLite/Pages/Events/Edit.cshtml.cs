using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UniBazaarLite.Data;
using UniBazaarLite.Filters;
using UniBazaarLite.Models;

namespace UniBazaarLite.Pages.Events;

// Razor Page for editing an existing event
[Authorize]
[ServiceFilter(typeof(ValidateEventExistsFilter))] // Checks if event exists before editing
public class EditModel : PageModel
{
    private readonly IEventRepository _repo;
    public EditModel(IEventRepository repo) => _repo = repo;

    // The event being edited (bound to the form)
    [BindProperty]
    public Event Event { get; set; } = default!;

    // Handles GET requests to /Events/Edit/{id}
    public IActionResult OnGet(Guid id)
    {
        Event? e = _repo.Get(id);
        if (e is null) return NotFound(); // If not found, show 404
        Event = e;
        return Page();
    }

    // Handles POST requests (form submission)
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page(); // If validation fails, show form again

        if (!_repo.Update(Event))
            return NotFound(); // If update fails, show 404

        TempData["Message"] = "Event updated!"; // Show a success message
        return RedirectToPage("Index"); // Go back to the events list
    }

    // Handles POST requests for deleting the event
    public IActionResult OnPostDelete()
    {
        if (Event == null || Event.Id == Guid.Empty)
            return NotFound();
        _repo.Delete(Event.Id);
        TempData["Message"] = "Event deleted.";
        return RedirectToPage("Index");
    }
}