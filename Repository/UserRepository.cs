using MongoDB.Bson;
using MongoDB.Driver;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class UserRepository
    {
        public static void CreateUser(string username, string password, string description)
        {
            User user = new User()
            {
                UserName = username,
                Password = password,
                Description = description

            };

            var db = new Database();
            db.UserCollection.InsertOne(user);
        }

        public static List<User> GetUsers()
        {
            Database db = new Database();

            return db.UserCollection.Find(u => true).ToList();
        }

        public static void DeleteUserById(string id)
        {
            Database db = new Database();
            
            db.UserCollection.DeleteOne(u => u.Id == id);
        }

        public static void EditUserById(string id, User user)
        {
            Database db = new Database();
            db.EditUserById(id, user);
        }

        public static bool LoginTest(string username, string password)
        {
            Database db = new Database();

            return db.UserCollection.Find(u => u.UserName == username && u.Password == password ).ToList<User>().Any();
        }

        public static void CreateNote(string id, User user)
        {
            Database db = new Database();

            db.NotesById(id, user);

        }

        public static string GetPassword(string password)
        {
            Database db = new Database();
            return db.UserCollection.Find(u => u.Password == password).ToString();
        }

        public static void DeleteAllUsers()
        {
            Database db = new Database();

            db.UserCollection.DeleteMany(u => true);
        }

        public static void SaveManyUsers(List<User> user)
        {
            Database db = new Database();

            db.UserCollection.InsertMany(user);
        }

        public static User GetUserByUsername(string username)
        {
            Database db = new Database();

            return db.UserCollection.Find(u => u.UserName == username).FirstOrDefault();
        }
    }
}
