using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UniBazaarLite.Data;
using UniBazaarLite.Models;

namespace UniBazaarLite.Pages.Events
{
    public class RegisterModel : PageModel
    {
        private readonly IEventRepository _eventRepository;

        public RegisterModel(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        [BindProperty]
        public EventRegistration Registration { get; set; }

        public Event Event { get; set; }

        public IActionResult OnGet(Guid id)
        {
            Event = _eventRepository.GetEventById(id);

            if (Event == null)
            {
                TempData["Error"] = "Event not found.";
                return RedirectToPage("Index");
            }

            Registration = new EventRegistration { EventId = id };
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen tüm gerekli alanları doldurun.";
                return Page();
            }

            // TODO: Kayıt işlemini buraya ekle, örn: _eventRepository.Register(Registration);

            TempData["Message"] = "Etkinliğe kaydınız başarıyla tamamlandı!";
            return RedirectToPage("Index");
        }
    }
}
