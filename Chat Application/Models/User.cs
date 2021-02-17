using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Chat_Application.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }

        [BsonElement("First Name")]
        public String First_Name { get; set; }

        [BsonElement("Last Name")]
        public String Last_Name { get; set; }

        [BsonElement("username")]
        public String username { get; set; }
    }
}
