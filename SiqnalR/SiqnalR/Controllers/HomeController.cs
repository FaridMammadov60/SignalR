using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SiqnalR.DAL;
using SiqnalR.Models;
using SiqnalR.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SiqnalR.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        //private static string CurUserName;
        private readonly AppDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;



        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, AppDbContext context, IHubContext<ChatHub> hubContext)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Chat()
        {
            List<AppUser> users = _userManager.Users.Where(u => u.ConnectId != null).ToList();
            List<AppUser> userlar = _userManager.Users.ToList();
            ViewBag.ChatUsers = users;
            ViewBag.Userlar = userlar;
            return View();
        }

        public async Task<IActionResult> CreateUser()
        {
            var result1 = await _userManager.CreateAsync(new AppUser { UserName = "_ferid", Email = "feridmma@code.edu.az", isOnline = false, Fullname = "Ferid" }, "12345@Ff");
            var result = await _userManager.CreateAsync(new AppUser { UserName = "_elgun", Email = "elgunpg@code.edu.az", isOnline = false, Fullname = "Elgun" }, "12345@Ff");
            var result2 = await _userManager.CreateAsync(new AppUser { UserName = "_xalid", Email = "khalidsr@code.edu.az", isOnline = false, Fullname = "Xalid" }, "12345@Ff");
            var result3 = await _userManager.CreateAsync(new AppUser { UserName = "_huseyn", Email = "huseynfh@code.edu.az", isOnline = false, Fullname = "Huseyn" }, "12345@Ff");
            var result4 = await _userManager.CreateAsync(new AppUser { UserName = "_tural", Email = "turalkhj@code.edu.az", isOnline = false, Fullname = "Tural" }, "12345@Ff");
            var result5 = await _userManager.CreateAsync(new AppUser { UserName = "_ulvi", Email = "ulvi@code.edu.az", isOnline = false, Fullname = "Ulvi" }, "12345@Ff");
            return Ok("User Created");

        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction ("index");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            var user = _userManager.FindByNameAsync(model.Username).Result;
            
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, true);
            user.isOnline = true;
            _context.SaveChanges();
            ViewBag.UserName = user.Fullname;
            return RedirectToAction("chat", "home");

        }
        public async Task<IActionResult> Logout()
        {            
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> PrivateSend(string id)
        {
            AppUser appUser = await _userManager.FindByIdAsync(id);
           await _hubContext.Clients.Client(appUser.ConnectId).SendAsync("PrivateMessage");
            return RedirectToAction("chat");
        }

        public IActionResult GroupChat()
        {
            List<AppUser> users = _userManager.Users.Where(u => u.ConnectId != null).ToList();
            List<AppUser> userlar = _userManager.Users.ToList();
            ViewBag.ChatUsers = users;
            ViewBag.Userlar = userlar;
            return View();
        }       

    }
}
