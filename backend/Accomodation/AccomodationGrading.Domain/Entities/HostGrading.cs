using AccomodationGradingDomain.Primitives;
using AccomodationGradingDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationGradingDomain.Entities
{
    public class HostGrading : Entity
    {
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

        private HostGrading(Guid id, Email hostEmail, Email guestEmail, DateTime date, int grade) : base(id)
        {
            HostEmail = hostEmail;
            GuestEmail = guestEmail;
            Date = date;
            Grade = grade;
        }

        public static HostGrading Create(Guid id, string hostEmail, string guestEmail, DateTime date, int grade)
        {
            return new HostGrading(id, Email.Create(hostEmail), Email.Create(guestEmail), date, grade);
        }
    }
}
