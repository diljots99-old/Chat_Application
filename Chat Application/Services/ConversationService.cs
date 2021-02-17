using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat_Application.Models;
using MongoDB.Bson;
using Chat_Application.Models;

namespace Chat_Application.Services
{
    public class ConversationService
    {

        private readonly IMongoCollection<Conversation> _conversations;

        public ConversationService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _conversations = database.GetCollection<Conversation>(settings.ConversationsCollectionName);
        }

        public ConversationService(string databaseName,string connectionString)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            _conversations = database.GetCollection<Conversation>("conversations");
        }
        public List<Conversation> Get() =>
            _conversations.Find(Conversation => true).ToList();

        public Conversation Get(string id) =>
            _conversations.Find<Conversation>(conversations => conversations.Id == id).FirstOrDefault();

        public List<Conversation> Get(User user) {
            List<User> listOfUser = new List<User>();
            listOfUser.Add(user);
            var filter = Builders<Conversation>.Filter.AnyIn("Participants", listOfUser );
            return  _conversations.Find(filter).ToList();
        }
    public Conversation Create(Conversation Conversation)
        {
            _conversations.InsertOne(Conversation);
            
            return Conversation;
        }

        public void Update(string id, Conversation ConversationIn) =>
            _conversations.ReplaceOne(conversation => conversation.Id == id, ConversationIn);

        public void Remove(Conversation ConversationIn) =>
            _conversations.DeleteOne(conversation => conversation.Id == ConversationIn.Id);

        public void Remove(string id) =>
            _conversations.DeleteOne(conversation => conversation.Id == id);
    }
}
