using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniBazaarLite.Data;
using UniBazaarLite.Filters;
using UniBazaarLite.Models;

namespace UniBazaarLite.Controllers;

/// <summary>
/// MVC tarafı: ilan listeleme, detay ve oluşturma.
/// </summary>
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
    [ServiceFilter(typeof(ValidateItemExistsFilter))]
    public IActionResult Details(Guid id)
    {
        var item = _repo.Get(id)!;                // filter sayesinde null olamaz
        return View(item);
    }

    // GET /Classifieds/Create
    [Authorize]
    [HttpGet("Create")]
    public IActionResult Create() => View(new ClassifiedItem());

    // POST /Classifieds/Create
    [Authorize]
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ClassifiedItem model)
    {
        if (!ModelState.IsValid) return View(model);

        // sahte kullanıcı bilgisi (middleware'den de alınabilir)
        model.SellerId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
        model.SellerEmail = User.Identity!.Name!;

        _repo.Add(model);
        TempData["Message"] = "Listing published!";
        return RedirectToAction(nameof(Index));
    }
}