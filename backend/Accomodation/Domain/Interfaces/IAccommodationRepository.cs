using AccomodationSuggestionDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationSuggestionDomain.Interfaces
{
    public interface IAccommodationRepository
    {
        Task<ICollection<Accommodation>> GetAllAsync();
        Task UpdateAsync(Guid id, Accommodation updatedAccommodation);
        Task<Accommodation> GetAsync(Guid id);
        Task<Accommodation> Create(Accommodation accommodation);
        Task RemoveAsync(Guid id);

    }
}
