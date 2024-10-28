using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Travel.Data;
using Travel.Models;
using System.Threading.Tasks;

namespace Travel.Controllers
{
    public class TravelGuidesController : Controller
    {
        private readonly TravelDbContext _context;

        public TravelGuidesController(TravelDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.TravelGuides.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TravelGuide guide)
        {
            if (ModelState.IsValid)
            {
                _context.Add(guide);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(guide);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var guide = await _context.TravelGuides.FindAsync(id);
            if (guide == null)
                return NotFound();
            return View(guide);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TravelGuide guide)
        {
            if (id != guide.DestinationId)
                return BadRequest();

            if (ModelState.IsValid)
            {
                _context.Update(guide);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(guide);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var guide = await _context.TravelGuides.FindAsync(id);
            if (guide == null)
                return NotFound();
            return View(guide);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var guide = await _context.TravelGuides.FindAsync(id);
            _context.TravelGuides.Remove(guide);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
