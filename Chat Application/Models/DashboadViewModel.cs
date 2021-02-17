using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat_Application.Models
{
    public class DashboadViewModel
    {
        public List<Conversation> listOfConversation { get; set; }
        public Conversation selectedConverstaion { get; set; }
        
        public User user { get; set; }
    }
}
