using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Travel.Data;
using Travel.Models;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Travel.Controllers
{
    public class ToursController : Controller
    {
        private readonly TravelDbContext _context;

        public ToursController(TravelDbContext context)
        {
            _context = context;
        }

        // GET: Tours
        public async Task<IActionResult> Index()
        {
            var tours = await _context.Tours.ToListAsync();
            return View(tours);
        }

        // GET: Tours/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tours/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tour tour, string CarouselImages)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Handle CarouselImages: store as a comma-separated string
                    if (!string.IsNullOrEmpty(CarouselImages))
                    {
                        tour.CarouselImages = string.Join(",", CarouselImages.Split(',').Select(img => img.Trim()));
                    }

                    // Add the tour to the database
                    _context.Add(tour);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Tour created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("ModelState is invalid.");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
            }

            return View(tour);
        }

        // GET: Tours/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var tour = await _context.Tours.FindAsync(id);
            if (tour == null)
            {
                return NotFound();
            }
            return View(tour);
        }

        // POST: Tours/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Tour tour, string CarouselImages)
        {
            if (id != tour.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle CarouselImages: update as a comma-separated string
                    if (!string.IsNullOrEmpty(CarouselImages))
                    {
                        tour.CarouselImages = string.Join(",", CarouselImages.Split(',').Select(img => img.Trim()));
                    }

                    _context.Update(tour);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Tour updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TourExists(tour.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }

            return View(tour);
        }

        // GET: Tours/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var tour = await _context.Tours.FindAsync(id);
            if (tour == null)
            {
                return NotFound();
            }
            return View(tour);
        }

        // POST: Tours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tour = await _context.Tours.FindAsync(id);
            if (tour != null)
            {
                _context.Tours.Remove(tour);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Tour deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        // Check if a tour exists
        private bool TourExists(int id)
        {
            return _context.Tours.Any(e => e.Id == id);
        }
    }
}
