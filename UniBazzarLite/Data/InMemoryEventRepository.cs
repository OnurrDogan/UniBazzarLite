using System.Collections.Concurrent;
using UniBazaarLite.Models;

namespace UniBazaarLite.Data
{
    public sealed class InMemoryEventRepository : IEventRepository
    {
        private readonly ConcurrentDictionary<Guid, Event> _events = new();

        public InMemoryEventRepository()
        {
            // Initialize with dummy data
            InitializeDummyData();
        }

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

        public IEnumerable<Event> GetAll() => _events.Values.OrderBy(e => e.StartsAt);

        public Event? Get(Guid id) => _events.TryGetValue(id, out var e) ? e : null;

        public void Add(Event entity) => _events[entity.Id] = entity;

        public bool Update(Event entity) =>
            _events.TryGetValue(entity.Id, out var _) && _events.TryUpdate(entity.Id, entity, _events[entity.Id]);

        public bool Delete(Guid id) => _events.TryRemove(id, out _);

        public bool Register(Guid eventId, EventRegistration reg)
        {
            if (!_events.TryGetValue(eventId, out var ev)) return false;
            lock (ev)                      // kapasite ve çifte kayıt koruması
            {
                if (ev.IsFull || ev.Registrations.Any(r => r.AttendeeEmail == reg.AttendeeEmail)) return false;
                ev.Registrations.Add(reg);
                return true;
            }
        }

        public IEnumerable<EventRegistration> GetRegistrations(Guid eventId) =>
            _events.TryGetValue(eventId, out var ev) ? ev.Registrations : Enumerable.Empty<EventRegistration>();

        public IEnumerable<Event> GetAllEvents() => _events.Values;

        public Event GetEventById(Guid id) => _events.TryGetValue(id, out var ev) ? ev : null;
    }
}
