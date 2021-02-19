using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Chat_Application.Areas.Identity.Data;

namespace Chat_Application.Models
{
    public class Messages
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime messageSent { get; set; }
        public DateTime messageDelivered { get; set; }
        public  User Sender { get; set; }

        public Conversation Conversation { get; set; }

        public string messageContent { get; set; }
    }
}
