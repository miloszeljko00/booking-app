using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Notification.Domain.Interfaces;
using Notification.Domain.Entities;

namespace Notification.Infrastructure.Notification
{
    public class MongoHostNotificationRepository : IHostNotificationRepository
    {
        private readonly IMongoCollection<HostNotification> _notificationCollection;

        public MongoHostNotificationRepository()
        {
            var mongoClient = new MongoClient("mongodb://user:user@localhost:27017/?authSource=admin");

            var mongoDatabase = mongoClient.GetDatabase("BookingApp");

            _notificationCollection = mongoDatabase.GetCollection<HostNotification>("HostNotification");
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
