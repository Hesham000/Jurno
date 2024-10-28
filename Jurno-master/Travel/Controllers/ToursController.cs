using Microsoft.AspNetCore.Mvc;
using Travel.Models;
using Travel.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Travel.Controllers
{
    public class ToursController : Controller
    {
        private readonly int PageSize = 9;
        private readonly IConfiguration _configuration;
        private readonly TravelDbContext _context;

        public ToursController(TravelDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Index(
            int page = 1,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            string location = null,
            int? minDuration = null,
            int? maxDuration = null)
        {
            ViewBag.PageTitle = "Tours";

            var toursQuery = _context.Tours.AsQueryable();

            if (minPrice.HasValue)
            {
                toursQuery = toursQuery.Where(t => t.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                toursQuery = toursQuery.Where(t => t.Price <= maxPrice.Value);
            }
            if (!string.IsNullOrEmpty(location))
            {
                toursQuery = toursQuery.Where(t => t.Location.Equals(location, System.StringComparison.OrdinalIgnoreCase));
            }
            if (minDuration.HasValue)
            {
                toursQuery = toursQuery.Where(t => t.DurationDays >= minDuration.Value);
            }
            if (maxDuration.HasValue)
            {
                toursQuery = toursQuery.Where(t => t.DurationDays <= maxDuration.Value);
            }

            var totalTours = toursQuery.Count();
            var paginatedTours = toursQuery
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            var viewModel = new TourPaginationViewModel
            {
                Tours = paginatedTours,
                CurrentPage = page,
                TotalPages = (int)System.Math.Ceiling(totalTours / (double)PageSize)
            };

            return View(viewModel);
        }

        public IActionResult Details(int id)
        {
            var tour = _context.Tours.FirstOrDefault(t => t.Id == id);

            if (tour == null)
            {
                return NotFound();
            }

            ViewBag.MapboxToken = _configuration["Mapbox:AccessToken"];

            return View(tour);
        }
    }
}
