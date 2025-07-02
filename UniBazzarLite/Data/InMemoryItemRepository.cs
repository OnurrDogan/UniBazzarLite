using System.Collections.Concurrent;
using UniBazaarLite.Models;

namespace UniBazaarLite.Data
{
    // In-memory repository for classified items (no database needed!)
    public sealed class InMemoryItemRepository : IItemRepository
    {
        // Stores all items in a thread-safe dictionary
        private readonly ConcurrentDictionary<Guid, ClassifiedItem> _items = new();

        // Constructor: adds some sample items when the app starts
        public InMemoryItemRepository()
        {
            // Add sample data for demo/testing
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
                },
                new ClassifiedItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Gaming Chair",
                    Description = "Ergonomic gaming chair with lumbar support. Perfect for long study sessions.",
                    Price = 200.00m,
                    Category = "Furniture",
                    PostedAt = DateTime.UtcNow.AddDays(-4),
                    SellerEmail = "gamer.student@university.edu"
                },
                new ClassifiedItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Chemistry Lab Kit",
                    Description = "Complete chemistry lab kit with safety equipment. Used for one semester only.",
                    Price = 75.00m,
                    Category = "Lab Equipment",
                    PostedAt = DateTime.UtcNow.AddDays(-6),
                    SellerEmail = "chem.major@university.edu"
                },
                new ClassifiedItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Coffee Maker",
                    Description = "Dorm-friendly coffee maker. Makes 4 cups, perfect for early morning classes.",
                    Price = 25.00m,
                    Category = "Appliances",
                    PostedAt = DateTime.UtcNow.AddDays(-2),
                    SellerEmail = "coffee.lover@university.edu"
                },
                new ClassifiedItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Psychology Textbook Set",
                    Description = "Complete set of psychology textbooks from last semester. All in excellent condition.",
                    Price = 120.00m,
                    Category = "Books",
                    PostedAt = DateTime.UtcNow.AddDays(-7),
                    SellerEmail = "psych.student@university.edu"
                },
                new ClassifiedItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Mini Fridge",
                    Description = "Small dorm refrigerator. Works perfectly, includes freezer compartment.",
                    Price = 80.00m,
                    Category = "Appliances",
                    PostedAt = DateTime.UtcNow.AddDays(-3),
                    SellerEmail = "dorm.resident@university.edu"
                },
                new ClassifiedItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Guitar",
                    Description = "Acoustic guitar, great for beginners. Includes case and extra strings.",
                    Price = 100.00m,
                    Category = "Musical Instruments",
                    PostedAt = DateTime.UtcNow.AddDays(-1),
                    SellerEmail = "music.major@university.edu"
                }
            };

            foreach (var item in sampleItems)
            {
                _items[item.Id] = item;
            }
        }

        // Returns all items, newest first
        public IEnumerable<ClassifiedItem> GetAll() => _items.Values.OrderByDescending(i => i.PostedAt);

        // Get a single item by ID
        public ClassifiedItem? Get(Guid id) => _items.TryGetValue(id, out var i) ? i : null;

        // Add a new item (if category is not allowed, set to 'Other')
        public void Add(ClassifiedItem entity)
        {
            var allowedCategories = new[] { "Electronics", "Books", "Furniture", "Appliances", "Lab Equipment", "Musical Instruments", "Other" };
            if (string.IsNullOrWhiteSpace(entity.Category) || !allowedCategories.Contains(entity.Category, StringComparer.OrdinalIgnoreCase))
            {
                entity.Category = "Other";
            }
            _items[entity.Id] = entity;
        }

        // Update an existing item
        public bool Update(ClassifiedItem entity) =>
            _items.TryGetValue(entity.Id, out var _) && _items.TryUpdate(entity.Id, entity, _items[entity.Id]);

        // Delete an item by ID
        public bool Delete(Guid id) => _items.TryRemove(id, out _);

        // Get all items by a specific seller
        public IEnumerable<ClassifiedItem> BySeller(Guid sellerId) =>
            _items.Values.Where(i => i.SellerId == sellerId);

        // Get all items in a specific category
        public IEnumerable<ClassifiedItem> ByCategory(string category) =>
            _items.Values.Where(i => string.Equals(i.Category, category, StringComparison.OrdinalIgnoreCase));
    }
}
