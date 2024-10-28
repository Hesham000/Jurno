using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Travel.Models;
using Microsoft.EntityFrameworkCore;

namespace Travel.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly TravelDbContext _context;

        public PaymentController(TravelDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBookingSession(int tourId)
        {
            var tour = await _context.Tours.FindAsync(tourId);

            if (tour == null)
            {
                return NotFound("Tour not found.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("User must be logged in to book.");
            }

            var finalPrice = tour.Discount > 0 ? tour.Price - (tour.Price * tour.Discount / 100) : tour.Price;

            var booking = new Booking
            {
                BookingDate = DateTime.Now,
                IsConfirmed = false, 
                TourId = tour.Id,
                UserId = userId, 
                PaymentStatus = "Pending"
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(finalPrice * 100), 
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = tour.Name,
                                Description = tour.Description,
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = Url.Action("PaymentSuccess", "Payment", new { bookingId = booking.BookingId }, Request.Scheme),
                CancelUrl = Url.Action("PaymentCancelled", "Payment", null, Request.Scheme),
            };

            var service = new SessionService();
            var session = service.Create(options);

            booking.PaymentId = session.Id;
            await _context.SaveChangesAsync();

            return Redirect(session.Url);
        }

        [HttpGet]
        public async Task<IActionResult> PaymentSuccess(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);

            if (booking == null)
            {
                return NotFound("Booking not found.");
            }

            booking.IsConfirmed = true;
            booking.PaymentStatus = "Completed";
            await _context.SaveChangesAsync();

            return View("Success"); 
        }

        [HttpGet]
        public IActionResult PaymentCancelled()
        {
            return View("Cancelled"); 
        }
    }
}
