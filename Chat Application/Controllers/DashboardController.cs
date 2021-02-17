using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat_Application.Models;
using Newtonsoft.Json;
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
using MongoDB.Driver;
using MongoDB.Bson;

namespace Chat_Application.Controllers
{
    public  class DashboardController : Controller
    {
        User _user;
        private readonly ILogger<DashboardController> _logger;
        private readonly ConversationService _conversationService;
        private readonly UserService _userService;
        public DashboardController(ILogger<DashboardController> logger, ConversationService conversationService, UserService userService)
        {
            _conversationService = conversationService;
            _userService = userService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            User user;
            string selectedConversationID ="";
            _user =  user = JsonConvert.DeserializeObject<User>((string)TempData["user"]);
            _user = (User)ViewData["user"];
            ViewData["user"] = user;


            List<Conversation> listOfConversation = _conversationService.Get(user);

            DashboadViewModel dashboadViewModel = new DashboadViewModel();
            dashboadViewModel.listOfConversation = listOfConversation;
            dashboadViewModel.user = user;
            if (!string.IsNullOrEmpty(selectedConversationID))
            {
                Conversation selectedConversation = _conversationService.Get(selectedConversationID);
                ViewData["selectedConversation"] = selectedConversation;
                dashboadViewModel.selectedConverstaion = selectedConversation;
            }
            else
            {
                ViewData["selectedConversation"] = listOfConversation[0];
                dashboadViewModel.selectedConverstaion = listOfConversation[0]; 
            }

            return View(dashboadViewModel);
        }

        [HttpPost]
        public IActionResult Index(IFormCollection data)
        {
            User user = _userService.Get(data["currentUserId"]);
            Conversation conversation = _conversationService.Get(data["conversationId"]);

            ViewData["selectedConversation"] = conversation;

            List<Conversation> listOfConversation = _conversationService.Get(user);

            DashboadViewModel dashboadViewModel = new DashboadViewModel();
            dashboadViewModel.listOfConversation = listOfConversation;
            dashboadViewModel.user = user;
            dashboadViewModel.selectedConverstaion = conversation;
            dashboadViewModel.selectedConverstaion.Messages = conversation.Messages;



            return View(dashboadViewModel);
        }
        public IActionResult Send_Message(IFormCollection data)
        {

            User user = _userService.Get(data["currentUserId"]);
            Conversation conversation = _conversationService.Get(data["currentconversationId"]);
            string messageContent = data["messageContent"];
            if (!string.IsNullOrEmpty(messageContent)){
                Message newMessage = new Message() { messageContent = messageContent, Sender = user, messageSent = DateTime.UtcNow, messageDelivered = DateTime.UtcNow };

                conversation.Messages.Add(newMessage);

                _conversationService.Update(conversation.Id, conversation);
            }
            List<Conversation> listOfConversation = _conversationService.Get(user);

            DashboadViewModel dashboadViewModel = new DashboadViewModel();
            dashboadViewModel.listOfConversation = listOfConversation;
            dashboadViewModel.user = user;
            dashboadViewModel.selectedConverstaion = conversation;




            return View("Index",dashboadViewModel);

        }

        public IActionResult New_Conversation( IFormCollection data)
        {
            string username = data["username"];
            string messageContent = data["message"];

            _user = _userService.Get(data["userId"]);

            User recipet = _userService.GetbyUsername(username);

            List<User> participants = new List<User>();
            participants.Add(_user);
            participants.Add(recipet);

            Message message = new Message();
            message.messageContent = messageContent;
            message.Sender = _user;
            message.messageSent = DateTime.UtcNow;
            message.messageDelivered   = DateTime.UtcNow;

            List<Message> listofMessage = new List<Message>();

            listofMessage.Add(message);




            Conversation conversation = new Conversation();
            conversation.Participants = participants;
            conversation.Messages = listofMessage;

            conversation = _conversationService.Create(conversation);



            return Content(conversation.Id);
        }
    }
}
