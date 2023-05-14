using Accomodation.Domain.Primitives.Enums;
using AccomodationSuggestionDomain.Exceptions.CustomExceptions;
using AccomodationSuggestionDomain.Primitives;
using AccomodationSuggestionDomain.Primitives.Enums;
using AccomodationSuggestionDomain.ValueObjects;
using FluentValidation;
using System;
using System.Diagnostics;

namespace AccomodationSuggestionDomain.Entities
{
    public class Accommodation : AggregateRoot
    {
        public string Name { get; init; }
        public Address Address { get; init; }
        public PriceCalculation PriceCalculation { get; init; }
        public List<Price> PricePerGuest { get; init; }
        public List<Benefit> Benefits { get; init; } = new List<Benefit> { };
        public List<Picture> Pictures { get; init; } = new List<Picture> { };
        public Capacity Capacity { get; init; }
        public List<Reservation> Reservations { get; init;} = new List<Reservation> { };
        public List<ReservationRequest> ReservationRequests { get; init; } = new List<ReservationRequest> { };
        public bool ReserveAutomatically { get; init; }
        public Email HostEmail { get; init; }
        
        private Accommodation(Guid id, string name, Address address, PriceCalculation priceCalculation,
            List<Price> prices, List<Benefit> benefits,List<Picture> pictures, Capacity capacity,
            List<Reservation> reservations, List<ReservationRequest> reservationRequests, bool reserveAutomatically, Email hostEmail) : base(id)
        {
            Name = name;
            Address = address;
            PricePerGuest = prices;
            Benefits = benefits;
            Pictures = pictures;
            Capacity = capacity;
            Reservations = reservations;
            PriceCalculation = priceCalculation;
            ReservationRequests = reservationRequests;
            ReserveAutomatically = reserveAutomatically;
            HostEmail = hostEmail;
        }
        public static Accommodation Create(Guid id, string name, Address address, PriceCalculation priceCalculation,
            List<Price> prices, List<Benefit> benefits, List<Picture> pictures,
            Capacity capacity, List<Reservation> reservations,
            List<ReservationRequest> reservationRequests, bool reserveAutomatically, Email hostEmail)
        {
            var accommodation = new Accommodation(id, name, address, priceCalculation, prices,
                benefits, pictures, capacity, reservations,
                reservationRequests, reserveAutomatically, hostEmail);
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
        public void CreateReservation(string email, DateTime start, DateTime end, int numberOfGuests, bool isPerPerson, int price, bool isCanceled)
        {
            Reservations.Add(Reservation.Create(Guid.NewGuid(), email, start, end, numberOfGuests, isPerPerson, price, isCanceled));
        }
        public void CreateReservationRequest(string email, DateTime start, DateTime end, int numberOfGuests, ReservationRequestStatus status)
        {
            ReservationRequests.Add(ReservationRequest.Create(Guid.NewGuid(), email, start, end, numberOfGuests, status));
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
        public int GetReservationCountAfterDate(DateTime date)
        {
            return Reservations.Where(reservation => reservation.ReservationDate.End > date && reservation.IsCanceled is false).ToList().Count;
        }
        public Price? GetPriceForSpecificDate(DateTime date)
        {
            foreach(Price p in PricePerGuest)
            {
                DateTime startFixedYear = new DateTime(2023, p.DateRange.Start.Month, p.DateRange.Start.Day);
                DateTime endFixedYear = new DateTime(2023, p.DateRange.End.Month, p.DateRange.End.Day);
                DateTime checkDateFixedYear = new DateTime(2023, date.Month, date.Day);

                if (checkDateFixedYear >= startFixedYear && checkDateFixedYear <= endFixedYear)
                {
                    return p;
                }
            }
            return null;
        }

        public bool IsReservationDateRangeTaken(DateRange dateRange)
        {
            foreach (Reservation r in Reservations)
            {
                if (r.IsCanceled)
                    continue;
                if (r.ReservationDate.Start.CompareTo(dateRange.Start) >= 0 && r.ReservationDate.Start.CompareTo(dateRange.End) <= 0)
                {
                    return true;
                }
                else if (r.ReservationDate.End.CompareTo(dateRange.Start) >= 0 && r.ReservationDate.End.CompareTo(dateRange.End) <= 0)
                {
                    return true;
                }
                else if (r.ReservationDate.Start.CompareTo(dateRange.Start) <= 0 && r.ReservationDate.End.CompareTo(dateRange.End) >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        public List<ReservationRequest> GetReservationRequestsOverlappingDateRange(DateRange dateRange)
        {
            List<ReservationRequest> reservationRequests = new List<ReservationRequest>();
            foreach (ReservationRequest r in ReservationRequests)
            {
                if (r.Status.Equals(ReservationRequestStatus.CANCELED))
                    continue;
                if (r.ReservationDate.Start.CompareTo(dateRange.Start) >= 0 && r.ReservationDate.Start.CompareTo(dateRange.End) <= 0)
                {
                    reservationRequests.Add(r);
                }
                else if (r.ReservationDate.End.CompareTo(dateRange.Start) >= 0 && r.ReservationDate.End.CompareTo(dateRange.End) <= 0)
                {
                    reservationRequests.Add(r);
                }
                else if (r.ReservationDate.Start.CompareTo(dateRange.Start) <= 0 && r.ReservationDate.End.CompareTo(dateRange.End) >= 0)
                {
                    reservationRequests.Add(r);
                }
            }
            return reservationRequests;
        }

        public bool IsDateRangeOfReservationValid(DateRange dateRange)
        {
            Price? p = GetPriceForSpecificDate(dateRange.Start);
            if(p == null)
            {
                return false;
            }
            else
            {
                if (dateRange.End.CompareTo(p.DateRange.End) >= 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public string GetAddressAsString()
        {
            return Address.Street + " " + Address.Number + ", " + Address.City + ", " + Address.Country;
        }

        public string GetBenefitsAsString()
        {
            string benefitString = "";
            foreach(Benefit b in Benefits)
            {
                benefitString += Enum.GetName(typeof(Benefit), b) + ", ";
            }
            return benefitString.Substring(0, benefitString.Length - 2); 
        }

        public int GetCancellationNumber(string guestEmail)
        {
            int cancellationNumber = 0;
            foreach (Reservation res in Reservations)
            {
                if(res.IsCanceled && res.GuestEmail.EmailAddress.Equals(guestEmail))
                {
                    cancellationNumber++;
                }
            }
            return cancellationNumber;
        }

        public bool IsValidNumberOfGuests(int numberOfGuest)
        {
            return numberOfGuest >= Capacity.Min && numberOfGuest <= Capacity.Max;
        }

        public void AddNewPrice(Price price)
        {
            List<Price> pricesToRemove = new List<Price>();
            List<Price> pricesToAdd = new List<Price>();
            DateTime priceStartFixedYear = new DateTime(2023, price.DateRange.Start.Month, price.DateRange.Start.Day);
            DateTime priceEndFixedYear = new DateTime(2023, price.DateRange.End.Month, price.DateRange.End.Day);
            foreach (Price p in PricePerGuest)
            {
                DateTime pStartFixedYear = new DateTime(2023, p.DateRange.Start.Month, p.DateRange.Start.Day);
                DateTime pEndFixedYear = new DateTime(2023, p.DateRange.End.Month, p.DateRange.End.Day);
                if (pStartFixedYear >= priceStartFixedYear && pEndFixedYear <= priceEndFixedYear)
                {
                    //PricePerGuest.Remove(p);
                    pricesToRemove.Add(p);
                    continue;
                }
                if(pStartFixedYear < priceStartFixedYear && pEndFixedYear >= priceStartFixedYear && pEndFixedYear <= priceEndFixedYear)
                {
                    p.DateRange = DateRange.Create(p.DateRange.Start, price.DateRange.End.AddDays(-1));
                    
                }
                else if (pStartFixedYear >= priceStartFixedYear && pStartFixedYear <= priceEndFixedYear && pEndFixedYear > priceEndFixedYear)
                {
                    p.DateRange = DateRange.Create(price.DateRange.End.AddDays(1), p.DateRange.End);
                }
                else if(pStartFixedYear < priceStartFixedYear && pEndFixedYear > priceEndFixedYear)
                {
                    Price newPrice = Price.Create(p.Value, DateRange.Create(price.DateRange.End.AddDays(1), p.DateRange.End));
                    p.DateRange = DateRange.Create(p.DateRange.Start, price.DateRange.Start.AddDays(-1));
                    pricesToAdd.Add(newPrice);
                    //PricePerGuest.Add(newPrice);
                }
            }

            foreach(Price p in pricesToRemove)
            {
                PricePerGuest.Remove(p);
            }
            foreach (Price p in pricesToAdd)
            {
                PricePerGuest.Add(p);
            }
            PricePerGuest.Add(price);

        }

        
    }
    public class AccommodationBuilder
    {
        public string Name { get; set; }
        public Address Address { get; set; }
        public PriceCalculation PriceCalculation { get; set; }
        public List<Price> PricePerGuest { get; set; } = new List<Price> { };
        public List<Benefit> Benefits { get; set; } = new List<Benefit> { };
        public List<Picture> Pictures { get; set; } = new List<Picture> { };
        public Capacity Capacity { get; set; }
        public List<Reservation> Reservations { get; set; } = new List<Reservation> { };
        public List<ReservationRequest> ReservationRequests { get; set; } = new List<ReservationRequest> { };
        public bool ReserveAutomatically { get; set; }
        public Email HostEmail{ get; set; }

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
        public AccommodationBuilder withPriceCalculation(PriceCalculation priceCalculation)
        {
            this.PriceCalculation = priceCalculation;
            return this;
        }
        public AccommodationBuilder withPricePerGuest(double value, DateRange dateRange)
        {
            this.PricePerGuest.Add(Price.Create(value, dateRange));
            return this;
        }
        public AccommodationBuilder withPricePerGuest(List<Price> prices)
        {
            this.PricePerGuest = prices;
            return this;
        }
        public AccommodationBuilder withReservations(List<Reservation> reservations)
        {
            this.Reservations = reservations;
            return this;
        }
        public AccommodationBuilder withReservationRequests(List<ReservationRequest> reservationRequests)
        {
            this.ReservationRequests = reservationRequests;
            return this;
        }
        public AccommodationBuilder withBenefits(List<Benefit> benefits)
        {
            this.Benefits = benefits;
            return this;
        }
        public AccommodationBuilder withPicture(string fileName, string base64)
        {
            Pictures.Add(Picture.Create(Guid.NewGuid(), fileName, base64));
            return this;
        }
        public AccommodationBuilder withPicture(List<Picture> pictures)
        {
            this.Pictures = pictures;
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
        public AccommodationBuilder withHostEmail(string hostEmail)
        {
            HostEmail = Email.Create(hostEmail);
            return this;
        }
        public Accommodation build()
        {
            return Accommodation.Create(Guid.NewGuid(), Name, Address, PriceCalculation, PricePerGuest, Benefits,
                Pictures, Capacity, Reservations, ReservationRequests, ReserveAutomatically, HostEmail);
        }
    }
    internal class AccommodationValidator : AbstractValidator<Accommodation>
    {
        public AccommodationValidator()
        {
            RuleFor(v => v.Name).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(v => v.Address).NotNull();
            RuleFor(v => v.PriceCalculation).NotNull();
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
