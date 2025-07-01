using UniBazaarLite.Models;

namespace UniBazaarLite.Data
{
    public interface IEventRepository
    {
        IEnumerable<Event> GetAll();
        Event? Get(Guid id);

        void Add(Event entity);
        bool Update(Event entity);          // true → başarı, false → yok
        bool Delete(Guid id);

        // Kayıt işlemleri
        bool Register(Guid eventId, EventRegistration reg);         // kapasite kontrolü içerir
        IEnumerable<EventRegistration> GetRegistrations(Guid eventId);
    }
}
