using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Chat_Application.Models
{
    public class Conversation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Messages")]
        public List<Message> Messages { get; set; }

        [BsonElement("Participants")]
        public List<User> Participants { get; set; }



    }
}
