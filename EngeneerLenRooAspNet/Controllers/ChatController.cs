using EngeneerLenRooAspNet.Areas.Identity.Data;
using EngeneerLenRooAspNet.Models;
using EngeneerLenRooAspNet.Services;
using EngeneerLenRooAspNet.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EngeneerLenRooAspNet.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly MainContext _context;
        readonly IHubContext<ChatHub> _hubContext;
        IWebHostEnvironment _appEnvironment;
        public ChatController(UserManager<IdentityUser> userManager, MainContext context, IHubContext<ChatHub> hubContext, IWebHostEnvironment appEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _hubContext = hubContext;
            _appEnvironment = appEnvironment;
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
                Message lastMessage = await _context.Messages.Include(c => c.Chat).Include(f => f.File).OrderByDescending(d => d.DateTime).FirstOrDefaultAsync(m => m.Chat.Id == chatItem.Id);
                if (lastMessage != null)
                    chatItem.Messages.Add(lastMessage);
                else
                    chatItem.Messages = new List<Message>();
            }

            List<Employee> employees = await _context.Employees
                .Include(c => c.Chats)
                    .ThenInclude(c => c.ChatUsers)
                .Where(u => u.Id != user.Id
                && u.Chats.Count(c => c.ChatUsers.Any(emp => emp.Id == user.Id) && c.TypeChat != TypeChat.Group) == 0
                && !u.Fio.Contains("admin"))
                .ToListAsync();

            ChatViewModel model = new()
            {
                User = user,
                Employees = employees,
                Chats = chats.OrderByDescending(m => m.Messages.LastOrDefault()?.DateTime).ToList()
            };
            return View(model);
        }

        [Route("chat/CreateDialog/{employeeId}")]
        [HttpGet]
        public async Task<IActionResult> CreateDialog(string employeeId)
        {
            var userDirect = await _context.Employees.FirstOrDefaultAsync(emp => emp.Id == employeeId);
            var user = await _context.Employees.FirstOrDefaultAsync(emp => emp.Id == _userManager.GetUserId(User));
            if (userDirect == null || user == null)
            {
                return PartialView("_errorLoadPartial", "Ошибка при создании диалога.");
            }
            Chat chat = new()
            {
                TypeChat = TypeChat.Direct,
                ChatUsers = new List<Employee>() { user, userDirect },
                Name = " "
            };
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();
            int chatId = _context.Chats.FirstOrDefaultAsync(c => c.ChatUsers.Contains(userDirect) && c.ChatUsers.Contains(user)).Result.Id;
            return RedirectToAction(nameof(ChatLoad), new { id = chatId.ToString() });
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup(string name, List<string> employees)
        {
            Employee user = await _context.Employees.FirstOrDefaultAsync(emp => emp.Id == _userManager.GetUserId(User));
            List<Employee> employeesList = new()
            {
                user
            };

            foreach (var emp in employees)
            {
                Employee employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == emp);
                employeesList.Add(employee);
            }

            Chat chat = new()
            {
                ChatUsers = employeesList,
                EmployeeAdministrator = user,
                EmployeeCreate = user,
                Name = name,
                TypeChat = TypeChat.Group
            };
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();
            int chatId = _context.Chats.FirstOrDefaultAsync(c => c.Name == name && c.EmployeeCreate == user).Result.Id;
            return RedirectToAction(nameof(Index));
        }

        [Route("chat/MessageBoxPartialUpdateDirect/{idchat}")]
        [Route("chat/MessageBoxPartialUpdateDirect/{idchat}/{search}")]
        [HttpGet]
        public async Task<PartialViewResult> MessageBoxPartialUpdateDirect(string idChat, string search = "")
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
                Message lastMessage = await _context.Messages
                    .Include(f => f.File)
                    .OrderByDescending(d => d.DateTime)
                    .FirstOrDefaultAsync(m => m.Chat.Id == chatItem.Id);
                if (lastMessage != null)
                    chatItem.Messages.Add(lastMessage);
            }
            List<Employee> employees = await _context.Employees
                .Include(c => c.Chats)
                    .ThenInclude(c => c.ChatUsers)
                .Where(u => u.Id != user.Id
                && u.Chats.Count(c => c.ChatUsers.Any(emp => emp.Id == user.Id) && c.TypeChat != TypeChat.Group) == 0
                && !u.Fio.Contains("admin"))
                .ToListAsync();
            if (search != null && search != string.Empty)
            {
                employees = employees.Where(e => e.Fio.Contains(search)).ToList();
                chats = chats.Where(c => c.ChatUsers
                .FirstOrDefault(emp => emp.Id != user.Id).Fio.Contains(search, StringComparison.OrdinalIgnoreCase)
                || c.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
                && c != chat)
                    .ToList();
            }
            ChatViewModel model = new()
            {
                User = user,
                Employees = employees,
                ChatActive = chat,
                UserDirect = userDirect,
                Chats = chats.OrderByDescending(m => m.Messages.LastOrDefault()?.DateTime).ToList()
            };
            return PartialView("_messagesBoxPartial", model);
        }

        [Route("chat/MessageBoxPartialUpdate")]
        [Route("chat/MessageBoxPartialUpdate/{search}")]
        [HttpGet]
        public async Task<PartialViewResult> MessageBoxPartialUpdate(string search = "")
        {
            Employee user = await _context.Employees.FirstOrDefaultAsync(emp => emp.Id == _userManager.GetUserId(User));
            if (user is null)
                return PartialView("_errorPartialView", "Ошибка загрузки сообщений.");
            var chats = await _context.Chats
                .Include(c => c.ChatUsers)
                .Where(c => c.ChatUsers
                    .Any(u => u.Id == user.Id))
                .ToListAsync();
            foreach (var chatItem in chats)
            {
                Message lastMessage = await _context.Messages
                    .Include(c => c.Chat)
                    .Include(f => f.File)
                    .OrderByDescending(d => d.DateTime)
                    .FirstOrDefaultAsync(m => m.Chat.Id == chatItem.Id);
                if (lastMessage != null)
                    chatItem.Messages.Add(lastMessage);
            }

            List<Employee> employees = await _context.Employees
                .Include(c => c.Chats)
                    .ThenInclude(c => c.ChatUsers)
                .Where(u => u.Id != user.Id
                && u.Chats.Count(c => c.ChatUsers.Any(emp => emp.Id == user.Id) && c.TypeChat != TypeChat.Group) == 0
                && !u.Fio.Contains("admin"))
                .ToListAsync();
            List<Employee> EmployeesGroup = await _context.Employees
                .Where(emp => user.IsCanIWriteUser(emp)).ToListAsync();
            if (search != null && search != string.Empty)
            {
                employees = employees.Where(e => e.Fio.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
                chats = chats.Where(c => c.ChatUsers.FirstOrDefault(emp => emp.Id != user.Id).Fio.Contains(search, StringComparison.OrdinalIgnoreCase) || c.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            ChatViewModel model = new()
            {
                User = user,
                Employees = employees,
                Chats = chats.OrderByDescending(m => m.Messages.LastOrDefault()?.DateTime).ToList(),
                EmployeesGroup = EmployeesGroup
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
                    .Include(m => m.Messages.OrderByDescending(d => d.DateTime).Take(50))
                        .ThenInclude(u => u.User)
                    .Include(u => u.ChatUsers)
                    .FirstOrDefaultAsync(c => c.Id == Convert.ToInt32(idChat));
                Employee user = await _context.Employees
                    .FirstOrDefaultAsync(emp => emp.Id == _userManager.GetUserId(User));

                var userDirectId = chat.ChatUsers
                    .FirstOrDefault(c => c.Id != user.Id).Id;

                if (chat == null || user is null || userDirectId == null)
                    return PartialView("_errorLoadPartial", "Ошибка при загрузке диалога.");

                Employee userDirect = await _context.Employees.Include(c => c.Cabinet)
                    .FirstOrDefaultAsync(emp => emp.Id == userDirectId);

                if (userDirect is null)
                    return PartialView("_errorLoadPartial", "Ошибка при загрузке диалога.");

                bool isAllMessageLoad = _context.Messages.Include(c => c.Chat)
                    .Count(c => c.Chat.Id == chat.Id) <= 50;

                var userSend = await _context.Employees
                    .FirstOrDefaultAsync(u => u.Id == user.Id);

                List<Message> messagesRead = new();
                foreach (var messageItem in chat.Messages)
                {
                    messageItem.File = await _context.File.FirstOrDefaultAsync(f => f.Id == messageItem.FileId);
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

                ChatViewModel model = new()
                {
                    User = user,
                    ChatActive = chat,
                    IsAllMessageLoad = isAllMessageLoad,
                    CountMessageLoad = 50,
                    UserDirect = userDirect
                };
                return PartialView("_chatBoxPartial", model);
            }
            catch
            {
                return PartialView("_errorLoadPartial", "Ошибка при загрузке диалога.");
            }
        }

        [Route("chat/LoadMessage/{chatId}/{countLoadMessage}")]
        [HttpGet]
        public async Task<IActionResult> LoadMessage(string chatId, string countLoadMessage)
        {
            try
            {
                int countLoadMessageInt = Convert.ToInt32(countLoadMessage);
                Chat chat = await _context.Chats
                    .Include(m => m.Messages.OrderByDescending(d => d.DateTime).Skip(countLoadMessageInt).Take(50))
                        .ThenInclude(f => f.File)
                        .Include(m => m.Messages.OrderByDescending(d => d.DateTime).Skip(countLoadMessageInt).Take(50))
                        .ThenInclude(f => f.User)
                    .Include(u => u.ChatUsers)
                    .FirstOrDefaultAsync(c => c.Id == Convert.ToInt32(chatId));
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

                bool isAllMessageLoad = _context.Messages.Include(c => c.Chat)
                    .Count(c => c.Chat.Id == chat.Id) <= countLoadMessageInt + 50;

                var userSend = await _context.Employees
                    .FirstOrDefaultAsync(u => u.Id == user.Id);



                ChatViewModel model = new()
                {
                    User = user,
                    ChatActive = chat,
                    IsAllMessageLoad = isAllMessageLoad,
                    CountMessageLoad = countLoadMessageInt + 50,
                    UserDirect = userDirect
                };
                return PartialView("_loadMessage", model);
            }
            catch
            {
                return PartialView("_errorLoadPartial", "Ошибка при загрузке диалога.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> GroupDelete(int id)
        {
            Chat chat = await _context.Chats.Include(u => u.EmployeeAdministrator).Include(m => m.Messages).Include(u => u.ChatUsers).FirstOrDefaultAsync(c => c.Id == id);
            if (chat is null) return RedirectToAction(nameof(Index));
            if (chat.EmployeeAdministrator.Id == _userManager.GetUserId(User))
            {
                _context.Chats.Remove(chat);
                await _context.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("GroupDelete", chat.Id)
                        .ConfigureAwait(true);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUserGroup(int id, string userId)
        {
            Chat chat = await _context.Chats.Include(u => u.ChatUsers).Include(m => m.Messages).Include(u => u.EmployeeAdministrator).FirstOrDefaultAsync(c => c.Id == id);
            if (chat is null || chat.ChatUsers is null || !chat.ChatUsers.Any(u => u.Id == userId)) return RedirectToAction(nameof(Index));
            Employee user = chat.ChatUsers.FirstOrDefault(u => u.Id == userId);
            chat.ChatUsers.Remove(user);
            _context.Chats.Update(chat);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("DeleteUserGroup", chat.Id, user.Id)
                    .ConfigureAwait(true);
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<JsonResult> UploadFile()
        {
            try
            {
                IFormFile uploadedFile = Request.Form.Files[0];

                string idFileGuid = Guid.NewGuid().ToString();

                string path = $"/Files/{idFileGuid}_{uploadedFile.FileName}";
                TypeFile typeFile = uploadedFile.ContentType.Contains("image") ? TypeFile.image : TypeFile.text;
                Models.File file = new Models.File
                {
                    Id = idFileGuid,
                    Path = path,
                    OriginalName = uploadedFile.FileName,
                    Size = uploadedFile.Length,
                    TypeFile = typeFile
                };

                await _context.File.AddAsync(file);
                await _context.SaveChangesAsync();

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                return Json(new { Status = "Success", File = idFileGuid.ToString() });

            }
            catch
            {
                return Json(new { Status = "Error", File = "" });
            }

        }




        //public ChatViewModel GetChatViewModel()
        //{

        //}
    }
}
