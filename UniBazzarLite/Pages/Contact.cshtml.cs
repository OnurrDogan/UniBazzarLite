using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace UniBazaarLite.Pages;

// Razor Page for the Contact page (static info + contact form)
public class ContactModel : PageModel
{
    private readonly IConfiguration _config;
    public string? AdminEmail { get; private set; }

    public ContactModel(IConfiguration config) => _config = config;

    // The contact form data (bound to the form)
    [BindProperty]
    public ContactFormModel ContactForm { get; set; } = new();

    // Handles GET requests to /Contact
    public void OnGet()
    {
        AdminEmail = _config["AdminEmail"];
    }

    // Handles POST requests (form submission)
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page(); // If validation fails, show form again
        }

        // In a real app, you'd send an email here!
        TempData["Message"] = $"Thank you for your message, {ContactForm.Name}! We'll get back to you at {ContactForm.Email} soon.";
        return RedirectToPage(); // Show the success message
    }
}

// Model for the contact form
public class ContactFormModel
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Message is required")]
    [StringLength(1000, ErrorMessage = "Message cannot be longer than 1000 characters")]
    public string Message { get; set; } = string.Empty;
}