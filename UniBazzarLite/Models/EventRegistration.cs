using System.ComponentModel.DataAnnotations;

namespace UniBazaarLite.Models;

// Represents a student's registration for an event
public class EventRegistration
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // The event this registration is for
    [Required]
    public Guid EventId { get; set; }

    [Required, StringLength(100)]
    public string AttendeeName { get; set; } = string.Empty; // Student's name

    [Required, EmailAddress]
    public string AttendeeEmail { get; set; } = string.Empty; // Student's email

    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
}
