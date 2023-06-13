using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Notification.Domain.Interfaces;
using Notification.Domain.Entities;
using Notification.Infrastructure.Persistance.Settings;
using Microsoft.Extensions.Options;

namespace Notification.Infrastructure.Notification
{
    public class MongoGuestNotificationRepository : IGuestNotificationRepository
    {
        private readonly IMongoCollection<GuestNotification> _notificationCollection;

        public MongoGuestNotificationRepository(IOptions<DatabaseSettings> dbSettings)
        {
            var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);

            _notificationCollection = mongoDatabase.GetCollection<GuestNotification>(dbSettings.Value.GuestNotificationCollectionName);
        }


        public async Task<GuestNotification> Create(GuestNotification notification)
        {
            await _notificationCollection.InsertOneAsync(notification);
            return notification;
        }
        
        public async Task<ICollection<GuestNotification>> GetAllAsync()
        {
            return await _notificationCollection.Find(_ => true).ToListAsync();
        }

        public async Task<GuestNotification> GetAsync(Guid id) =>
            await _notificationCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task UpdateAsync(Guid id, GuestNotification updatedNotification) =>
            await _notificationCollection.ReplaceOneAsync(x => x.Id == id, updatedNotification);

        public async Task RemoveAsync(Guid id) =>
            await _notificationCollection.DeleteOneAsync(x => x.Id == id);
    }
}
