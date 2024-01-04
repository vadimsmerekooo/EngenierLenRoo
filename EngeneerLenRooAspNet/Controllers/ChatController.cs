using EngeneerLenRooAspNet.Areas.Identity.Data;
using EngeneerLenRooAspNet.Models;
using EngeneerLenRooAspNet.Services;
using EngeneerLenRooAspNet.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
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
        readonly IHubContext<ChatHub> _hubContext;
        public ChatController(UserManager<IdentityUser> userManager, MainContext context, IHubContext<ChatHub> hubContext)
        {
            _userManager = userManager;
            _context = context;
            _hubContext = hubContext;
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
                Chats = _context.Chats
                .Where(c => c.ChatUsers
                    .Contains(user))
                .Include(c => c.ChatUsers)
                .Include(m => m.Messages)
                .OrderByDescending(m => m.Messages.Max(d => d.DateTime))
                .ToList()
            };
            return View(model);
        }
        [Route("chat/{id}")]
        public async Task<IActionResult> Index(int id)
        {
            Chat chat = await _context.Chats.Include(m => m.Messages).ThenInclude(u => u.User).Include(u => u.ChatUsers).FirstOrDefaultAsync(c => c.Id == id);
            Employee user = await _context.Employees.FirstOrDefaultAsync(emp => emp.Id == _userManager.GetUserId(User));
            if (chat == null || user == null) return null;
            var userDirectId = chat.ChatUsers.FirstOrDefault(c => c.Id != user.Id).Id;
            if(userDirectId == null)
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
                Chats = _context.Chats
                .Where(c => c.ChatUsers
                    .Contains(user))
                .Include(u => u.ChatUsers)
                .Include(m => m.Messages)
                .OrderByDescending(m => m.Messages
                    .Max(d => d.DateTime))
                .ToList()
            };
            List<Message> messagesRead = new List<Message>();
            foreach (var messageItem in chat.Messages)
            {
                if (messageItem.User != user)
                {
                    messageItem.Status = StatusMessage.Read;
                    messagesRead.Add(messageItem);
                }
            }
            foreach (var messageRead in messagesRead)
            {
                await this._hubContext.Clients.All.SendAsync("Read", messageRead.Id)
                    .ConfigureAwait(true);
            }


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
        [Route("chat/MessageBoxPartialUpdateDirect/{idchat}")]
        [HttpGet]
        public async Task<PartialViewResult> MessageBoxPartialUpdateDirect(string idChat)
        {
            Chat chat = await _context.Chats.Include(m => m.Messages).ThenInclude(u => u.User).Include(u => u.ChatUsers).FirstOrDefaultAsync(c => c.Id == Convert.ToInt32(idChat));
            Employee user = await _context.Employees.FirstOrDefaultAsync(emp => emp.Id == _userManager.GetUserId(User));
            if(chat == null)
            {
                return MessageBoxPartialUpdate().Result;
            }
            var userDirectId = chat.ChatUsers.FirstOrDefault(c => c.Id != user.Id).Id;
            if (user is null || userDirectId == null)
                return null;
            Employee userDirect = await _context.Employees.FirstOrDefaultAsync(emp => emp.Id == userDirectId);
            if (userDirect is null)
                return null;

            ChatViewModel model = new ChatViewModel()
            {
                User = user,
                Employees = await _context.Employees.Where(u => u.Fio != this.User.Identity.Name).ToListAsync(),
                ChatActive = chat,
                UserDirect = userDirect,
                Chats = _context.Chats
                .Where(c => c.ChatUsers
                    .Contains(user))
                .Include(u => u.ChatUsers)
                .Include(m => m.Messages)
                .OrderByDescending(m => m.Messages
                    .Max(d => d.DateTime))
                .ToList()
            };
            return PartialView("_messagesBoxPartial", model);
        }
        [Route("chat/MessageBoxPartialUpdate")]
        [HttpGet]
        public async Task<PartialViewResult> MessageBoxPartialUpdate()
        {
            Employee user = await _context.Employees.FirstOrDefaultAsync(emp => emp.Id == _userManager.GetUserId(User));
            if (user is null)
                return null;


            ChatViewModel model = new ChatViewModel()
            {
                User = user,
                Employees = await _context.Employees.Where(u => u.Fio != this.User.Identity.Name).ToListAsync(),
                Chats = _context.Chats
                .Where(c => c.ChatUsers
                    .Contains(user))
                .Include(c => c.ChatUsers)
                .Include(m => m.Messages)
                .OrderByDescending(m => m.Messages.Max(d => d.DateTime))
                .ToList()
            };
            return PartialView("_messagesBoxPartial", model);
        }
        [Route("chat/ChatLoad/{idChat}")]
        [HttpGet]
        public async Task<IActionResult> ChatLoad(string idChat)
        {
            Chat chat = await _context.Chats.Include(m => m.Messages).ThenInclude(u => u.User).Include(u => u.ChatUsers).FirstOrDefaultAsync(c => c.Id == Convert.ToInt32(idChat));
            Employee user = await _context.Employees.FirstOrDefaultAsync(emp => emp.Id == _userManager.GetUserId(User));
            var userDirectId = chat.ChatUsers.FirstOrDefault(c => c.Id != user.Id).Id;
            if (chat == null || user is null || userDirectId == null)
                return null;
            Employee userDirect = await _context.Employees.FirstOrDefaultAsync(emp => emp.Id == userDirectId);
            if (userDirect is null)
                return null;

            ChatViewModel model = new ChatViewModel()
            {
                User = user,
                ChatActive = chat,
                UserDirect = userDirect
            };
            return PartialView("_chatBoxPartial", model);
        }
    }
}
