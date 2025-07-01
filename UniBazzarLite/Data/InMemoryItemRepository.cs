using System.Collections.Concurrent;
using UniBazaarLite.Models;

namespace UniBazaarLite.Data
{
    public sealed class InMemoryItemRepository : IItemRepository
    {
        private readonly ConcurrentDictionary<Guid, ClassifiedItem> _items = new();

        public InMemoryItemRepository()
        {
            // Add sample data
            var sampleItems = new[]
            {
                new ClassifiedItem
                {
                    Id = Guid.NewGuid(),
                    Title = "MacBook Pro 2021",
                    Description = "Excellent condition MacBook Pro with M1 chip. 16GB RAM, 512GB SSD.",
                    Price = 1200.00m,
                    Category = "Electronics",
                    PostedAt = DateTime.UtcNow.AddDays(-5),
                    SellerEmail = "john.doe@university.edu"
                },
                new ClassifiedItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Calculus Textbook",
                    Description = "Calculus: Early Transcendentals, 8th Edition. Used but in good condition.",
                    Price = 45.00m,
                    Category = "Books",
                    PostedAt = DateTime.UtcNow.AddDays(-3),
                    SellerEmail = "math.student@university.edu"
                },
                new ClassifiedItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Bicycle",
                    Description = "Mountain bike, perfect for campus commuting. Includes lock and helmet.",
                    Price = 150.00m,
                    Category = "Other",
                    PostedAt = DateTime.UtcNow.AddDays(-1),
                    SellerEmail = "bike.lover@university.edu"
                },
                new ClassifiedItem
                {
                    Id = Guid.NewGuid(),
                    Title = "iPhone 13",
                    Description = "iPhone 13, 128GB, Blue. Excellent condition, comes with original box.",
                    Price = 800.00m,
                    Category = "Electronics",
                    PostedAt = DateTime.UtcNow.AddDays(-2),
                    SellerEmail = "tech.seller@university.edu",
                    IsSold = true
                }
            };

            foreach (var item in sampleItems)
            {
                _items[item.Id] = item;
            }
        }

        public IEnumerable<ClassifiedItem> GetAll() => _items.Values.OrderByDescending(i => i.PostedAt);

        public ClassifiedItem? Get(Guid id) => _items.TryGetValue(id, out var i) ? i : null;

        public void Add(ClassifiedItem entity) => _items[entity.Id] = entity;

        public bool Update(ClassifiedItem entity) =>
            _items.TryGetValue(entity.Id, out var _) && _items.TryUpdate(entity.Id, entity, _items[entity.Id]);

        public bool Delete(Guid id) => _items.TryRemove(id, out _);

        public IEnumerable<ClassifiedItem> BySeller(Guid sellerId) =>
            _items.Values.Where(i => i.SellerId == sellerId);

        public IEnumerable<ClassifiedItem> ByCategory(string category) =>
            _items.Values.Where(i => string.Equals(i.Category, category, StringComparison.OrdinalIgnoreCase));
    }
}
