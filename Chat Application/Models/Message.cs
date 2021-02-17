using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat_Application.Models
{
    public class Message
    {
        public int Id { get; set; }
        public DateTime messageSent { get; set; }
        public DateTime messageDelivered { get; set; }
        public User Sender { get; set; }
        public string messageContent { get; set; }
    }
}
