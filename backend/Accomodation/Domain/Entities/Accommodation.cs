﻿using Accomodation.Domain.Primitives.Enums;
using AccomodationDomain.Exceptions.CustomExceptions;
using AccomodationDomain.Primitives;
using AccomodationDomain.Primitives.Enums;
using AccomodationDomain.ValueObjects;
using FluentValidation;
using System;

namespace AccomodationDomain.Entities
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
        
        private Accommodation(Guid id, string name, Address address, PriceCalculation priceCalculation,
            List<Price> prices, List<Benefit> benefits,List<Picture> pictures, Capacity capacity,
            List<Reservation> reservations, List<ReservationRequest> reservationRequests, bool reserveAutomatically) : base(id)
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
        }
        public static Accommodation Create(Guid id, string name, Address address, PriceCalculation priceCalculation,
            List<Price> prices, List<Benefit> benefits, List<Picture> pictures,
            Capacity capacity, List<Reservation> reservations,
            List<ReservationRequest> reservationRequests, bool reserveAutomatically)
        {
            var accommodation = new Accommodation(id, name, address, priceCalculation, prices,
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
        public void CreateReservation(string email, DateTime start, DateTime end, int numberOfGuests, bool isPerPerson, int price)
        {
            Reservations.Add(Reservation.Create(Guid.NewGuid(), email, start, end, numberOfGuests, isPerPerson, price));
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

        public bool IsDateRangeOfReservationValid(DateRange dateRange)
        {
            Price? p = GetPriceForSpecificDate(dateRange.Start);
            if(p == null)
            {
                return false;
            }
            else
            {
                if (dateRange.End.CompareTo(p.DateRange.End) > 0)
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
        public AccommodationBuilder withPicture(string fileName, string description)
        {
            Pictures.Add(Picture.Create(Guid.NewGuid(), fileName, description));
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
        public Accommodation build()
        {
            return Accommodation.Create(Guid.NewGuid(), Name, Address, PriceCalculation, PricePerGuest, Benefits,
                Pictures, Capacity, Reservations, ReservationRequests, ReserveAutomatically);
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
