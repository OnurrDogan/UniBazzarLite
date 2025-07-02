using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UniBazaarLite.Data;
using UniBazaarLite.Filters;
using UniBazaarLite.Models;

namespace UniBazaarLite.Pages.Events;

// Razor Page for registering for an event
[ServiceFilter(typeof(ValidateEventExistsFilter))] // Checks if event exists before registering
public class RegisterModel : PageModel
{
    private readonly IEventRepository _repo;
    public RegisterModel(IEventRepository repo) => _repo = repo;

    // The event being registered for
    public Event Event { get; private set; } = default!;

    // The registration info (bound to the form)
    [BindProperty]
    public EventRegistration Registration { get; set; } = new();

    // Handles GET requests to /Events/Register/{id}
    public IActionResult OnGet(Guid id)
    {
        Event = _repo.Get(id)!; // filter ensures not null
        Registration.EventId = id;
        return Page();
    }

    // Handles POST requests (form submission)
    public IActionResult OnPost()
    {
        // Always load the event (even if validation fails)
        Event = _repo.Get(Registration.EventId)!;

        if (!ModelState.IsValid) return Page(); // If validation fails, show form again

        // Try to register (checks capacity and duplicates)
        var ok = _repo.Register(Event.Id, Registration);

        TempData["Message"] = ok
            ? "Successfully registered!"
            : "Registration failed – event is full or you already registered.";

        return RedirectToPage("Index"); // Go back to the events list
    }
}