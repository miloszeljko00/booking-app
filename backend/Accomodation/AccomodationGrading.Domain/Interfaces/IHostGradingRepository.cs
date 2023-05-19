using AccomodationGradingDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationGradingDomain.Interfaces
{
    public interface IHostGradingRepository
    {
        Task<ICollection<HostGrading>> GetAllAsync();
        Task UpdateAsync(Guid id, HostGrading updatedHostGrading);
        Task<HostGrading> GetAsync(Guid id);
        Task<HostGrading> Create(HostGrading hostGrading);
        Task RemoveAsync(Guid id);

    }
}
