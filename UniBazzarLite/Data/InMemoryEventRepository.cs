using System.Collections.Concurrent;
using UniBazaarLite.Models;

namespace UniBazaarLite.Data
{
    // In-memory repository for events (no database needed!)
    public sealed class InMemoryEventRepository : IEventRepository
    {
        // Stores all events in a thread-safe dictionary
        private readonly ConcurrentDictionary<Guid, Event> _events = new();

        // Constructor: adds some sample events when the app starts
        public InMemoryEventRepository()
        {
            // Initialize with dummy data
            InitializeDummyData();
        }

        // Adds some example events for demo/testing
        private void InitializeDummyData()
        {
            var events = new[]
            {
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Spring Campus Festival",
                    Description = "Join us for a day of music, food, and fun! Live bands, food trucks, and activities for all students.",
                    StartsAt = DateTime.Now.AddDays(7),
                    EndsAt = DateTime.Now.AddDays(7).AddHours(6),
                    Location = "Main Campus Quad",
                    Capacity = 500
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Tech Career Fair",
                    Description = "Meet with top tech companies and explore internship and job opportunities. Bring your resume!",
                    StartsAt = DateTime.Now.AddDays(14),
                    EndsAt = DateTime.Now.AddDays(14).AddHours(4),
                    Location = "Engineering Building, Room 201",
                    Capacity = 200
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Study Abroad Information Session",
                    Description = "Learn about study abroad opportunities for next semester. Representatives from partner universities will be present.",
                    StartsAt = DateTime.Now.AddDays(3),
                    EndsAt = DateTime.Now.AddDays(3).AddHours(2),
                    Location = "International Center",
                    Capacity = 100
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Basketball Tournament",
                    Description = "Annual intramural basketball tournament. Teams of 5 players. Registration required.",
                    StartsAt = DateTime.Now.AddDays(21),
                    EndsAt = DateTime.Now.AddDays(21).AddHours(8),
                    Location = "University Gymnasium",
                    Capacity = 80
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Academic Writing Workshop",
                    Description = "Improve your academic writing skills. Learn about research methods, citation styles, and essay structure.",
                    StartsAt = DateTime.Now.AddDays(5),
                    EndsAt = DateTime.Now.AddDays(5).AddHours(3),
                    Location = "Library Conference Room",
                    Capacity = 50
                }
            };

            foreach (var evt in events)
            {
                _events[evt.Id] = evt;
            }
        }

        // Returns all events, sorted by start date
        public IEnumerable<Event> GetAll() => _events.Values.OrderBy(e => e.StartsAt);

        // Get a single event by ID
        public Event? Get(Guid id) => _events.TryGetValue(id, out var e) ? e : null;

        // Add a new event
        public void Add(Event entity) => _events[entity.Id] = entity;

        // Update an existing event
        public bool Update(Event entity) =>
            _events.TryGetValue(entity.Id, out var _) && _events.TryUpdate(entity.Id, entity, _events[entity.Id]);

        // Delete an event by ID
        public bool Delete(Guid id) => _events.TryRemove(id, out _);

        // Register a student for an event
        public bool Register(Guid eventId, EventRegistration reg)
        {
            if (!_events.TryGetValue(eventId, out var ev)) return false;
            lock (ev) // lock so two people can't register at the same time
            {
                // Check if event is full or already registered
                if (ev.IsFull || ev.Registrations.Any(r => r.AttendeeEmail == reg.AttendeeEmail)) return false;
                ev.Registrations.Add(reg);
                return true;
            }
        }

        // Get all registrations for an event
        public IEnumerable<EventRegistration> GetRegistrations(Guid eventId) =>
            _events.TryGetValue(eventId, out var ev) ? ev.Registrations : Enumerable.Empty<EventRegistration>();

        // (Legacy) Get all events
        public IEnumerable<Event> GetAllEvents() => _events.Values;

        // (Legacy) Get event by ID
        public Event GetEventById(Guid id) => _events.TryGetValue(id, out var ev) ? ev : null;
    }
}
