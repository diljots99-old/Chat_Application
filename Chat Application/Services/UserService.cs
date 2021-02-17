using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat_Application.Models;

namespace Chat_Application.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _Users;

        public UserService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _Users = database.GetCollection<User>(settings.UsersCollectionName);
        }

        public List<User> Get() =>
            _Users.Find(User => true).ToList();

        public User Get(string id) =>
            _Users.Find<User>(Users => Users.Id == id).FirstOrDefault();

        public User GetbyUsername(string username) =>
            _Users.Find<User>(Users => Users.username == username).FirstOrDefault();
        public User Create(User User)
        {
            _Users.InsertOne(User);
            return User;
        }

        public void Update(string id, User UserIn) =>
            _Users.ReplaceOne(User => User.Id == id, UserIn);

        public void Remove(User UserIn) =>
            _Users.DeleteOne(User => User.Id == UserIn.Id);

        public void Remove(string id) =>
            _Users.DeleteOne(User => User.Id == id);
    }
}
