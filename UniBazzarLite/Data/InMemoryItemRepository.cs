using System.Collections.Concurrent;
using UniBazaarLite.Models;

namespace UniBazaarLite.Data
{
    public sealed class InMemoryItemRepository : IItemRepository
    {
        private readonly ConcurrentDictionary<Guid, ClassifiedItem> _items = new();

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
