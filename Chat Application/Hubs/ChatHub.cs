using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat_Application.Models;
using Chat_Application.Services;
using Microsoft.AspNetCore.SignalR;

namespace Chat_Application.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ConversationService _conversationService;
        private readonly UserService _userService;

        public ChatHub(ConversationService conversationService, UserService userService)
        {
            _conversationService = conversationService;
            _userService = userService;
        }


        public  async Task SendMessage(string conversationId, string userId,string messageContent)
        {
            User user = _userService.Get(userId);
            Conversation conversation = _conversationService.Get(conversationId);

            if (!string.IsNullOrEmpty(messageContent))
            {
                Message newMessage = new Message() { messageContent = messageContent, Sender = user, messageSent = DateTime.UtcNow, messageDelivered = DateTime.UtcNow };

                conversation.Messages.Add(newMessage);

                _conversationService.Update(conversation.Id, conversation);
            }
            await Clients.All.SendAsync("ReceiveMessage",  conversationId, userId,messageContent);
        }
    }
}
