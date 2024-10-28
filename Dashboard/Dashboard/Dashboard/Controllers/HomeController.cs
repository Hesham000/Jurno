using Microsoft.AspNetCore.Mvc;
using Travel.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Travel.Controllers
{
    public class HomeController : Controller
    {
        private readonly TravelDbContext _context;

        public HomeController(TravelDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalTours = _context.Tours.Count();
            var totalBookings = _context.Bookings.Count();
            var confirmedBookings = _context.Bookings.Count(b => b.IsConfirmed);

            ViewBag.TotalTours = totalTours;
            ViewBag.TotalBookings = totalBookings;
            ViewBag.ConfirmedBookings = confirmedBookings;

            return View();
        }
    }
}
