using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Models
{
    public class User
    {
        public ObjectId Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        public List<string> Notes { get; set; }
    }
}
