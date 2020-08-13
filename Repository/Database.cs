using MongoDB.Bson;
using MongoDB.Driver;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    

    class Database
    {
        public IMongoCollection<User>UserCollection { get; private set; }
        private readonly IMongoDatabase _database;
        private const string USERS_COLLECTION = "users";

        public Database()
        {
           MongoClient Client = new MongoClient();
            _database = Client.GetDatabase("it-sak-app");
            UserCollection = _database.GetCollection<User>(USERS_COLLECTION);
        }


        public void EditUserById(ObjectId id, User user)
        {
            UserCollection = _database.GetCollection<User>(USERS_COLLECTION);

            UpdateDefinition<User> update = Builders<User>.Update
                .Set(u => u.Description, user.Description);

            UserCollection.UpdateOne(u => u.Id == id, update);
        }

        internal void NotesById(ObjectId id, User user)
        {
            UserCollection = _database.GetCollection<User>(USERS_COLLECTION);

            UpdateDefinition<User> update = Builders<User>.Update
               .Set(u => u.Notes, user.Notes);

            UserCollection.UpdateOne(u => u.Id == id, update);

        }
    }
}
