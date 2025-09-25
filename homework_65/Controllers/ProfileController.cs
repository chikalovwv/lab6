using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MyChat.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MyChat.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProfileController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> User(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return NotFound();
            var user = await _db.Set<Microsoft.AspNetCore.Identity.IdentityUser>().FirstOrDefaultAsync();
            int messagesCount = await _db.Messages.CountAsync(m => m.UserName == username);
            ViewBag.UserName = username;
            ViewBag.MessagesCount = messagesCount;
            return View();
        }
    }
}