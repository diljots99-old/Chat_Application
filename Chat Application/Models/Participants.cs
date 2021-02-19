using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Chat_Application.Areas.Identity.Data;

namespace Chat_Application.Models
{
    public class Participants
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public Conversation Conversation { get; set; }
        public  User User { get; set; }
    }
}
