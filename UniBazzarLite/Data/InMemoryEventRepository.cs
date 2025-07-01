using System.Collections.Concurrent;
using UniBazaarLite.Models;

namespace UniBazaarLite.Data
{
    public sealed class InMemoryEventRepository : IEventRepository
    {
        private readonly ConcurrentDictionary<Guid, Event> _events = new();

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
