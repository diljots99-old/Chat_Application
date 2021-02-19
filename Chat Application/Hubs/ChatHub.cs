using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat_Application.Areas.Identity.Data;
using Chat_Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Chat_Application.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<ChatApplicationUser> _userManager;
        public ChatHub(UserManager<ChatApplicationUser> userManager)
        {
            _userManager = userManager;
        }



        public async Task SendMessage(string conversationId, string userId,string messageContent)
        {
            DatabaseContext dbContext = new DatabaseContext();
            User user = dbContext.Users.Find(userId);
            Conversation conversation = dbContext.Conversations.Include(c => c.Participants).ThenInclude(p => p.User)
                .Where(c => c.Id == conversationId).FirstOrDefault();

            List<string> usersList = new List<string>();
            foreach (var participant in conversation.Participants)
            {
                usersList.Add(participant.User.Id);   
            }

            Messages newMessage = new Messages() { messageContent = messageContent, Sender = user, Conversation = conversation, messageSent = DateTime.Now, messageDelivered = DateTime.Now };

            if (!string.IsNullOrEmpty(messageContent))
            {
                
                dbContext.Messages.Add(newMessage);

                dbContext.SaveChanges();

            }

            string data = JsonConvert.SerializeObject(new { messageContent = messageContent,senderName = newMessage.Sender.First_Name,messageSent=newMessage.messageSent.ToLongDateString()});
            await Clients.Users(usersList).SendAsync("ReceiveMessage", conversationId, userId, data);

        }
    }
}
