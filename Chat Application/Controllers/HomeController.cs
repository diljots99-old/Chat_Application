using Chat_Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Chat_Application.Services;
using Chat_Application.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;

namespace Chat_Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ConversationService _conversationService;
        private readonly UserService _userService;
        private readonly UserManager<User> _userManager;


        public HomeController(ILogger<HomeController> logger,ConversationService conversationService,UserService  userService)
        {
            _conversationService = conversationService;
            _userService = userService;
            _logger = logger;

        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(IFormCollection data)
        {
            string username = data["username"];


            User user = _userService.GetbyUsername(username);

            if (user == null)
            {
                
                return Content(" User Does Not Exits");


            }
            else
            {
                TempData["user"] = JsonConvert.SerializeObject(user);
                ViewData["user"] = user;

              

                return RedirectToAction("Index", "Dashboard",user );
            }

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
    
        public IActionResult Create_Account(User user)
        {


            if (ModelState.IsValid)
            {
                _userService.Create(user);
                return RedirectToAction("Index");

            }
            return View(user);
        }

        public IActionResult Create_Account()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
