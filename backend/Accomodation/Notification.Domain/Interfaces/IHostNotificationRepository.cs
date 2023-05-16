using Notification.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Domain.Interfaces
{
    public interface IHostNotificationRepository
    {
        Task<ICollection<HostNotification>> GetAllAsync();
        Task UpdateAsync(Guid id, HostNotification updatedHostNotification);
        Task<HostNotification> GetAsync(Guid id);
        Task<HostNotification> Create(HostNotification hostNotification);
        Task RemoveAsync(Guid id);

    }
}
