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

            var chats = await _context.Chats
                .Include(c => c.ChatUsers)
                .Where(c => c.ChatUsers
                    .Any(u => u.Id == user.Id))
                .ToListAsync();
            foreach (var chatItem in chats)
            {
                Message lastMessage = await _context.Messages.Include(c => c.Chat).OrderByDescending(d => d.DateTime).FirstOrDefaultAsync(m => m.Chat.Id == chatItem.Id);
                if (lastMessage != null)
                    chatItem.Messages.Add(lastMessage);
                else
                    chatItem.Messages = new List<Message>();
            }

            ChatViewModel model = new ChatViewModel()
            {
                User = user,
                Employees = await _context.Employees.Where(u => u.Fio != this.User.Identity.Name).ToListAsync(),
                Chats = chats.OrderByDescending(m => m.Messages.LastOrDefault()?.DateTime).ToList()
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
            if (userDirectId == null)
                return RedirectToAction(nameof(Index));
            Employee userDirect = await _context.Employees.FirstOrDefaultAsync(emp => emp.Id == userDirectId);
            if (userDirect is null)
                return RedirectToAction(nameof(Index));

            var chats = await _context.Chats
                .Include(c => c.ChatUsers)
                .Where(c => c.ChatUsers
                    .Any(u => u.Id == user.Id))
                .ToListAsync();
            foreach (var chatItem in chats)
            {
                Message lastMessage = await _context.Messages.Include(c => c.Chat).OrderByDescending(d => d.DateTime).FirstOrDefaultAsync(m => m.Chat.Id == chatItem.Id);
                if (lastMessage != null)
                    chatItem.Messages.Add(lastMessage);
            }

            ChatViewModel model = new ChatViewModel()
            {
                User = user,
                Employees = await _context.Employees.Where(u => u.Fio != this.User.Identity.Name).ToListAsync(),
                ChatActive = chat,
                UserDirect = userDirect,
                Chats = chats.OrderByDescending(m => m.Messages.LastOrDefault()?.DateTime).ToList()
            };
            List<Message> messagesRead = new List<Message>();
            foreach (var messageItem in chat.Messages)
            {
                if (messageItem.User != user && messageItem.Status != StatusMessage.Read)
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
        [HttpGet]
        public async Task<IActionResult> CreateDialog(string employeeId)
        {
            var userDirect = await _context.Employees.FirstOrDefaultAsync(emp => emp.Id == employeeId);
            var user = await _context.Employees.FirstOrDefaultAsync(emp => emp.Id == _userManager.GetUserId(User));
            if (userDirect == null && user == null)
            {
                return PartialView("_errorLoadPartial", "Ошибка при создании диалога.");
            }
            Chat chat = new Chat()
            {
                TypeChat = TypeChat.Direct,
                ChatUsers = new List<Employee>() { user, userDirect },
            };
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();
            int chatId = _context.Chats.FirstOrDefaultAsync(c => c.ChatUsers.Contains(userDirect) && c.ChatUsers.Contains(user)).Result.Id;
            return RedirectToAction(nameof(ChatLoad), new { id = chatId.ToString() });
        }
        [Route("chat/MessageBoxPartialUpdateDirect/{idchat}")]
        [HttpGet]
        public async Task<PartialViewResult> MessageBoxPartialUpdateDirect(string idChat)
        {
            Chat chat = await _context.Chats.Include(m => m.Messages).ThenInclude(u => u.User).Include(u => u.ChatUsers).FirstOrDefaultAsync(c => c.Id == Convert.ToInt32(idChat));
            Employee user = await _context.Employees.FirstOrDefaultAsync(emp => emp.Id == _userManager.GetUserId(User));
            if (chat == null)
            {
                return MessageBoxPartialUpdate().Result;
            }
            var userDirectId = chat.ChatUsers.FirstOrDefault(c => c.Id != user.Id).Id;
            if (user is null || userDirectId == null)
                return null;
            Employee userDirect = await _context.Employees.FirstOrDefaultAsync(emp => emp.Id == userDirectId);
            if (userDirect is null)
                return null;


            var chats = await _context.Chats
                .Include(c => c.ChatUsers)
                .Where(c => c.ChatUsers
                    .Any(u => u.Id == user.Id))
                .ToListAsync();
            foreach (var chatItem in chats)
            {
                Message lastMessage = await _context.Messages.OrderByDescending(d => d.DateTime).FirstOrDefaultAsync(m => m.Chat.Id == chatItem.Id);
                if (lastMessage != null)
                    chatItem.Messages.Add(lastMessage);
            }

            ChatViewModel model = new ChatViewModel()
            {
                User = user,
                Employees = await _context.Employees.Where(u => u.Fio != this.User.Identity.Name).ToListAsync(),
                ChatActive = chat,
                UserDirect = userDirect,
                Chats = chats.OrderByDescending(m => m.Messages.LastOrDefault()?.DateTime).ToList()
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
            var chats = await _context.Chats
                .Include(c => c.ChatUsers)
                .Where(c => c.ChatUsers
                    .Any(u => u.Id == user.Id))
                .ToListAsync();
            foreach (var chatItem in chats)
            {
                Message lastMessage = await _context.Messages.Include(c => c.Chat).OrderByDescending(d => d.DateTime).FirstOrDefaultAsync(m => m.Chat.Id == chatItem.Id);
                if (lastMessage != null)
                    chatItem.Messages.Add(lastMessage);
            }

            ChatViewModel model = new ChatViewModel()
            {
                User = user,
                Employees = await _context.Employees.Where(u => u.Fio != this.User.Identity.Name).ToListAsync(),
                Chats = chats.OrderByDescending(m => m.Messages.LastOrDefault()?.DateTime).ToList()
            };
            return PartialView("_messagesBoxPartial", model);
        }
        [Route("chat/ChatLoad/{idChat}")]
        [HttpGet]
        public async Task<IActionResult> ChatLoad(string idChat)
        {
            try
            {
                Chat chat = await _context.Chats
                    .Include(m => m.Messages.Where(d => d.DateTime.Day >= (DateTime.Now.AddDays(-1)).Day))
                        .ThenInclude(u => u.User)
                    .Include(u => u.ChatUsers)
                    .FirstOrDefaultAsync(c => c.Id == Convert.ToInt32(idChat));
                Employee user = await _context.Employees
                    .FirstOrDefaultAsync(emp => emp.Id == _userManager.GetUserId(User));

                var userDirectId = chat.ChatUsers
                    .FirstOrDefault(c => c.Id != user.Id).Id;

                if (chat == null || user is null || userDirectId == null)
                    return PartialView("_errorLoadPartial", "Ошибка при загрузке диалога.");

                Employee userDirect = await _context.Employees
                    .FirstOrDefaultAsync(emp => emp.Id == userDirectId);

                if (userDirect is null)
                    return PartialView("_errorLoadPartial", "Ошибка при загрузке диалога.");


                var userSend = await _context.Employees
                    .FirstOrDefaultAsync(u => u.Id == user.Id);

                List<Message> messagesRead = new List<Message>();
                foreach (var messageItem in chat.Messages)
                {
                    if (messageItem.User != userSend && messageItem.Status != StatusMessage.Read)
                    {
                        messageItem.Status = StatusMessage.Read;
                        messagesRead.Add(messageItem);
                    }
                }
                foreach (var messageRead in messagesRead)
                {
                    await _hubContext.Clients.All.SendAsync("Read", messageRead.Id)
                        .ConfigureAwait(true);
                }
                _context.Chats.UpdateRange(chat);
                await _context.SaveChangesAsync();



                ChatViewModel model = new ChatViewModel()
                {
                    User = user,
                    ChatActive = chat,
                    UserDirect = userDirect
                };
                return PartialView("_chatBoxPartial", model);
            }
            catch(Exception ex)
            {
                return PartialView("_errorLoadPartial", "Ошибка при загрузке диалога.");
            }
        }
    }
}
