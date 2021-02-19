using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Chat_Application.Areas.Identity.Data;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Chat_Application.Models
{
    public class Conversation
    {
       
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
        public DateTime Deleted_at { get; set; }
        public List<Messages> Messages { get; set; }

        public List<Participants> Participants { get; set; }

     
        public  User CreatorUser { get; set; }


    }
}
