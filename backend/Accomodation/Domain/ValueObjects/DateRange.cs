using AccomodationSuggestionDomain.Exceptions.CustomExceptions;
using AccomodationSuggestionDomain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationSuggestionDomain.ValueObjects
{
    public class DateRange : ValueObject
    {
        public DateTime Start { get; init; }
        public DateTime End { get; init; }

        public DateRange() { }
        private DateRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }
        public static DateRange Create(DateTime start, DateTime end)
        {
            if (CheckIfDatesAreValid(start, end))
            {
                return new DateRange(start, end);
            }
            else
            {
                throw new InvalidDateRangeException();
            }
        }

        private static bool CheckIfDatesAreValid(DateTime start, DateTime end)
        {
            return start < end;
        }
        public int CalculateNumberOfDaysInRange()
        {
            TimeSpan timeSpan = End - Start;
            return timeSpan.Days;
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Start;
            yield return End;
        }
    }
}
