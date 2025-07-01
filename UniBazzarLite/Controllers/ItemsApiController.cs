using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using UniBazaarLite.Data;
using UniBazaarLite.Models;
using UniBazaarLite.ViewModels;

namespace UniBazaarLite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsApiController : ControllerBase
    {
        private readonly IItemRepository _repo;
        public ItemsApiController(IItemRepository repo) => _repo = repo;

        // GET /api/items
        [HttpGet]
        public IActionResult GetAll() => Ok(_repo.GetAll());

        // GET /api/items/{id}
        [HttpGet("{id:guid}")]
        public IActionResult Get(Guid id)
        {
            var item = _repo.Get(id);
            return item is null ? NotFound() : Ok(item);
        }

        // POST /api/items
        [HttpPost]
        public IActionResult Post([FromBody] ClassifiedItemCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var item = new ClassifiedItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                Category = dto.Category,
                SellerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value),
                SellerEmail = User.Identity!.Name
            };

            _repo.Add(item);
            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }
    }
}
