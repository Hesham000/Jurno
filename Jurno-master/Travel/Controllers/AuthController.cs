using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Travel.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Net;
using Travel.Models;
using Travel.Services;

namespace TravelBookingModels.Controllers
{
    public class AuthController : Controller
    {
        private readonly TravelDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;

        public AuthController(TravelDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }





        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.PageTitle = "Register";
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    var claims = new List<Claim>
                    {
                      new Claim(ClaimTypes.Name, user.FullName),
                      new Claim(ClaimTypes.NameIdentifier, user.Id), 
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }




        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.PageTitle = "LogIn";
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
                        var claims = new List<Claim>
                        {
                           new Claim(ClaimTypes.Name, user.FullName), 
                           new Claim(ClaimTypes.NameIdentifier, user.Id),
                     
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties
                        {
                            IsPersistent = model.RememberMe,
                            ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(14) : (DateTimeOffset?)null 
                        });




                        HttpContext.Session.SetString("UserId", user.Id);
                        return RedirectToAction("Index", "Home"); 


                    }
                }
            }
            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }



        //[HttpGet]
        //public IActionResult PasswordManagement(string userId, string token, bool isReset = false)
        //{
        //    var model = new PasswordManagementViewModel
        //    {
        //        UserId = userId,
        //        Token = token,
        //        IsReset = isReset
        //    };

        //    return View(model);
        //}




        //[HttpPost]
        //public async Task<IActionResult> PasswordManagement(PasswordManagementViewModel model)
        //{
        //    if (!string.IsNullOrEmpty(model.UserId) && !string.IsNullOrEmpty(model.Token) && model.IsReset)
        //    {
        //        // Reset Password flow
        //        if (ModelState.IsValid)
        //        {
        //            var user = await _userManager.FindByIdAsync(model.UserId);
        //            if (user == null)
        //            {
        //                ModelState.AddModelError(string.Empty, "User not found.");
        //                return View("ResetPassword", model); // Return Reset Password view
        //            }

        //            var decodedToken = WebUtility.UrlDecode(model.Token);
        //            var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);

        //            if (result.Succeeded)
        //            {
        //                ViewBag.Message = "Your password has been reset successfully.";
        //                return View("ResetPassword", model); // Success
        //            }

        //            foreach (var error in result.Errors)
        //            {
        //                ModelState.AddModelError(string.Empty, error.Description);
        //            }
        //        }
        //        return View("ResetPassword", model); // Show validation errors in Reset Password view
        //    }
        //    else
        //    {
        //        // Forgot Password flow
        //        if (ModelState.IsValid)
        //        {
        //            var user = await _userManager.FindByEmailAsync(model.Email);
        //            if (user == null)
        //            {
        //                ModelState.AddModelError(string.Empty, "User not found.");
        //                return View("ForgotPassword", model); // Return Forgot Password view
        //            }

        //            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //            var resetLink = Url.Action("PasswordManagement", "Auth", new
        //            {
        //                userId = user.Id,
        //                token = WebUtility.UrlEncode(token),
        //                isReset = true
        //            }, Request.Scheme);

        //            await _emailService.SendEmailAsync(
        //                model.Email,
        //                "Reset Your Password",
        //                $"Please reset your password by clicking here: <a href='{resetLink}'>Reset Password</a>");

        //            ViewBag.Message = "A password reset link has been sent to your email.";
        //        }
        //        return View("ForgotPassword", model); // Show validation errors in Forgot Password view
        //    }
        //}

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                    return View(model);
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetLink = Url.Action("ResetPassword", "Auth", new { userId = user.Id, token = token }, Request.Scheme);

                var subject = "Reset Password";
                string emailBody = $"Please reset your password by clicking on the link below:<br><a href='{resetLink}'>Reset Password</a>";
                
                try
                {
                    await _emailService.SendEmailAsync(user.Email, subject, emailBody);
                    ViewBag.Message = "A password reset link has been sent to your email address.";
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "There was an error sending the email. Please try again.");
                    return View(model);
                }

                
            }

            return View(model);
        }




        
        [HttpGet]
        public IActionResult ResetPassword(string userId, string token)
        {
            var model = new ResetPasswordViewModel { UserId = userId, Token = token };
            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
             
                ViewBag.Message = "Password reset successful.";
                return View(model);
            }

            var decodedToken = model.Token;
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);

            if (result.Succeeded)
            {
                ViewBag.Message = "Your password has been reset successfully.";
                return View(model);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }




    }

}

