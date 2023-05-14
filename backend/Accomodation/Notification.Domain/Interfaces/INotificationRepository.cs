using Notification.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Domain.Interfaces
{
    public interface INotificationRepository
    {
        Task<ICollection<GuestNotification>> GetAllAsync();
        Task UpdateAsync(Guid id, GuestNotification updatedGuestNotification);
        Task<GuestNotification> GetAsync(Guid id);
        Task<GuestNotification> Create(GuestNotification guestNotification);
        Task RemoveAsync(Guid id);

    }
}
