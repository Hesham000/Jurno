using System.ComponentModel.DataAnnotations;

namespace Travel.Models
{
    public class TravelGuide
    {
        [Key]
        public int DestinationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public List<string> Activities { get; set; } = new List<string>();
        public List<string> Hotels { get; set; } = new List<string>();
        public List<string> Restaurants { get; set; } = new List<string>();
    }
}
