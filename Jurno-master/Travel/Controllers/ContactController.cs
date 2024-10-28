using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Travel.Models;

namespace Travel.Controllers
{
    public class ContactController : Controller
    {
        private readonly IEmailService _emailService;

        public ContactController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("ContactUs");
        }

     
        [HttpPost]
        public async Task<IActionResult> Send(ContactViewModel model)
        {
            
            if (!ModelState.IsValid)
            {
                return View("ContactUs", model); 
            }

            
            var message = $"Name: {model.Name}<br>Email: {model.Email}<br>Message: {model.Message}";

      
            await _emailService.SendEmailAsync("journo024@gmail.com", model.Subject, message);

           
            return RedirectToAction("Confirmation");
        }

        public IActionResult Confirmation()
        {
            return View();
        }
    }
}
