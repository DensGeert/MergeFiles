using System;
using System.Collections.Generic;

namespace Protime.Bespoke.Tools.Helpers
{
    public static class DateTimeHelpers
    {
        /// <summary>
        /// Gets the 12:00:00 instance of a DateTime
        /// </summary>
        public static DateTime AbsoluteStart(this DateTime dateTime)
        {
            return dateTime.Date;
        }

        /// <summary>
        /// Gets the 11:59:59 instance of a DateTime
        /// </summary>
        public static DateTime AbsoluteEnd(this DateTime dateTime)
        {
            return AbsoluteStart(dateTime).AddDays(1).AddTicks(-1);
        }

        /// <summary>
        /// Gets days in range of 2 given DateTimes
        /// </summary>
        public static IEnumerable<DateTime> EachDay(this DateTime from, DateTime to)
        {
            for (var day = from.Date; day.Date <= to.Date; day = day.AddDays(1))
                yield return day;
        }

        public static bool InRange(this DateTime dateToCheck, DateTime startDate, DateTime endDate)
        {
            return dateToCheck >= startDate && dateToCheck <= endDate;
        }

        /// <summary>
        /// Splits Start en End in dateranges by days  (dayChunkSize: minimum 2 days)
        /// </summary>
        public static IEnumerable<Tuple<DateTime, DateTime>> SplitDateRange(DateTime start, DateTime end, int dayChunkSize)
        {
            if (dayChunkSize <= 1)
                dayChunkSize = (int)(end - start).TotalDays + 1;

            DateTime chunkEnd;
            while ((chunkEnd = start.AddDays(dayChunkSize - 1)) < end)
            {
                yield return Tuple.Create(start, chunkEnd);
                start = chunkEnd.AddDays(1);
            }
            yield return Tuple.Create(start, end);
        }
    }
}
