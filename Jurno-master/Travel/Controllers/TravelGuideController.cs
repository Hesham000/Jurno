using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Travel.Models;

namespace Travel.Controllers
{
    public class TravelGuideController : Controller
    {
        private static List<TravelGuide> TravelGuides = new List<TravelGuide>
        {
              new TravelGuide
    {
        DestinationId = 1,
        Name = "Thailand",
        Description = "Tropical beaches, opulent royal palaces, ancient ruins, and ornate temples.",
        ImageUrl = "/img/Thailand_Featured.webp",
        Activities = new List<string> { "Island hopping", "Scuba diving", "Visiting temples" },
        Hotels = new List<string> { "Siam Kempinski Hotel", "Rayavadee Beach Resort" },
        Restaurants = new List<string> { "Gaggan", "Nahm" }
    },
    new TravelGuide
    {
        DestinationId = 2,
        Name = "Australia",
        Description = "Iconic landmarks and natural wonders, including the Great Barrier Reef and Sydney Opera House.",
        ImageUrl = "https://i.pinimg.com/564x/f6/a6/80/f6a68049dda3b982ef1094424880760b.jpg",
        Activities = new List<string> { "Surfing", "Exploring the Outback", "Snorkeling at the Great Barrier Reef" },
        Hotels = new List<string> { "Park Hyatt Sydney", "Qualia Resort" },
        Restaurants = new List<string> { "Quay", "Attica" }
    },
    new TravelGuide
    {
        DestinationId = 3,
        Name = "Switzerland",
        Description = "Famous for its picturesque alpine scenery, charming villages, and high-end watches.",
        ImageUrl = "https://i.pinimg.com/564x/b4/c2/f8/b4c2f859e61841b0ef552a9a9c1c8325.jpg",
        Activities = new List<string> { "Skiing in Zermatt", "Hiking in the Alps", "Visiting the Swiss Museum of Transport" },
        Hotels = new List<string> { "The Chedi Andermatt", "Badrutt's Palace Hotel" },
        Restaurants = new List<string> { "Restaurant de l’Hôtel de Ville", "Cheval Blanc" }
    },
    new TravelGuide
    {
        DestinationId = 4,
        Name = "Japan",
        Description = "A blend of ancient traditions and futuristic innovations, with beautiful landscapes and cultural heritage.",
        ImageUrl = "https://i.pinimg.com/564x/87/a4/40/87a4409319f1e047f942b43f88824bb5.jpg",
        Activities = new List<string> { "Cherry Blossom Viewing", "Visiting Temples in Kyoto", "Shopping in Shibuya" },
        Hotels = new List<string> { "Aman Tokyo", "The Ritz-Carlton Kyoto" },
        Restaurants = new List<string> { "Sukiyabashi Jiro", "Narisawa" }
    },
    new TravelGuide
    {
        DestinationId = 5,
        Name = "Italy",
        Description = "Known for its rich history, art, fashion, and culinary delights, with stunning coastlines and ancient ruins.",
        ImageUrl = "https://i.pinimg.com/564x/db/b7/8e/dbb78eb5c26aa429f500c93aa2658c79.jpg",
        Activities = new List<string> { "Exploring the Colosseum", "Vatican Museum Tour", "Gondola Ride in Venice" },
        Hotels = new List<string> { "Hotel Danieli Venice", "The St. Regis Rome" },
        Restaurants = new List<string> { "Osteria Francescana", "Da Vittorio" }
    },
    new TravelGuide
    {
        DestinationId = 6,
        Name = "New Zealand",
        Description = "A paradise for nature lovers, with stunning fjords, beaches, and adventure sports opportunities.",
        ImageUrl = "https://i.pinimg.com/564x/0e/f7/63/0ef763379b64ef88f565d1b5a489a5d2.jpg",
        Activities = new List<string> { "Bungee Jumping in Queenstown", "Hiking in Fiordland", "Visiting Hobbiton" },
        Hotels = new List<string> { "Eichardt’s Private Hotel", "Huka Lodge" },
        Restaurants = new List<string> { "Amisfield Bistro", "Clooney" }
    },
    new TravelGuide
    {
        DestinationId = 7,
        Name = "South Africa",
        Description = "A land of diversity, offering wildlife safaris, scenic landscapes, and vibrant cities.",
        ImageUrl = "https://i.pinimg.com/564x/68/f5/c9/68f5c9dd78ecc3ef5209a7e694d528a1.jpg",
        Activities = new List<string> { "Safari in Kruger National Park", "Climbing Table Mountain", "Exploring Cape Town" },
        Hotels = new List<string> { "One&Only Cape Town", "Singita Sabi Sand" },
        Restaurants = new List<string> { "The Test Kitchen", "La Colombe" }
    }
        };

        public IActionResult Index()
        {

            return View(TravelGuides);
        }


        public IActionResult Details(int id)
        {
            var destination = TravelGuides.Find(d => d.DestinationId == id);
            if (destination == null)
            {
                return NotFound();
            }
            return View(destination);
        }
    }
}
