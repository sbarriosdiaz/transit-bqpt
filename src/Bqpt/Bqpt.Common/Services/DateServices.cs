////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System;

namespace Bqpt.Common
{
    public static class DateServices
    {
        public static DateTime DtNow => DateTime.Now;

        public static int GetNumberOfWorkingDays(DateTime startDate, DateTime endDate)
        {
            var days = 0;

            if (startDate >= endDate) return days;

            while (startDate <= endDate)
            {
                if (startDate.DayOfWeek != DayOfWeek.Saturday && startDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    ++days;
                }

                startDate = startDate.AddDays(1);
            }

            return days;
        }

        /// <summary>
        /// Determines if this date is a federal holiday.
        /// </summary>
        /// <param name="date">This date</param>
        /// <returns>True if this date is a federal holiday</returns>
        public static bool IsFederalHoliday(DateTime date)
        {
            var nextWeekDay = (int)Math.Ceiling(date.Day / 7.0d);
            var dayName = date.DayOfWeek;
            var isThursday = dayName == DayOfWeek.Thursday;
            var isFriday = dayName == DayOfWeek.Friday;
            var isMonday = dayName == DayOfWeek.Monday;
            var isWeekend = dayName == DayOfWeek.Saturday || dayName == DayOfWeek.Sunday;

            if (IsNewYearDay(date, isFriday, isWeekend, isMonday)) return true;

            if (IsMlkDay(date, isMonday, nextWeekDay)) return true;

            if (IsPresidentsDay(date, isMonday, nextWeekDay)) return true;

            if (IsMemorialDay(date, isMonday)) return true;

            if (IsIndependencesDay(date, isMonday, isFriday, isWeekend)) return true;

            if (IsLaborDay(date, isMonday, nextWeekDay)) return true;

            if (IsColumbusDay(date, isMonday, nextWeekDay)) return true;

            if (IsVeternasDay(date, isMonday, isFriday, isWeekend)) return true;

            if (IsThanksGivingDay(date, isThursday, isFriday, nextWeekDay)) return true;

            if (IsChristmasDay(date, isFriday, isWeekend, isMonday)) return true;

            return false;
        }

        #region Helpers

        private static bool IsNewYearDay(DateTime date, bool isFriday, bool isWeekend, bool isMonday) => (date.Month == 12 && date.Day == 31 && isFriday) ||
                (date.Month == 1 && date.Day == 1 && !isWeekend) ||
                (date.Month == 1 && date.Day == 2 && isMonday);

        private static bool IsMlkDay(DateTime date, bool isMonday, int nthWeekDay) => (date.Month == 1 && isMonday && nthWeekDay == 3);

        private static bool IsPresidentsDay(DateTime date, bool isMonday, int nthWeekDay) => (date.Month == 2 && isMonday && nthWeekDay == 3);

        private static bool IsMemorialDay(DateTime date, bool isMonday) => (date.Month == 5 && isMonday && date.AddDays(7).Month == 6);

        private static bool IsIndependencesDay(DateTime date, bool isMonday, bool isFriday, bool isWeekend) => (date.Month == 7 && date.Day == 3 && isFriday) ||
                (date.Month == 7 && date.Day == 4 && !isWeekend) ||
                (date.Month == 7 && date.Day == 5 && isMonday);

        private static bool IsLaborDay(DateTime date, bool isMonday, int nthWeekDay) => (date.Month == 9 && isMonday && nthWeekDay == 1);

        private static bool IsColumbusDay(DateTime date, bool isMonday, int nthWeekDay) => (date.Month == 10 && isMonday && nthWeekDay == 2);

        private static bool IsVeternasDay(DateTime date, bool isMonday, bool isFriday, bool isWeekend) => ((date.Month == 11 && date.Day == 10 && isFriday) ||
                (date.Month == 11 && date.Day == 11 && !isWeekend) ||
                (date.Month == 11 && date.Day == 12 && isMonday));

        private static bool IsThanksGivingDay(DateTime date, bool isThursday, bool isFriday, int nthWeekDay) => (date.Month == 11 && isThursday && nthWeekDay == 4 || date.Month == 11 && isFriday && nthWeekDay == 4);

        private static bool IsChristmasDay(DateTime date, bool isFriday, bool isWeekend, bool isMonday) => (date.Month == 12 && date.Day == 24 && isFriday) ||
                (date.Month == 12 && date.Day == 25 && !isWeekend) ||
                (date.Month == 12 && date.Day == 26 && isMonday);

        #endregion Helpers
    }
}