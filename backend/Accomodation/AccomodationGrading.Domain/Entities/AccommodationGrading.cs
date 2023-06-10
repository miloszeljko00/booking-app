using AccomodationGradingDomain.Primitives;
using AccomodationGradingDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationGradingDomain.Entities
{
    public class AccommodationGrading : Entity
    {
        public string AccommodationName { get; init; }
        public Email GuestEmail { get; init; }
        public Email HostEmail { get; init; }
        public DateTime Date { get; init; }

        private int grade;

        public int Grade
        {
            get
            {
                return grade;
            }
            init
            {
                if (value <= 0 || value >= 6)
                {
                    throw new Exception("Grade must be greater than 0 and less than 6.");
                }
                grade = value;
            }
        }

        private AccommodationGrading(Guid id, string name, Email hostEmail, Email guestEmail, DateTime date, int grade) : base(id)
        {
            AccommodationName = name;
            HostEmail = hostEmail;
            GuestEmail = guestEmail;
            Date = date;
            Grade = grade;
        }

        public static AccommodationGrading Create(Guid id, string name, string hostEmail, string guestEmail, DateTime date, int grade)
        {
            return new AccommodationGrading(id, name, Email.Create(hostEmail), Email.Create(guestEmail), date, grade);
        }
    }
}
