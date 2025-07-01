using System.ComponentModel.DataAnnotations;

namespace UniBazzarLite.Models
{
    public class Event
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(100)]
        public string Title { get; set; } = default!;

        [StringLength(2000)]
        public string? Description { get; set; }

        [Required, DataType(DataType.DateTime)]
        public DateTime StartsAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? EndsAt { get; set; }

        [Required, StringLength(200)]
        public string Location { get; set; } = default!;

        /// <summary>Etkinlik kontenjanı (0 = sınırsız).</summary>
        [Range(0, 10000)]
        public int Capacity { get; set; } = 0;

        public List<EventRegistration> Registrations { get; set; } = [];

        public bool IsFull => Capacity > 0 && Registrations.Count >= Capacity;
    }
}
