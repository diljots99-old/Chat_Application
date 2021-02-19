using Chat_Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Chat_Application.Areas.Identity.Data;
using Chat_Application.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Chat_Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ChatApplicationUser> _userManager;
        public HomeController(ILogger<HomeController> logger, UserManager<ChatApplicationUser> userManager)
        {
           
            _logger = logger;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            DatabaseContext dbContext = new DatabaseContext();

            ChatApplicationUser currentUser = await GetCurrentUserAsync();
            User user = CreateUser(currentUser);

            List<Conversation> listofConversations = new List<Conversation>();

            try
            {
                List<Participants> participants = dbContext.Participants.Where(p => p.User.Id == user.Id)
                    .Include(participant=> participant.User).Include(participant => participant.Conversation).ThenInclude(conversaion => conversaion.Messages).Include(conversation => conversation.Conversation.Participants ).ToList();

                foreach (var participant in participants)
                {
                    if (participant != null)
                    {
                        listofConversations.Add(participant.Conversation);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
            DashboadViewModel dashboadViewModel = new DashboadViewModel();
            dashboadViewModel.listOfConversation = listofConversations;
            dashboadViewModel.user = user;
            dashboadViewModel.databaseContext = new DatabaseContext();

            if (TempData["selectedConversationId"] != null) { 
            Conversation selectedConversation = dbContext.Conversations.Find(TempData["selectedConversationId"].ToString());

            dbContext.Entry(selectedConversation).Collection(c => c.Participants).Load();
            dbContext.Entry(selectedConversation).Collection(c => c.Messages).Load();
            dbContext.Entry(selectedConversation).Reference(c => c.CreatorUser).Load();
            dashboadViewModel.selectedConverstaion =selectedConversation;
            }
            return View(dashboadViewModel);
        }
        private Task<ChatApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        
        [HttpPost]
        public async Task<IActionResult> Index(IFormCollection data)
        {
            DatabaseContext dbContext = new DatabaseContext();

            ChatApplicationUser currentUser = await GetCurrentUserAsync();
            Conversation conversation = dbContext.Conversations.Find(data["conversationId"]);
            
            dbContext.Entry(conversation).Collection(c => c.Participants).Load();
            dbContext.Entry(conversation).Collection(c => c.Messages).Load();
            dbContext.Entry(conversation).Reference(c => c.CreatorUser).Load();


            ViewData["selectedConversation"] = conversation;

            User user = dbContext.Users.Find(currentUser.Id);
            List<Conversation> listOfConversation = new List<Conversation>();

            try
            {
                List<Participants> participants = dbContext.Participants.Where(p => p.User.Id == user.Id)
                    .Include(participant => participant.User).Include(participant => participant.Conversation).ThenInclude(conversaion => conversaion.Messages).Include(conversation => conversation.Conversation.Participants).ToList();

                foreach (var participant in participants)
                {
                    if (participant != null)
                    {
                        listOfConversation.Add(participant.Conversation);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            DashboadViewModel dashboadViewModel = new DashboadViewModel();
            dashboadViewModel.listOfConversation = listOfConversation;
            dashboadViewModel.user = user;
            dashboadViewModel.selectedConverstaion = conversation;
            dashboadViewModel.databaseContext = new DatabaseContext();
            
            return View(dashboadViewModel);
        }
        public async Task<IActionResult> Send_Message(IFormCollection data)
        {
            DatabaseContext dbContext = new DatabaseContext();

            ChatApplicationUser currentUser = await GetCurrentUserAsync();
            Conversation conversation = dbContext.Conversations.Find(data["currentConversationId"]);

            dbContext.Entry(conversation).Collection(c => c.Participants).Load();
            dbContext.Entry(conversation).Collection(c => c.Messages).Load();
            dbContext.Entry(conversation).Reference(c => c.CreatorUser).Load();


            ViewData["selectedConversation"] = conversation;

            User user = dbContext.Users.Find(currentUser.Id);
            List<Conversation> listOfConversation = new List<Conversation>();

            try
            {
                List<Participants> participants = dbContext.Participants.Where(p => p.User.Id == user.Id)
                    .Include(participant => participant.User).Include(participant => participant.Conversation).ThenInclude(conversaion => conversaion.Messages).Include(conversation => conversation.Conversation.Participants).ToList();

                foreach (var participant in participants)
                {
                    if (participant != null)
                    {
                        listOfConversation.Add(participant.Conversation);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            DashboadViewModel dashboadViewModel = new DashboadViewModel();
            dashboadViewModel.listOfConversation = listOfConversation;
            dashboadViewModel.user = user;
            dashboadViewModel.selectedConverstaion = conversation;
            dashboadViewModel.databaseContext = new DatabaseContext();
            TempData["selectedConversationId"] = conversation.Id;

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> New_Conversation(IFormCollection data)
        {

            string recipentUserName = data["recipentUserName"];
            string creatorId = data["creatorId"];
            string creatorUserName = data["creatorUserName"];

            IdentityDBContext identityDbContext = new IdentityDBContext();
            DatabaseContext databaseContext = new DatabaseContext();
            
            
            ChatApplicationUser cauRecipentUser  = identityDbContext.ChatApplicationUsers.Where(ChatApplicationUser => ChatApplicationUser.UserName == recipentUserName).FirstOrDefault(); ; 
            ChatApplicationUser cauCurrentUser = await GetCurrentUserAsync();

            User currentUser  = databaseContext.Users.Find(cauCurrentUser.Id);
            User  recipentUser = databaseContext.Users.Find(cauRecipentUser.Id);
            if (recipentUser == null)
            {
                recipentUser = CreateUser(cauRecipentUser);
                databaseContext.Users.Add(recipentUser);
                databaseContext.SaveChanges();
            }
            if (currentUser == null)
            {
                currentUser = CreateUser(cauCurrentUser);
                databaseContext.Users.Add(currentUser);
                databaseContext.SaveChanges();
            }

            List<Participants> existingCurrentParticipants = databaseContext.Participants.Where(p => p.User.Id == currentUser.Id).Include(p => p.Conversation).Include(p => p.User).ToList();
            List<Participants> existingRecipientParticipants = databaseContext.Participants.Where(p => p.User.Id == recipentUser.Id).Include(p => p.Conversation).Include(p => p.User).ToList();

            Conversation conversation;

            DashboadViewModel dashboadViewModel;
            foreach (var recipent in existingRecipientParticipants)
            {
                foreach (var current in existingCurrentParticipants)
                {
                    if (current.Conversation.Id == recipent.Conversation.Id)
                    {
                        // existing Conversation 
                        conversation = current.Conversation;

                        dashboadViewModel = new DashboadViewModel();
                        dashboadViewModel.user = currentUser;
                        dashboadViewModel.databaseContext = new DatabaseContext();
                        
                        //return Content($"existing conversation with conversation id: {conversation.Id}");
                        return RedirectToAction("Index", dashboadViewModel);
                    }
                }   
            }

            Conversation newConversation = new Conversation();

            newConversation.CreatorUser = currentUser;
            newConversation.Created_at = DateTime.Now;
            newConversation.Updated_at= DateTime.Now;

            databaseContext.Conversations.Add(newConversation);
            databaseContext.SaveChanges();
            
            Participants currentParticipant = new Participants() { User = currentUser, Conversation = newConversation };
            Participants recipientParticipant = new Participants() { User = recipentUser, Conversation = newConversation };

            databaseContext.Participants.Add(currentParticipant);
            databaseContext.Participants.Add(recipientParticipant);

            databaseContext.SaveChanges();

            dashboadViewModel = new DashboadViewModel();
            dashboadViewModel.user = currentUser;
            dashboadViewModel.databaseContext = new DatabaseContext();
            //return Red(newConversation.Id);
            return RedirectToAction("Index", dashboadViewModel);

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


        public User CreateUser(ChatApplicationUser caUser)
        {
            User user = new User();
            user.Id = caUser.Id;
            user.First_Name = caUser.First_Name;
            user.Last_Name = caUser.Last_Name;
            user.DOB = caUser.DOB;
            user.UserName = caUser.UserName;
            user.Email = caUser.Email;
            user.PhoneNumber = caUser.PhoneNumber;
            return user;

        }
    }
}
