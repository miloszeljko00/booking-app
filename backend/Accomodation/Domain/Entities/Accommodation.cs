using AccomodationDomain.Exceptions.CustomExceptions;
using AccomodationDomain.Primitives;
using AccomodationDomain.Primitives.Enums;
using AccomodationDomain.ValueObjects;
using FluentValidation;

namespace AccomodationDomain.Entities
{
    public class Accommodation : AggregateRoot
    {
        public string Name { get; init; }
        public Address Address { get; init; }
        public List<Price> PricePerGuest { get; init; }
        public List<Benefit> Benefits { get; init; } = new List<Benefit> { };
        public List<Picture> Pictures { get; init; } = new List<Picture> { };
        public Capacity Capacity { get; init; }
        public List<Reservation> Reservations { get; init;} = new List<Reservation> { };
        public List<Reservation> ReservationRequests { get; init; } = new List<Reservation> { };
        public bool ReserveAutomatically { get; init; }
        
        private Accommodation(Guid id, string name, Address address, List<Price> prices, List<Benefit> benefits,
                              List<Picture> pictures, Capacity capacity, List<Reservation> reservations,
                              List<Reservation> reservationRequests, bool reserveAutomatically) : base(id)
        {
            Name = name;
            Address = address;
            PricePerGuest = prices;
            Benefits = benefits;
            Pictures = pictures;
            Capacity = capacity;
            Reservations = reservations;
            ReservationRequests = reservationRequests;
            ReserveAutomatically = reserveAutomatically;
        }
        public static Accommodation Create(Guid id, string name, Address address,
            List<Price> prices, List<Benefit> benefits, List<Picture> pictures,
            Capacity capacity, List<Reservation> reservations,
            List<Reservation> reservationRequests, bool reserveAutomatically)
        {
            var accommodation = new Accommodation(id, name, address, prices,
                benefits, pictures, capacity, reservations,
                reservationRequests, reserveAutomatically);
            var validationResult = CheckIfAccommodationIsValid(accommodation);
            if (validationResult.IsValid)
            {
                return accommodation;
            }
            else
            {
                throw new InvalidAccommodationException();
            }
        }
        /*public double GenerateTotalPricePerNight()
        {
            return PricePerGuest.Value * Capacity.Max;
        }*/
        public void CreateReservation(string email, DateTime start, DateTime end, int numberOfGuests, double price)
        {
            Reservations.Add(Reservation.Create(Guid.NewGuid(), email, start, end, numberOfGuests, price));
        }
        public void CreateReservationRequest(string email, DateTime start, DateTime end, int numberOfGuests, double price)
        {
            ReservationRequests.Add(Reservation.Create(Guid.NewGuid(), email, start, end, numberOfGuests, price));
        }
        public bool CheckIfAccommodationIsUnique(Accommodation accommodation)
        {
            if(accommodation.Address.Equals(this.Address) && accommodation.Name.Equals(this.Name))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private static FluentValidation.Results.ValidationResult CheckIfAccommodationIsValid(Accommodation accommodation)
        {
            var accommodationValidator = new AccommodationValidator();
            return accommodationValidator.Validate(accommodation);
        }
    }
    public class AccommodationBuilder
    {
        public string Name { get; set; }
        public Address Address { get; set; }
        public List<Price> PricePerGuest { get; set; } = new List<Price> { };
        public List<Benefit> Benefits { get; set; } = new List<Benefit> { };
        public List<Picture> Pictures { get; set; } = new List<Picture> { };
        public Capacity Capacity { get; set; }
        public List<Reservation> Reservations { get; set; } = new List<Reservation> { };
        public List<Reservation> ReservationRequests { get; set; } = new List<Reservation> { };
        public bool ReserveAutomatically { get; set; }

        public AccommodationBuilder withName(string name)
        {
            this.Name = name;
            return this;
        }
        public AccommodationBuilder withAddress(string country, string city, string street, string number)
        {
            this.Address = Address.Create(country, city, street, number);
            return this;
        }
        public AccommodationBuilder withPricePerGuest(double value)
        {
            this.PricePerGuest.Add(Price.Create(value));
            return this;
        }
        public AccommodationBuilder withBenefits(List<Benefit> benefits)
        {
            this.Benefits = benefits;
            return this;
        }
        public AccommodationBuilder withPicture(string fileName, string description)
        {
            Pictures.Add(Picture.Create(Guid.NewGuid(), fileName, description));
            return this;
        }
        public AccommodationBuilder withCapacity(int max, int min)
        {
            this.Capacity = Capacity.Create(max,min);
            return this;
        }
        public AccommodationBuilder withAutomaticallyReservation(bool reserveAutomatically)
        {
            ReserveAutomatically = reserveAutomatically;
            return this;
        }
        public Accommodation build()
        {
            return Accommodation.Create(Guid.NewGuid(), Name, Address, PricePerGuest, Benefits,
                Pictures, Capacity, Reservations, ReservationRequests, ReserveAutomatically);
        }
    }
    internal class AccommodationValidator : AbstractValidator<Accommodation>
    {
        public AccommodationValidator()
        {
            RuleFor(v => v.Name).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(v => v.Address).NotNull();
            RuleFor(v => v.PricePerGuest).NotNull();
            RuleFor(v => v.Benefits).NotNull();
            RuleFor(v => v.Benefits).NotEmpty();
            RuleFor(v => v.Pictures).NotNull();
            RuleFor(v => v.Pictures).NotEmpty();
            RuleFor(v => v.Capacity).NotNull();
            RuleFor(v => v.ReserveAutomatically).NotNull();
        }
    }
}
