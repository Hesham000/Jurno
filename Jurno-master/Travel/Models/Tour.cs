using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Travel.Models
{
    public class Tour
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public string Description { get; set; }

        [Range(1, 365)]
        public int DurationDays { get; set; }

        public string ImageUrl { get; set; }

        [Range(0, 100)]
        public decimal Discount { get; set; }

        [Range(-90, 90)]
        public double Latitude { get; set; }

        [Range(-180, 180)]
        public double Longitude { get; set; }

        public string CarouselImages { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        // Method to return a list of carousel image URLs
        public List<string> GetCarouselImagesList()
        {
            return string.IsNullOrEmpty(CarouselImages)
                ? new List<string>()
                : CarouselImages.Split(',').Select(img => img.Trim()).ToList();
        }
    }
}
