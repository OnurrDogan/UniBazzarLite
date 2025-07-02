using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UniBazaarLite.Data;
using UniBazaarLite.Filters;
using UniBazaarLite.Models;
using UniBazaarLite.ViewModels;

namespace UniBazaarLite.Controllers;

// Controller for the Classifieds system (MVC style)
[Route("[controller]")]
public class ClassifiedsController : Controller
{
    private readonly IItemRepository _repo;
    private readonly IOptions<SiteOptions> _opt;

    public ClassifiedsController(IItemRepository repo, IOptions<SiteOptions> opt)
    {
        _repo = repo;
        _opt = opt;
    }

    // GET /Classifieds?page=1&category=Books
    // Shows a paginated, optionally filtered list of items
    [HttpGet("")]
    public IActionResult Index(int page = 1, string? category = null)
    {
        int pageSize = _opt.Value.MaxItemsPerPage;

        // Filter by category if provided
        var source = string.IsNullOrWhiteSpace(category)
                     ? _repo.GetAll()
                     : _repo.ByCategory(category);

        var list = source.ToList();
        int totalPages = (int)Math.Ceiling(list.Count / (double)pageSize);
        page = Math.Clamp(page, 1, Math.Max(totalPages, 1));

        var slice = list.Skip((page - 1) * pageSize).Take(pageSize);

        ViewData["CurrentPage"] = page;
        ViewData["TotalPages"] = totalPages;
        ViewData["CurrentCat"] = category ?? "";

        return View(slice);
    }

    // GET /Classifieds/Details/{id}
    // Shows details for a single item
    [HttpGet("Details/{id:guid}")]
    [ServiceFilter(typeof(ValidateItemExistsFilter))]
    public IActionResult Details(Guid id)
    {
        var item = _repo.Get(id)!;      // filter ensures not null
        return View(item);
    }

    // GET /Classifieds/Create
    // Shows the form to create a new item (must be logged in)
    [Authorize]
    [HttpGet("Create")]
    public IActionResult Create() => View(new ClassifiedItem());

    // POST /Classifieds/Create
    // Handles form submission for new item
    [Authorize]
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ClassifiedItem model)
    {
        if (!ModelState.IsValid) return View(model);

        // Set seller info from the current user
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        model.SellerId = idClaim is null ? Guid.Empty : Guid.Parse(idClaim.Value);
        model.SellerEmail = User.Identity!.Name ?? "unknown@local";

        _repo.Add(model);
        TempData["Message"] = "Listing published!";
        return RedirectToAction(nameof(Index));
    }

    // GET /Classifieds/Edit/{id}
    // Shows the form to edit an item (must be logged in)
    [Authorize]
    [HttpGet("Edit/{id:guid}")]
    [ServiceFilter(typeof(ValidateItemExistsFilter))]
    public IActionResult Edit(Guid id)
    {
        var item = _repo.Get(id)!;
        return View(item);
    }

    // POST /Classifieds/Edit
    // Handles form submission for editing an item
    [Authorize]
    [HttpPost("Edit")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(ClassifiedItem model)
    {
        if (!ModelState.IsValid) return View(model);

        var ok = _repo.Update(model);
        TempData["Message"] = ok ? "Listing updated!" : "Update failed!";
        return RedirectToAction(nameof(Index));
    }

    // POST /Classifieds/Delete/{id}
    // Handles deletion of an item (must be logged in)
    [Authorize]
    [HttpPost("Delete/{id:guid}")]
    [ValidateAntiForgeryToken]
    [ServiceFilter(typeof(ValidateItemExistsFilter))]
    public IActionResult Delete(Guid id)
    {
        _repo.Delete(id);
        TempData["Message"] = "Listing deleted.";
        return RedirectToAction(nameof(Index));
    }
}