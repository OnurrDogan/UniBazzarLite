using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UniBazaarLite.Data;
using UniBazaarLite.Models;

namespace UniBazaarLite.Pages.Events;

[Authorize]
public class CreateModel : PageModel
{
    private readonly IEventRepository _repo;
    public CreateModel(IEventRepository repo) => _repo = repo;

    [BindProperty] public Event Event { get; set; } = new();

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();

        _repo.Add(Event);
        TempData["Message"] = "Event created!";
        return RedirectToPage("Index");
    }
}