using EngeneerLenRooAspNet.Areas.Identity.Data;
using EngeneerLenRooAspNet.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EngeneerLenRooAspNet.Services
{
    public class ChatHub : Hub
    {
        private MainContext _context;
        public ChatHub(MainContext context)
        {
            _context = context;
        }

        public async Task Send(string user, string userName,string message, string room)
        {
            var chat = await _context.Chats
                .Include(u => u.ChatUsers)
                .Include(m => m.Messages)
                .FirstOrDefaultAsync(c => c.Id == Convert.ToInt32(room));
            var userSend = await _context.Employees.FirstOrDefaultAsync(u => u.Id == user);
            if(chat != null && userSend != null && chat.ChatUsers.Contains(userSend))
            {
                var messageModel = new Message()
                {
                    Chat = chat,
                    DateTime = DateTime.Now,
                    User = userSend,
                    Text = message,
                    Status = StatusMessage.NotRead
                };
                await _context.Messages.AddAsync(messageModel);
                await _context.SaveChangesAsync();
                await Clients.All.SendAsync("Send", room, user, userName, messageModel.Id, message.Trim(), DateTime.Now.ToShortTimeString())
                    .ConfigureAwait(true);
            }
            else
            {
                await Clients.All.SendAsync("Send", room, user, userName, "Error send Message", DateTime.Now.ToShortTimeString())
                    .ConfigureAwait(true);
            }
            Read(user, room);
        }
        public async Task Read(string user, string room)
        {
            var chat = await _context.Chats
               .Include(u => u.ChatUsers)
               .Include(m => m.Messages)
               .FirstOrDefaultAsync(c => c.Id == Convert.ToInt32(room));
            var userSend = await _context.Employees.FirstOrDefaultAsync(u => u.Id == user);

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
