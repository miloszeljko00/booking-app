using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Interfaces;
using UserManagement.Infrastructure.Persistance.Settings;

namespace UserManagement.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IOptions<DatabaseSettings> dbSettings)
        {
            var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);

            _users = mongoDatabase.GetCollection<User>(dbSettings.Value.CollectionName);
        }


        public async Task<User> CreateAsync(User user)
        {
            await _users.InsertOneAsync(user);
            return user;
        }

        public async Task<ICollection<User>> GetAllAsync()
        {
            return await _users.Find(_ => true).ToListAsync();
        }

        public async Task<User> GetAsync(Guid id) =>
            await _users.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task UpdateAsync(Guid id, User user) =>
            await _users.ReplaceOneAsync(x => x.Id == id, user);

        public async Task RemoveAsync(Guid id) =>
            await _users.DeleteOneAsync(x => x.Id == id);
    }
}
