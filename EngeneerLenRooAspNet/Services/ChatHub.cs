using EngeneerLenRooAspNet.Areas.Identity.Data;
using EngeneerLenRooAspNet.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
                .Include(m => m.Messages)
                .Include(u => u.ChatUsers)
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

            List<Message> messagesRead = new List<Message>();
            foreach (var messageItem in chat.Messages)
            {
                if (messageItem.User != userSend)
                {
                    messageItem.Status = StatusMessage.Read;
                    messagesRead.Add(messageItem);
                }
            }
            foreach (var messageRead in messagesRead)
            {
                await Clients.All.SendAsync("Read", messageRead.Id)
                    .ConfigureAwait(true);
            }


        }
    }
}
