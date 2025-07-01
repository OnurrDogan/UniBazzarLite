using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UniBazzarLite.Data;
using UniBazzarLite.Models;

namespace UniBazaarLite.Pages.Events
{
    public class RegisterModel : PageModel
    {
        private readonly IEventRepository _repo;
        public RegisterModel(IEventRepository repo) => _repo = repo;

        public Event? Event { get; private set; }

        [BindProperty]
        public EventRegistration Registration { get; set; } = new();

        public IActionResult OnGet(Guid id)
        {
            Event = _repo.Get(id);
            if (Event is null) return NotFound();

            Registration.EventId = id;
            return Page();
        }

        public IActionResult OnPost()
        {
            Event = _repo.Get(Registration.EventId);
            if (Event is null) return NotFound();

            if (!ModelState.IsValid) return Page();
            if (!_repo.Register(Event.Id, Registration))
            {
                TempData["Error"] = "Registration failed – event is full or you already registered.";
                return RedirectToPage(new { id = Event.Id });
            }

            TempData["Message"] = "Successfully registered!";
            return RedirectToPage("Index");
        }
    }
}
