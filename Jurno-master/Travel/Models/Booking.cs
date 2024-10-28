using System.ComponentModel.DataAnnotations;
using Travel.Models;

public class Booking
{
    [Key]
    public int BookingId { get; set; }
    public DateTime BookingDate { get; set; }
    public bool IsConfirmed { get; set; }

    // Foreign keys
    public int TourId { get; set; }
    public Tour Tour { get; set; }

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; }

    // Stripe payment-related properties
    public string PaymentId { get; set; } = string.Empty; 
    public string PaymentStatus { get; set; } = string.Empty; 
}
