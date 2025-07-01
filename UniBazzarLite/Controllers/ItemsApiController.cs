using System;
using Microsoft.AspNetCore.Mvc;
using UniBazaarLite.Data;
using UniBazaarLite.Models;

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
        public IActionResult Post([FromBody] ClassifiedItem dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            dto.SellerId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            dto.SellerEmail = "student@example.edu";

            _repo.Add(dto);
            return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
        }
    }
}
