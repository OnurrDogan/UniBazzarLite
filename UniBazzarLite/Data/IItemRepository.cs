using UniBazaarLite.Models;

namespace UniBazaarLite.Data
{
    // Interface for item repository (how we access classifieds data)
    public interface IItemRepository
    {
        // Get all items
        IEnumerable<ClassifiedItem> GetAll();
        // Get a single item by ID
        ClassifiedItem? Get(Guid id);

        // Add a new item
        void Add(ClassifiedItem entity);
        // Update an existing item
        bool Update(ClassifiedItem entity);
        // Delete an item by ID
        bool Delete(Guid id);

        // Get all items by a specific seller
        IEnumerable<ClassifiedItem> BySeller(Guid sellerId);
        // Get all items in a specific category
        IEnumerable<ClassifiedItem> ByCategory(string category);
    }
}
