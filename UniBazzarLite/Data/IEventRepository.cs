using UniBazaarLite.Models;

namespace UniBazaarLite.Data
{
    // Interface for event repository (how we access event data)
    public interface IEventRepository
    {
        // Get all events
        IEnumerable<Event> GetAll();
        // Get a single event by ID
        Event? Get(Guid id);

        // Add a new event
        void Add(Event entity);
        // Update an existing event
        bool Update(Event entity);          // true = success, false = not found
        // Delete an event by ID
        bool Delete(Guid id);

        // Register a student for an event (checks capacity)
        bool Register(Guid eventId, EventRegistration reg);
        // Get all registrations for an event
        IEnumerable<EventRegistration> GetRegistrations(Guid eventId);
        // (Legacy) Get all events
        IEnumerable<Event> GetAllEvents();
        // (Legacy) Get event by ID
        Event GetEventById(Guid id);
    }
}
