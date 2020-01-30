using Example.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example.Hubs
{
    public class ChatHub : Hub
    {
        static List<User> Users = new List<User>();

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public override async Task OnConnectedAsync()
        {
            string name = Context.User.Identity.Name;

            if (Users.Count(x => x.UserName == name) == 0)
            {
                Users.Add(new User { UserName = name });

                await Clients.Others.SendAsync("ConnectAll", Context.User.Identity.Name);

                await Clients.Caller.SendAsync("ConnectUser", Users);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var item = Users.FirstOrDefault(x => x.UserName == Context.User.Identity.Name);
            if (item != null)
            {
                Users.Remove(item);

                await Clients.All.SendAsync("Disconnect", Context.User.Identity.Name);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
