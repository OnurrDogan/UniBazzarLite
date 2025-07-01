using System;
using Microsoft.AspNetCore.Mvc;
using UniBazzarLite.Data;
using UniBazzarLite.Models;

namespace UniBazaarLite.Controllers
{
    [Route("[controller]")]
    public class ClassifiedsController : Controller
    {
        private readonly IItemRepository _repo;
        public ClassifiedsController(IItemRepository repo) => _repo = repo;

        // GET /Classifieds
        [HttpGet("")]
        public IActionResult Index() => View(_repo.GetAll());

        // GET /Classifieds/Details/{id}
        [HttpGet("Details/{id:guid}")]
        public IActionResult Details(Guid id)
        {
            var item = _repo.Get(id);
            return item is null ? NotFound() : View(item);
        }

        // GET /Classifieds/Create
        [HttpGet("Create")]
        public IActionResult Create() => View(new ClassifiedItem());

        // POST /Classifieds/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ClassifiedItem model)
        {
            if (!ModelState.IsValid) return View(model);

            // Simüle edilmiş “giriş yapmış kullanıcı”
            model.SellerId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            model.SellerEmail = "student@example.edu";

            _repo.Add(model);
            TempData["Message"] = "Listing published!";
            return RedirectToAction(nameof(Index));
        }
    }
}
