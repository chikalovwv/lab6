using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyChat.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace MyChat.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
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
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            DateTime now = DateTime.UtcNow;
            int age = now.Year - model.BirthDate.Year;
            if (model.BirthDate > now.AddYears(-age)) { age = age - 1; }

            if (age < 18)
            {
                ModelState.AddModelError("", "Нельзя регистрировать пользователя младше 18 лет.");
                return View(model);
            }

            ApplicationUser user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                BirthDate = model.BirthDate,
                AvatarUrl = string.IsNullOrWhiteSpace(model.AvatarUrl) ? "/images/default-avatar.png" : model.AvatarUrl
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Chat");
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            LoginViewModel model = new LoginViewModel { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            ApplicationUser user = await _userManager.FindByNameAsync(model.UserNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(model.UserNameOrEmail);
            }

            if (user == null)
            {
                ModelState.AddModelError("", "Неверные учетные данные.");
                return View(model);
            }

            if (user.IsBlocked)
            {
                ModelState.AddModelError("", "Ваш аккаунт заблокирован.");
                return View(model);
            }

            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (signInResult.Succeeded)
            {
                if (string.IsNullOrWhiteSpace(model.ReturnUrl))
                {
                    return RedirectToAction("Index", "Chat");
                }
                return Redirect(model.ReturnUrl);
            }

            ModelState.AddModelError("", "Неверные учетные данные.");
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Chat");
        }

        public IActionResult AccessDenied()
        {
            return Content("Доступ запрещен.");
        }
    }
}