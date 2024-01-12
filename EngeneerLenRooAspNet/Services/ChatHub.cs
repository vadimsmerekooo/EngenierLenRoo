using EngeneerLenRooAspNet.Areas.Identity.Data;
using EngeneerLenRooAspNet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EngeneerLenRooAspNet.Services
{
    public class ChatHub : Hub
    {
        private readonly MainContext _context;
        public ChatHub(MainContext context)
        {
            _context = context;
        }

        public async Task Send(string user, string userName, string message, string room, string fileId)
        {
            var chat = await _context.Chats
                .Include(u => u.ChatUsers)
                .Include(m => m.Messages)
                .ThenInclude(f => f.File)
                .FirstOrDefaultAsync(c => c.Id == Convert.ToInt32(room));
            var userSend = await _context.Employees.FirstOrDefaultAsync(u => u.Id == user);
            if (chat != null && userSend != null && chat.ChatUsers.Contains(userSend))
            {
                var messageModel = new Message()
                {
                    Chat = chat,
                    DateTime = DateTime.Now,
                    User = userSend,
                    Text = message,
                    Status = StatusMessage.NotRead
                };
                if (!fileId.IsNullOrEmpty())
                {
                    File file = await _context.File.FirstOrDefaultAsync(f => f.Id == fileId);
                    if (file != null)
                    {
                        messageModel.File = file;
                    }
                }
                await _context.Messages.AddAsync(messageModel);
                await _context.SaveChangesAsync();
                if (messageModel.File == null)
                {
                    await Clients.All.SendAsync("Send", room, user, userName, messageModel.Id, message.Trim(),
                        DateTime.Now.ToShortTimeString(), chat.TypeChat is TypeChat.Direct ? "Direct" : "Group",
                        chat.TypeChat is TypeChat.Group ? chat.EmployeeAdministrator.Id : "", Newtonsoft.Json.JsonConvert.SerializeObject(new { IsNull = "null" }))
                        .ConfigureAwait(true);
                }
                else
                {
                    await Clients.All.SendAsync("Send", room, user, userName, messageModel.Id, message.Trim(),
                        DateTime.Now.ToShortTimeString(), chat.TypeChat is TypeChat.Direct ? "Direct" : "Group",
                        chat.TypeChat is TypeChat.Group ? chat.EmployeeAdministrator.Id : "",
                        Newtonsoft.Json.JsonConvert.SerializeObject( new { IsNull = "exsist", Path = messageModel.File.Path, Name = messageModel.File.OriginalName, Type = messageModel.File.TypeFile.ToString(), Size = messageModel.File.GetSizeToString()}))
                        .ConfigureAwait(true);
                }
            }
            else
            {
                await Clients.All.SendAsync("Send", room, user, userName, "Error send Message", DateTime.Now.ToShortTimeString(), chat.TypeChat is TypeChat.Direct ? "Direct" : "Group", chat.TypeChat is TypeChat.Group ? chat.EmployeeAdministrator.Id : "")
                    .ConfigureAwait(true);
            }
            await Read(user, room);
        }
        public async Task Read(string user, string room)
        {
            var chat = await _context.Chats
               .Include(u => u.ChatUsers)
               .Include(m => m.Messages)
               .FirstOrDefaultAsync(c => c.Id == Convert.ToInt32(room));
            var userSend = await _context.Employees.FirstOrDefaultAsync(u => u.Id == user);

            List<Message> messagesRead = new();
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
                await Clients.All.SendAsync("Read", messageRead.Id, room)
                    .ConfigureAwait(true);
            }
            _context.Chats.UpdateRange(chat);
            await _context.SaveChangesAsync();
        }
        public async Task Print(string room, string user)
        {
            await Clients.All.SendAsync("Print", room, user)
                .ConfigureAwait(true);
        }
    }
}
