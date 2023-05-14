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
    public class Reservation : Entity
    {
        public Email GuestEmail { get; init; }
        public DateRange ReservationDate { get; init; }
        public int TotalPrice { get; init; }
        public bool IsCanceled { get; init; }

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

        private Reservation(Guid id, Email guestEmail, DateRange reservationDate, int guestNumber, bool isPerPerson, int price, bool isCanceled) : base(id)
        {
            GuestEmail = guestEmail;
            ReservationDate = reservationDate;
            IsCanceled = isCanceled;
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

        public static Reservation Create(Guid id, string email, DateTime start, DateTime end, int guestNumber, bool isPerPerson, int price, bool isCanceled)
        {
            return new Reservation(id, Email.Create(email), DateRange.Create(start, end), guestNumber, isPerPerson, price, isCanceled);
        }
    }
}
