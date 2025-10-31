using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QABoard.Infrastructure.Entities;
using QABoard.Web.Models;
using System.Text.RegularExpressions;

namespace QABoard.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Custom password validation: at least 2 uppercase, 3 numbers, 3 symbols
                var uppercaseCount = Regex.Matches(model.Password, @"[A-Z]").Count;
                var digitCount = Regex.Matches(model.Password, @"\d").Count;
                var symbolCount = Regex.Matches(model.Password, @"[^a-zA-Z0-9]").Count;

                if (uppercaseCount < 2)
                {
                    ModelState.AddModelError("Password", "Password must contain at least 2 uppercase letters.");
                    return View(model);
                }
                if (digitCount < 3)
                {
                    ModelState.AddModelError("Password", "Password must contain at least 3 numbers.");
                    return View(model);
                }
                if (symbolCount < 3)
                {
                    ModelState.AddModelError("Password", "Password must contain at least 3 symbols.");
                    return View(model);
                }

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    await _signInManager.SignInAsync(user, isPersistent: false);
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}