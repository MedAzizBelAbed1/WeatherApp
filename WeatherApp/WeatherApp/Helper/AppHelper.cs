using System;
using System.Globalization;

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
    }
}
