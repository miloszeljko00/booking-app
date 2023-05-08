using AccomodationDomain.Primitives;
using AccomodationDomain.ValueObjects;

namespace AccomodationDomain.Entities
{
    public class AccommodationOffer : AggregateRoot
    {
        public string HotelName { get; init; }
        public DateRange? DateRange { get; init; }
        private AccommodationOffer(
            Guid id,
            string hotelName,
            DateTime start,
            DateTime end
            ) : base(id)
        {
            HotelName = hotelName;
            DateRange = DateRange.Create(start, end);
        }
        public static AccommodationOffer Create(Guid id,
            string hotelName,
            DateTime start,
            DateTime end)
        {
            return new AccommodationOffer(id, hotelName, start, end);
        }

    }
}