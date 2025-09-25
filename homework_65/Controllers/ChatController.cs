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
        [Authorize]
        public async Task<IActionResult> Send([FromBody] SendMessageRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Text))
            {
                return BadRequest("empty");
            }

            string text = request.Text.Trim();
            int length = text.Length;
            if (length < 1 || length > 150)
            {
                return BadRequest("length");
            }

            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user == null) return BadRequest("nouser");
            if (user.IsBlocked) return Forbid();

            Message message = new Message
            {
                Id = _db.Messages.Any() ? _db.Messages.Max(m => m.Id) + 1 : 1,
                UserId = user.Id,
                UserName = user.UserName,
                AvatarUrl = string.IsNullOrWhiteSpace(user.AvatarUrl) ? "/images/default-avatar.png" : user.AvatarUrl,
                Text = text,
                SentAt = DateTime.UtcNow
            };

            _db.Messages.Add(message);
            await _db.SaveChangesAsync();

            return Json(message);
        }
    }

    public class SendMessageRequest
    {
        public string Text { get; set; }
    }
}