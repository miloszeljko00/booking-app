using FlightsBooking.Models;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsBooking.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserService()
        {
            var mongoClient = new MongoClient("mongodb+srv://svenadev:0mSCxqslvdQV46wi@booking-app.udfgsob.mongodb.net/?retryWrites=true&w=majority");

            var mongoDatabase = mongoClient.GetDatabase("FlightsApp");

            _userCollection = mongoDatabase.GetCollection<User>("Users");
        }

        public async Task<User?> GetAsync(Guid id) =>
            await _userCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(User newUser) =>
            await _userCollection.InsertOneAsync(newUser);

        public async Task UpdateAsync(Guid id, User user) =>
            await _userCollection.ReplaceOneAsync(x => x.Id == id, user);

        public async Task RemoveAsync(Guid id) =>
            await _userCollection.DeleteOneAsync(x => x.Id == id);
    }
}
