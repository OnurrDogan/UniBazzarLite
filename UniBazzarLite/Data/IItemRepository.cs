using UniBazaarLite.Models;

namespace UniBazaarLite.Data
{
    public interface IItemRepository
    {
        IEnumerable<ClassifiedItem> GetAll();
        ClassifiedItem? Get(Guid id);

        void Add(ClassifiedItem entity);
        bool Update(ClassifiedItem entity);
        bool Delete(Guid id);

        // Basit filtreleme örnekleri
        IEnumerable<ClassifiedItem> BySeller(Guid sellerId);
        IEnumerable<ClassifiedItem> ByCategory(string category);
    }
}
