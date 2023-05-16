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
    public class MongoGuestNotificationRepository : IGuestNotificationRepository
    {
        private readonly IMongoCollection<GuestNotification> _notificationCollection;

        public MongoGuestNotificationRepository()
        {
            var mongoClient = new MongoClient("mongodb://user:user@localhost:27017/?authSource=admin");

            var mongoDatabase = mongoClient.GetDatabase("BookingApp");

            _notificationCollection = mongoDatabase.GetCollection<GuestNotification>("GuestNotification");
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
