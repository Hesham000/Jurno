using System;
using System.ComponentModel.DataAnnotations;

namespace Travel.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        public DateTime BookingDate { get; set; }
        public bool IsConfirmed { get; set; }

        public int TourId { get; set; }
        public Tour Tour { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string PaymentId { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
    }
}
