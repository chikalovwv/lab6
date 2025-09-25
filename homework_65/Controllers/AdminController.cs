using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MyChat.Models;
using Microsoft.AspNetCore.Authorization;

namespace MyChat.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users;
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleBlock(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            user.IsBlocked = !user.IsBlocked;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }
    }
}