using System;
using System.Globalization;

namespace CoreBoard.Extensions
{
    public static class DateTimeExtension
    {
        private static readonly TimeZoneInfo kstTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Korea Standard Time");

        public static DateTime ToKst(this DateTime source) => TimeZoneInfo.ConvertTime(source, kstTimeZone);

        public static int GetWeekOfMonth(this DateTime date)
        {
            DateTime beginningOfMonth = new DateTime(date.Year, date.Month, 1);

            while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                date = date.AddDays(1);

            return (int)Math.Truncate((double)date.Subtract(beginningOfMonth).TotalDays / 7f) + 1;
        }

        public static string ToShortTimespan(this DateTime input)
        {
            var now = DateTime.Now;
            var diff = now - input;

            var sec = (int)diff.TotalSeconds;
            var min = sec / 60;
            var hr = min / 60;
            var day = hr / 24;

            if (sec < 5)
                return "방금";
            if (sec < 60)
                return sec + "초 전";
            if (min < 60)
                return min + "분 전";
            if (hr < 24)
                return hr + "시간 전";
            if (day < 3)
                return day + "일 전";

            if (input.Year != now.Year)
            {
                return input.Year + "년 " + input.Month + "월";
            }

            return input.Month + "월 " + input.Day + "일";
        }

        public static DateTimeOffset ToKst(this DateTimeOffset source) => TimeZoneInfo.ConvertTime(source, kstTimeZone);

        public static int GetWeekOfMonth(this DateTimeOffset date)
        {
            DateTime beginningOfMonth = new DateTime(date.Year, date.Month, 1);

            while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                date = date.AddDays(1);

            return (int)Math.Truncate((double)date.Subtract(beginningOfMonth).TotalDays / 7f) + 1;
        }

        public static string ToShortTimespan(this DateTimeOffset input)
        {
            var now = DateTimeOffset.Now;
            var diff = now - input;

            var sec = (int)diff.TotalSeconds;
            var min = sec / 60;
            var hr = min / 60;
            var day = hr / 24;

            if (sec < 5)
                return "방금";
            if (sec < 60)
                return sec + "초 전";
            if (min < 60)
                return min + "분 전";
            if (hr < 24)
                return hr + "시간 전";
            if (day < 3)
                return day + "일 전";

            if (input.Year != now.Year)
            {
                return input.Year + "년 " + input.Month + "월";
            }

            return input.Month + "월 " + input.Day + "일";
        }

        public static int MonthDifference(this DateTime d1, DateTime d2)
            => (d1.Month - d2.Month) + 12 * (d1.Year - d2.Year);

        public static int MonthDifference(this DateTimeOffset d1, DateTimeOffset d2)
            => (d1.Month - d2.Month) + 12 * (d1.Year - d2.Year);
    }
}
