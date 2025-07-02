using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UniBazaarLite.Data;
using UniBazaarLite.Models;

namespace UniBazaarLite.Pages.Events;

// Razor Page for creating a new event
[Authorize] // Only logged-in users can create events
public class CreateModel : PageModel
{
    private readonly IEventRepository _repo;
    public CreateModel(IEventRepository repo) => _repo = repo;

    // The event being created (bound to the form)
    [BindProperty] public Event Event { get; set; } = new();

    // Handles POST requests (form submission)
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page(); // If validation fails, show form again

        _repo.Add(Event); // Add the new event to the repository
        TempData["Message"] = "Event created!"; // Show a success message
        return RedirectToPage("Index"); // Go back to the events list
    }
}