using ISSF2020.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISSF2020.Services
{
    public class UserService
    {

        private readonly IMongoCollection<UserModel> _user;

        public UserService(IUserDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _user = database.GetCollection<UserModel>(settings.UserCollectionName);
        }

        public List<UserModel> Get() =>
            _user.Find(user => true).ToList();

        public UserModel Get(string username) =>
            _user.Find<UserModel>(user => user.Username == username).FirstOrDefault();

        public bool CheckPass(string username, string inputPassword)
        {
            var user = _user.Find<UserModel>(user => user.Username == username).FirstOrDefault();

            if (user.Password == inputPassword)
            {
                return true;
            }
            return false; 
        }

        public UserModel Create(UserModel user)
        {
            _user.InsertOne(user);
            return user;
        }

        public void Update(string id, UserModel userIn) =>
            _user.ReplaceOne(user => user.Id == id, userIn);

        public void Remove(UserModel userIn) =>
            _user.DeleteOne(user => user.Id == userIn.Id);

        public void Remove(string id) =>
            _user.DeleteOne(user => user.Id == id);
    }
}
