using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat_Application.Areas.Identity.Data;

namespace Chat_Application.Models
{
    public class DashboadViewModel
    {
        public List<Conversation> listOfConversation { get; set; }
        public Conversation selectedConverstaion { get; set; }
        
        public User user { get; set; }
        public DatabaseContext databaseContext { get; set; }
    }
}
