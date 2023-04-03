using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure
{
    public class InMemoryAccommodationOfferRepository : IAccommodationOfferRepository
    {
        public static List<AccommodationOffer> MyList = new List<AccommodationOffer>
        {
            AccommodationOffer.Create(Guid.NewGuid(), "Velja hotel 1", DateTime.UtcNow, DateTime.UtcNow.AddDays(1)),
            AccommodationOffer.Create(Guid.NewGuid(), "Velja hotel 2", DateTime.UtcNow, DateTime.UtcNow.AddDays(2)),
            AccommodationOffer.Create(Guid.NewGuid(), "Velja hotel 3", DateTime.UtcNow, DateTime.UtcNow.AddDays(3)),
        };

        public Task<AccommodationOffer> Create(AccommodationOffer accommodationOffer)
        {
            MyList.Add(accommodationOffer);
            return Task.FromResult(accommodationOffer);
        }

        public async Task<List<AccommodationOffer>> GetAllAsync()
        {
            return await Task.FromResult(MyList);
        }
    }
}