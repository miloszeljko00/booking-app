using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Notification.Domain.Interfaces;
using Notification.Domain.Entities;
using Microsoft.Extensions.Options;
using Notification.Infrastructure.Persistance.Settings;

namespace Notification.Infrastructure.Notification
{
    public class MongoHostNotificationRepository : IHostNotificationRepository
    {
        private readonly IMongoCollection<HostNotification> _notificationCollection;

        public MongoHostNotificationRepository(IOptions<DatabaseSettings> dbSettings)
        {
            var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);

            _notificationCollection = mongoDatabase.GetCollection<HostNotification>(dbSettings.Value.HostNotificationCollectionName);
        }


        public async Task<HostNotification> Create(HostNotification notification)
        {
            await _notificationCollection.InsertOneAsync(notification);
            return notification;
        }
        
        public async Task<ICollection<HostNotification>> GetAllAsync()
        {
            return await _notificationCollection.Find(_ => true).ToListAsync();
        }

        public async Task<HostNotification> GetAsync(Guid id) =>
            await _notificationCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task UpdateAsync(Guid id, HostNotification updatedNotification) =>
            await _notificationCollection.ReplaceOneAsync(x => x.Id == id, updatedNotification);

        public async Task RemoveAsync(Guid id) =>
            await _notificationCollection.DeleteOneAsync(x => x.Id == id);
    }
}
