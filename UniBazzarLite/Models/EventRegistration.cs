using System.ComponentModel.DataAnnotations;

namespace UniBazaarLite.Models
{
    public class EventRegistration
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid EventId { get; set; }

        [Required, StringLength(60)]
        public string AttendeeName { get; set; } = default!;

        [Required, EmailAddress]
        public string AttendeeEmail { get; set; } = default!;

        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    }
}
