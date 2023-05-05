﻿using Domain.Exceptions.CustomExceptions;
using Domain.Primitives;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Reservation : Entity
    {
        public Email GuestEmail { get; init; }
        public DateRange ReservationDate { get; init; }
        public int GuestNumber
        {
            get
            {
                return GuestNumber;
            }
            init
            {
                if (value <= 0)
                {
                    throw new NumberIsLessOrEqualToZeroException("Guest number must be greater than 0.");
                }
                GuestNumber = value;
            }
        }
        public Price PricePerGuest { get; init; }

        private Reservation(Guid id, Email questEmail, DateRange reservationDate, int guestNumber, Price price) : base(id)
        {
            GuestEmail = questEmail;
            ReservationDate = reservationDate;
            GuestNumber = guestNumber;
            PricePerGuest = price;
        }

        public static Reservation Create(Guid id, string email, DateTime start, DateTime end, int guestNumber, double price)
        {
            return new Reservation(id, Email.Create(email), DateRange.Create(start, end), guestNumber, Price.Create(price));
        }
    }
}