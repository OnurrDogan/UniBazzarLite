using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UniBazaarLite.Data;
using UniBazaarLite.Filters;
using UniBazaarLite.Models;

namespace UniBazaarLite.Pages.Events;

/// <summary>
/// Herkesin eriþebildiði etkinlik kayýt sayfasý.
/// </summary>
[ServiceFilter(typeof(ValidateEventExistsFilter))]
public class RegisterModel : PageModel
{
    private readonly IEventRepository _repo;
    public RegisterModel(IEventRepository repo) => _repo = repo;

    public Event Event { get; private set; } = default!;

    [BindProperty]
    public EventRegistration Registration { get; set; } = new();

    // GET /Events/Register/{id}
    public IActionResult OnGet(Guid id)
    {
        Event = _repo.Get(id)!;           // filter null'ý engelledi
        Registration.EventId = id;
        return Page();
    }

    // POST /Events/Register
    public IActionResult OnPost()
    {
        Event = _repo.Get(Registration.EventId)!;

        if (!ModelState.IsValid) return Page();

        if (!_repo.Register(Event.Id, Registration))
        {
            TempData["Message"] = "Registration failed – event is full or already registered.";
            return RedirectToPage(new { id = Event.Id });
        }

        TempData["Message"] = "Successfully registered!";
        return RedirectToPage("Index");
    }
}