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
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        ILogger<ChatController> _logger;
        public ChatController(UserManager<IdentityUser> userManager, MainContext context, IHubContext<ChatHub> hubContext, IWebHostEnvironment appEnvironment, ILogger<ChatController> logger)
        {
            _userManager = userManager;
            _context = context;
            _hubContext = hubContext;
            _appEnvironment = appEnvironment;
            _logger = logger;
        }
        [Route("")]
        [Route("chat")]
        public async Task<IActionResult> Index()
        {
            ChatViewModel model = await GetChatViewModelAsync(_userManager.GetUserId(User));

            if (model is null || model.User is null)
                return RedirectToAction("HttpStatusCodeHandler", "Error", new { StatusCode = 500 });

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
            try
            {

                ChatViewModel model = await GetChatViewModelAsync(_userManager.GetUserId(User));
                model.ChatActive = await GetChatById(Convert.ToInt32(idChat));
                if (model.ChatActive == null)
                {
                    return MessageBoxPartialUpdate().Result;
                }
                Employee userDirect = model.ChatActive.ChatUsers.FirstOrDefault(c => c.Id != model.User.Id);
                if (model.User is null || userDirect == null)
                    return null;

                if (search != null && search != string.Empty)
                {
                    model.Employees = model.Employees.Where(e => e.Fio.Contains(search)).ToList();
                    model.Chats = model.Chats.Where(c => c.ChatUsers
                    .FirstOrDefault(emp => emp.Id != model.User.Id).Fio.Contains(search, StringComparison.OrdinalIgnoreCase)
                    || c.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
                    && c != model.ChatActive)
                        .ToList();
                }

                return PartialView("_messagesBoxPartial", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return MessageBoxPartialUpdate().Result;
            }

        }

        [Route("chat/MessageBoxPartialUpdate")]
        [Route("chat/MessageBoxPartialUpdate/{search}")]
        [HttpGet]
        public async Task<PartialViewResult> MessageBoxPartialUpdate(string search = "")
        {
            try
            {
                ChatViewModel model = await GetChatViewModelAsync(_userManager.GetUserId(User));

                if (model.User is null)
                    return PartialView("_errorPartialView", "Ошибка загрузки сообщений.");

                if (search != null && search != string.Empty)
                {
                    model.Employees = model.Employees.Where(e => e.Fio.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
                    model.Chats = model.Chats.Where(c => c.ChatUsers.FirstOrDefault(emp => emp.Id != model.User.Id).Fio.Contains(search, StringComparison.OrdinalIgnoreCase) || c.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                return PartialView("_messagesBoxPartial", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        [Route("chat/ChatLoad/{idChat}")]
        [HttpGet]
        public async Task<IActionResult> ChatLoad(string idChat)
        {
            try
            {
                ChatViewModel model = await GetChatViewModelAsync(_userManager.GetUserId(User));
                model.ChatActive = await GetChatById(Convert.ToInt32(idChat));


                Employee userDirect = model.ChatActive.ChatUsers
                    .FirstOrDefault(c => c.Id != model.User.Id);

                if (model.ChatActive == null || model.User is null || userDirect == null)
                    return PartialView("_errorLoadPartial", "Ошибка при загрузке диалога.");


                bool isAllMessageLoad = _context.Messages.Include(c => c.Chat).AsSplitQuery()
                    .Count(c => c.Chat.Id == model.ChatActive.Id) <= 50;


                List<Message> messagesRead = new();
                foreach (var messageItem in model.ChatActive.Messages)
                {
                    messageItem.File = await _context.File.FirstOrDefaultAsync(f => f.Id == messageItem.FileId);
                    if (messageItem.User != model.User && messageItem.Status != StatusMessage.Read)
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
                _context.Chats.UpdateRange(model.ChatActive);
                await _context.SaveChangesAsync();

                model.IsAllMessageLoad = isAllMessageLoad;
                model.CountMessageLoad = 50;
                model.UserDirect = userDirect;

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

                ChatViewModel model = await GetChatViewModelAsync(_userManager.GetUserId(User));
                model.ChatActive = await GetChatById(Convert.ToInt32(chatId));


                Employee userDirect = model.ChatActive.ChatUsers
                    .FirstOrDefault(c => c.Id != model.User.Id);

                if (userDirect is null || model is null || model.ChatActive is null)
                    return PartialView("_errorLoadPartial", "Ошибка при загрузке диалога.");

                bool isAllMessageLoad = _context.Messages.Include(c => c.Chat).AsSplitQuery()
                    .Count(c => c.Chat.Id == model.ChatActive.Id) <= countLoadMessageInt + 50;

                var userSend = await _context.Employees
                    .FirstOrDefaultAsync(u => u.Id == model.User.Id);

                model.IsAllMessageLoad = isAllMessageLoad;
                model.CountMessageLoad = countLoadMessageInt + 50;
                model.UserDirect = userDirect;

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
            try
            {
                Chat chat = await _context.Chats.Include(u => u.EmployeeAdministrator).Include(m => m.Messages).Include(u => u.ChatUsers).AsSplitQuery().FirstOrDefaultAsync(c => c.Id == id);
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
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error", "Error", new { statusCode = 501});
            }
        }
        
        public async Task<IActionResult> DeleteUserGroup(int id, string userId)
        {
            try
            {
                Chat chat = await _context.Chats.Include(u => u.ChatUsers).Include(m => m.Messages).Include(u => u.EmployeeAdministrator).AsSplitQuery().FirstOrDefaultAsync(c => c.Id == id);
                if (chat is null || chat.ChatUsers is null || !chat.ChatUsers.Any(u => u.Id == userId)) return RedirectToAction(nameof(Index));
                Employee user = chat.ChatUsers.FirstOrDefault(u => u.Id == userId);
                chat.ChatUsers.Remove(user);
                _context.Chats.Update(chat);
                await _context.SaveChangesAsync();

                await _hubContext.Clients.All.SendAsync("DeleteUserGroup", chat.Id, user.Id)
                        .ConfigureAwait(true);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return RedirectToAction("Error", "Error", new { StatusCode = 501});
            }
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(new { Status = "Error", File = "" });
            }

        }


        private async Task<ChatViewModel> GetChatViewModelAsync(string userId)
        {
            try
            {
                Employee user = await _context.Employees.FirstOrDefaultAsync(emp => emp.Id == userId);

                var chats = await _context.Chats
                .Include(c => c.ChatUsers)
                .Where(c => c.ChatUsers
                    .Any(u => u.Id == user.Id)).AsSplitQuery()
                .ToListAsync();

                foreach (var chatItem in chats)
                {
                    Message lastMessage = await _context.Messages.Include(c => c.Chat).Include(f => f.File).OrderByDescending(d => d.DateTime).AsSplitQuery().FirstOrDefaultAsync(m => m.Chat.Id == chatItem.Id);
                    if (lastMessage != null)
                        chatItem.Messages.Add(lastMessage);
                    else
                        chatItem.Messages = new List<Message>();
                }

                List<Employee> AllEmployees = await _context.Employees
                    .Include(c => c.Chats)
                        .ThenInclude(c => c.ChatUsers)
                        .AsSplitQuery()
                    .ToListAsync();

                List<Employee> employees = AllEmployees.Where(u => u.Id != user.Id
                    && u.Chats.Count(c => c.ChatUsers.Any(emp => emp.Id == user.Id) && c.TypeChat != TypeChat.Group) == 0
                    && !u.Fio.Contains("admin")).ToList();


                ChatViewModel model = new()
                {
                    User = user,
                    Employees = employees,
                    Chats = chats.OrderByDescending(m => m.Messages.LastOrDefault()?.DateTime).ToList(),
                    EmployeesGroup = AllEmployees
                };

                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        private async Task<Chat> GetChatById(int id)
        {
            try
            {
                Chat chat = await _context.Chats.Include(m => m.Messages).ThenInclude(u => u.User).Include(u => u.ChatUsers).AsSplitQuery().FirstOrDefaultAsync(c => c.Id == id);
                return chat;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }

        }
    }
}
