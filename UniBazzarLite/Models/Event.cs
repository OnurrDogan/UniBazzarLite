using System.ComponentModel.DataAnnotations;

namespace UniBazaarLite.Models
{
    // Represents a campus event (like a festival, workshop, etc.)
    public class Event
    {
        // Unique ID for the event
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(100)]
        public string Title { get; set; } = default!; // Event name

        [StringLength(2000)]
        public string? Description { get; set; } // Details about the event

        [Required, DataType(DataType.DateTime)]
        [NotInPast] // Custom validation: can't be before today
        public DateTime StartsAt { get; set; } = DateTime.Now.Date; // When it starts

        [DataType(DataType.DateTime)]
        public DateTime? EndsAt { get; set; } // When it ends (optional)

        [Required, StringLength(200)]
        public string Location { get; set; } = default!; // Where it happens

        /// <summary>Event capacity (0 = unlimited)</summary>
        [Range(0, 10000)]
        public int Capacity { get; set; } = 0;

        // List of people registered for this event
        public List<EventRegistration> Registrations { get; set; } = [];

        // Is the event full?
        public bool IsFull => Capacity > 0 && Registrations.Count >= Capacity;
    }

    // Custom validation: event can't be in the past
    public class NotInPastAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime dt)
            {
                if (dt.Date < DateTime.Now.Date)
                {
                    return new ValidationResult("Event date cannot be in the past.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
