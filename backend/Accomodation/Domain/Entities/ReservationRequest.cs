﻿using Accomodation.Domain.Primitives.Enums;
using AccomodationSuggestionDomain.Exceptions.CustomExceptions;
using AccomodationSuggestionDomain.Primitives;
using AccomodationSuggestionDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationSuggestionDomain.Entities
{
    public class ReservationRequest : Entity
    {
        public Email GuestEmail { get; init; }
        public DateRange ReservationDate { get; init; }
        public ReservationRequestStatus Status { get; init; }

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
                guestNumber = value;
            }
        }


        private ReservationRequest(Guid id, Email guestEmail, DateRange reservationDate, int guestNumber, ReservationRequestStatus status) : base(id)
        {
            GuestEmail = guestEmail;
            ReservationDate = reservationDate;
            GuestNumber = guestNumber;
            Status = status;
        }

        public static ReservationRequest Create(Guid id, string email, DateTime start, DateTime end, int guestNumber, ReservationRequestStatus status)
        {
            return new ReservationRequest(id, Email.Create(email), DateRange.Create(start, end), guestNumber, status);
        }
    }
}
