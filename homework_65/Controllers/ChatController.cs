using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyChat.Data;
using MyChat.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MyChat.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages(int? afterId)
        {
            if (afterId.HasValue)
            {
                List<Message> newMessages = await _db.Messages
                    .Where(m => m.Id > afterId.Value)
                    .OrderBy(m => m.SentAt)
                    .ToListAsync();
                return Json(newMessages);
            }
            else
            {
                List<Message> last = await _db.Messages
                    .OrderByDescending(m => m.SentAt)
                    .Take(30)
                    .OrderBy(m => m.SentAt)
                    .ToListAsync();
                return Json(last);
            }
        }
[HttpPost]
public async Task<IActionResult> SendMessage([FromBody] Message message)
{
    var user = await _userManager.GetUserAsync(User);
    message.UserName = user?.UserName;
    message.AvatarUrl = user?.AvatarUrl;

    _db.Messages.Add(message);
    await _db.SaveChangesAsync();

    return Ok(new { success = true, message = message.Text });
}
        
    }

    public class SendMessageRequest
    {
        public string Text { get; set; }
    }
}