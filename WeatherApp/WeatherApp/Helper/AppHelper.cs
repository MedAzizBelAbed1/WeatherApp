using System;
using System.Globalization;
using WeatherApp.Constants;

namespace WeatherApp.Helper
{
    public static class AppHelper
    {
        public static string GetDayOfWeek(string date)
        {
            DateTime dateTime;
            DateTime.TryParseExact(date,
                                   "yyyy-MM-dd",
                                   CultureInfo.InvariantCulture,
                                   DateTimeStyles.None,
                                   out dateTime);
            var year = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
            return dateTime.DayOfWeek.ToString();
        }

        public static string CheckIfNight(string timeSunRise, string timeSunSet)
        {
            string result = string.Empty;
            TimeSpan start;
            TimeSpan end;
            TimeSpan.TryParse(timeSunSet, out start);
            TimeSpan.TryParse(timeSunRise, out end);
            TimeSpan now = DateTime.Now.TimeOfDay;
            if ((now > start) && (now < end))
            {
                result = AppConstants.dayImage;
            }
            else
            {
                result = AppConstants.nightImage;
            }
            return result;
        }
    }
}
