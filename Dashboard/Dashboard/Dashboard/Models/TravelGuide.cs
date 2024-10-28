using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Travel.Models
{
    public class TravelGuide
    {
        [Key]
        public int DestinationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public List<string> Activities { get; set; } = new List<string>();
        public List<string> Hotels { get; set; } = new List<string>();
        public List<string> Restaurants { get; set; } = new List<string>();
    }
}
