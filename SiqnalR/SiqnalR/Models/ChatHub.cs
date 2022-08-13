using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SiqnalR.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiqnalR.Models
{
    public class ChatHub:Hub
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public ChatHub(UserManager<AppUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message, DateTime.Now.ToString("dd.MM.yyyy"));

            //await Clients.Others.SendAsync("ReceiveMessage", user, message);
            // await Clients.Caller.SendAsync("ReceiveMessage", user, message);
            //await Clients.Users.(user).SendAsync("ReceiveMessage", message);
            //await Clients.Group("p322").SendAsync("Group");
        }
        public override Task OnConnectedAsync()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                AppUser user = _userManager.FindByNameAsync(Context.User.Identity.Name).Result;
                user.ConnectId = Context.ConnectionId;
                _context.SaveChanges();
                Clients.All.SendAsync("Userconnect", user.Id);
            }
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                AppUser user = _userManager.FindByNameAsync(Context.User.Identity.Name).Result;
                user.ConnectId = null;
                _context.SaveChanges();
                Clients.All.SendAsync("Userdisconnect", user.Id);
            }
            return base.OnDisconnectedAsync(exception);
        }

 
    }
}
