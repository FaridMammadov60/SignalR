
using Microsoft.AspNetCore.Identity;
using System;

namespace SiqnalR.Models
{
    public class AppUser : IdentityUser
    {

        public string Fullname { get; set; }
        public string ConnectId { get; set; }
        public bool isOnline { get; set; }

    }
}
