using Domain.Entities;
using Domain.Interfaces;
using Domain.Primitives.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Accommodation
{
    public class InMemoryAccommodationRepository : IAccommodationRepository
    {
        public static AccommodationBuilder AccommodationBuilder { get; set; } = new AccommodationBuilder();
        public InMemoryAccommodationRepository()
        {
        }

        public static List<Domain.Entities.Accommodation> MyList = new List<Domain.Entities.Accommodation>
        {
            //Domain.Entities.Accommodation.Create(Guid.NewGuid(), "Velja hotel 1", "Velja drzava 1", DateTime.UtcNow.AddDays(1)),
            //Domain.Entities.Accommodation.Create(Guid.NewGuid(), "Velja hotel 2", DateTime.UtcNow, DateTime.UtcNow.AddDays(2)),
            //Domain.Entities.Accommodation.Create(Guid.NewGuid(), "Velja hotel 3", DateTime.UtcNow, DateTime.UtcNow.AddDays(3)),
            AccommodationBuilder.withAddress("Velja drzava 1", "Velja grad 1", "Velja ulica 1", "Velja broj 1 :)")
                                .withPricePerGuest(999.99)
                                .withName("Velja smestaj 1")
                                .withPicture("velja_slika.jpg", "velja opis slike")
                                .withCapacity(8, 4)
                                .withBenefits(new List<Benefit>
                                {
                                    Benefit.WI_FI,
                                    Benefit.KITCHEN,
                                    Benefit.AIR_CONDITIONING
                                })
                                .withAutomaticallyReservation(false)
                                .build(),
            AccommodationBuilder.withAddress("Velja drzava 2", "Velja grad 2", "Velja ulica 2", "Velja broj 2 :/")
                                .withPricePerGuest(888.88)
                                .withName("Velja smestaj 2")
                                .withPicture("velja_slika.jpg", "velja opis slike")
                                .withCapacity(8, 4)
                                .withBenefits(new List<Benefit>
                                {
                                    Benefit.WI_FI,
                                    Benefit.KITCHEN,
                                    Benefit.AIR_CONDITIONING
                                })
                                .withAutomaticallyReservation(false)
                                .build(),
            AccommodationBuilder.withAddress("Velja drzava 3", "Velja grad 3", "Velja ulica 3", "Velja broj 3 :/")
                                .withPricePerGuest(777.77)
                                .withName("Velja smestaj 3")
                                .withPicture("velja_slika.jpg", "velja opis slike")
                                .withCapacity(8, 4)
                                .withBenefits(new List<Benefit>
                                {
                                    Benefit.WI_FI,
                                    Benefit.KITCHEN,
                                    Benefit.AIR_CONDITIONING
                                })
                                .withAutomaticallyReservation(false)
                                .build()
        };

        public Task<Domain.Entities.Accommodation> Create(Domain.Entities.Accommodation accommodation)
        {
            MyList.Add(accommodation);
            return Task.FromResult(accommodation);
        }

        public Task<IReadOnlyCollection<Domain.Entities.Accommodation>> GetAllAsync()
        {
            IReadOnlyCollection<Domain.Entities.Accommodation> list = new ReadOnlyCollection<Domain.Entities.Accommodation>(MyList);
            return Task.FromResult(list);
        }
    }
}
