using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace EngeneerLenRooAspNet.Services
{
    public class ChatHub : Hub
    {
        public async Task Send(string user, string userName, string message, string room)
        {
            await Clients.All.SendAsync("Send", room, user, userName, message.Trim(), DateTime.Now.ToShortTimeString())
                .ConfigureAwait(true);
        }
    }
}
