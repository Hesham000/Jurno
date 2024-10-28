using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Travel.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
