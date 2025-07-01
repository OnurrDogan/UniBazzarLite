using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UniBazaarLite.Data;
using UniBazaarLite.Filters;
using UniBazaarLite.Models;

namespace UniBazaarLite.Pages.Events;

[ServiceFilter(typeof(ValidateEventExistsFilter))]
public class RegisterModel : PageModel
{
    private readonly IEventRepository _repo;
    public RegisterModel(IEventRepository repo) => _repo = repo;

    public Event Event { get; private set; } = default!;

    [BindProperty]
    public EventRegistration Registration { get; set; } = new();

    public IActionResult OnGet(Guid id)
    {
        Event = _repo.Get(id)!;                 // filtre null'ı engelledi
        Registration.EventId = id;
        return Page();
    }

    public IActionResult OnPost()
    {
        // Etkinliği her hâlükârda yükle – ModelState geçersizse de lazım
        Event = _repo.Get(Registration.EventId)!;

        if (!ModelState.IsValid) return Page();

        var ok = _repo.Register(Event.Id, Registration);   // kapasite + duplicate kontrolü

        TempData["Message"] = ok
            ? "Successfully registered!"
            : "Registration failed – event is full or you already registered.";

        return RedirectToPage("Index");
    }
}