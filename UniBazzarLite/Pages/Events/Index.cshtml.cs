using Microsoft.AspNetCore.Mvc.RazorPages;
using UniBazaarLite.Data;
using UniBazaarLite.Models;

namespace UniBazaarLite.Pages.Events
{
    public class IndexModel : PageModel
    {
        private readonly IEventRepository _repo;

        public IndexModel(IEventRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<Event> Events { get; set; }

        public void OnGet()
        {
            Events = _repo.GetAllEvents();
        }
    }
}
