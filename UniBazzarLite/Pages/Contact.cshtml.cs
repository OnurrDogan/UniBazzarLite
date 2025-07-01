using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace UniBazaarLite.Pages;
public class ContactModel : PageModel
{
    private readonly IConfiguration _config;
    public string? AdminEmail { get; private set; }

    [BindProperty]
    public ContactFormModel ContactForm { get; set; } = new();

    public ContactModel(IConfiguration config) => _config = config;

    public void OnGet()
    {
        AdminEmail = _config["AdminEmail"];
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // In a real application, you would send an email here
        // For now, we'll just show a success message
        TempData["Message"] = $"Thank you for your message, {ContactForm.Name}! We'll get back to you at {ContactForm.Email} soon.";
        
        return RedirectToPage();
    }
}

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