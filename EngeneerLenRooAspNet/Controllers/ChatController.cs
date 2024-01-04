using EngeneerLenRooAspNet.Areas.Identity.Data;
using EngeneerLenRooAspNet.Models;
using EngeneerLenRooAspNet.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EngeneerLenRooAspNet.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private MainContext _context;
        public ChatController(UserManager<IdentityUser> userManager, MainContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        [Route("")]
        [Route("chat")]
        public async Task<IActionResult> Index()
        {
            Employee user = await _context.Employees.FirstOrDefaultAsync(emp => emp.Id == _userManager.GetUserId(User));
            if (user is null)
                return NotFound();


            ChatViewModel model = new ChatViewModel()
            {
                User = user,
                Employees = await _context.Employees.Where(u => u.Fio != this.User.Identity.Name).ToListAsync(),
                Chats = _context.Chats.Where(c => c.ChatUsers.Contains(user)).Include(c => c.ChatUsers).Include(m => m.Messages).ToList()
            };
            return View(model);
        }
        [Route("chat/{id}")]
        public async Task<IActionResult> Index(int id)
        {
            Chat chat = await _context.Chats.Include(m => m.Messages).Include(u => u.ChatUsers).FirstOrDefaultAsync(c => c.Id == id);
            Employee user = await _context.Employees.FirstOrDefaultAsync(emp => emp.Id == _userManager.GetUserId(User));
            var userDirectId = chat.ChatUsers.FirstOrDefault(c => c.Id != user.Id).Id;
            if(chat == null || user is null || userDirectId == null)
                return RedirectToAction(nameof(Index));
            Employee userDirect = await _context.Employees.FirstOrDefaultAsync(emp => emp.Id == userDirectId);
            if (userDirect is null)
                return RedirectToAction(nameof(Index));

            ChatViewModel model = new ChatViewModel()
            {
                User = user,
                Employees = await _context.Employees.Where(u => u.Fio != this.User.Identity.Name).ToListAsync(),
                ChatActive = chat,
                UserDirect = userDirect,
                Chats = _context.Chats.Where(c => c.ChatUsers.Contains(user)).Include(u => u.ChatUsers).ToList()
            };
            return View(model);
        }
        [Route("chat/dialog/create")]
        public async Task<IActionResult> CreateDialog(string employeeId)
        {
            var userDirect = await _context.Employees.FirstOrDefaultAsync(emp => emp.Id == employeeId);
            var user = await _context.Employees.FirstOrDefaultAsync(emp => emp.Id == _userManager.GetUserId(User));
            if (userDirect == null && user == null)
            {
                return RedirectToAction(nameof(Index));
            }
            Chat chat = new Chat()
            {
                TypeChat = TypeChat.Direct,
                ChatUsers = new List<Employee>() { user,  userDirect },
            };
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();
            int chatId = _context.Chats.FirstOrDefaultAsync(c => c.ChatUsers.Contains(userDirect) && c.ChatUsers.Contains(user)).Result.Id;
            return RedirectToAction(nameof(Index), new { id = chatId});
        }
    }
}
