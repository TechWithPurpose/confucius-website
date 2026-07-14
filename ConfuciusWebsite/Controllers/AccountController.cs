using ConfuciusWebsite.Models;
using ConfuciusWebsite.ViewModels;
using Microsoft.AspNetCore.Http; // Add this using directive for session extension methods
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using SixLabors.Fonts;

namespace ConfuciusWebsite.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AdminUser> _userManager;
        private readonly SignInManager<AdminUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
                UserManager<AdminUser> userManager,
                SignInManager<AdminUser> signInManager,
                ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }


        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string CaptchaInput, string? returnUrl = null)
        {
            try
            {
                var storedCode = HttpContext.Session.GetString("CaptchaCode");

                if (CaptchaInput != storedCode)
                {
                    ModelState.AddModelError("", "Incorrect security code.");
                    return View(model);
                }

                if (!ModelState.IsValid)
                    return View(model);

                var result = await _signInManager.PasswordSignInAsync(
                    model.Email,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    //
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    //If Successful, redirect to Dashboard
                    return RedirectToAction("Dashboard", "Admin", new { area = "Admin" }); ;
                }

                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for email: {Email}", model.Email);
                ModelState.AddModelError("", "An unexpected error occurred. Please try again later.");
                return View(model);
            }
        }
        

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        private string GenerateCaptchaCode()
        {
            var chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 5)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public IActionResult CaptchaImage()
        {
            var code = GenerateCaptchaCode();
            HttpContext.Session.SetString("CaptchaCode", code);

            using var image = new Image<Rgba32>(120, 40);
            image.Mutate(x => x.Fill(Color.White));

            image.Mutate(x => x.DrawText(
                code,
                SystemFonts.CreateFont("Arial", 20),
                Color.Black,
                new PointF(10, 5)
            ));

            using var ms = new MemoryStream();
            image.SaveAsPng(ms);
            return File(ms.ToArray(), "image/png");
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}