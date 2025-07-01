using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using UniBazzarLite.Data;
using UniBazzarLite.Models;

namespace UniBazaarLite.Pages.Events
{
    public class IndexModel : PageModel
    {
        private readonly IEventRepository _repo;
        public IndexModel(IEventRepository repo) => _repo = repo;

        public IEnumerable<Event> Events { get; private set; } = [];

        public void OnGet() => Events = _repo.GetAll();
    }
}
