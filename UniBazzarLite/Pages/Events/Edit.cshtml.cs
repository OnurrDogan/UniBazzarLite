using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UniBazzarLite.Data;
using UniBazzarLite.Models;

namespace UniBazaarLite.Pages.Events
{
    public class EditModel : PageModel
    {
        private readonly IEventRepository _repo;
        public EditModel(IEventRepository repo) => _repo = repo;

        [BindProperty]
        public Event? Event { get; set; }

        public IActionResult OnGet(Guid id)
        {
            Event = _repo.Get(id);
            return Event is null ? NotFound() : Page();
        }

        public IActionResult OnPost()
        {
            if (Event is null || !ModelState.IsValid) return Page();
            if (!_repo.Update(Event)) return NotFound();

            TempData["Message"] = "Event updated!";
            return RedirectToPage("Index");
        }
    }
}
