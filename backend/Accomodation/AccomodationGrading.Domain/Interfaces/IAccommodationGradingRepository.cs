using AccomodationGradingDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationGradingDomain.Interfaces
{
    public interface IAccommodationGradingRepository
    {
        Task<ICollection<AccommodationGrading>> GetAllAsync();
        Task UpdateAsync(Guid id, AccommodationGrading updatedAccommodationGrading);
        Task<AccommodationGrading> GetAsync(Guid id);
        Task<AccommodationGrading> Create(AccommodationGrading accommodationGrading);
        Task RemoveAsync(Guid id);

    }
}
