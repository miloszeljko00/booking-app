﻿using AccomodationDomain.Exceptions.CustomExceptions;
using AccomodationDomain.Primitives;
using AccomodationDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationDomain.Entities
{
    public class Reservation : Entity
    {
        public Email GuestEmail { get; init; }
        public DateRange ReservationDate { get; init; }
        public int TotalPrice { get; init; }
        private int guestNumber;
        public int GuestNumber
        {
            get
            {
                return guestNumber;
            }
            init
            {
                if (value <= 0)
                {
                    throw new NumberIsLessOrEqualToZeroException("Guest number must be greater than 0.");
                }
                GuestNumber = guestNumber;
            }
        }

        private Reservation(Guid id, Email questEmail, DateRange reservationDate, int guestNumber, bool isPerPerson, int price) : base(id)
        {
            GuestEmail = questEmail;
            ReservationDate = reservationDate;
            GuestNumber = guestNumber;
            if (isPerPerson)
            {
                TotalPrice = guestNumber * price;
            }
            else
            {
                TotalPrice = price;
            }
        }

        public static Reservation Create(Guid id, string email, DateTime start, DateTime end, int guestNumber, bool isPerPerson, int price)
        {
            return new Reservation(id, Email.Create(email), DateRange.Create(start, end), guestNumber, isPerPerson, price);
        }
    }
}
