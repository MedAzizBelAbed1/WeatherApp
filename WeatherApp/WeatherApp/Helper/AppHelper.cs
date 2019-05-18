using System;
using System.Globalization;
using Plugin.Connectivity;
using WeatherApp.Constants;

namespace WeatherApp.Helper
{
    public static class AppHelper
    {

        public static bool CheckInternet()
        {
            return CrossConnectivity.Current.IsConnected;
        }
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

        public static string CheckDayState(string currentLocationTime,string timeSunRise, string timeSunSet)
        {
            string result = string.Empty;
            bool isBetween;
            TimeSpan sunrise;
            TimeSpan sunset;
            TimeSpan now;
            now = DateTime.ParseExact(currentLocationTime,
                                   "yyyy-MM-dd H:mm", CultureInfo.InvariantCulture).TimeOfDay;
            sunrise = DateTime.ParseExact(timeSunRise,
                                 "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
            sunset = DateTime.ParseExact(timeSunSet,
                                    "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
            if (sunset < sunrise)
                isBetween = sunset <= now && now <= sunrise;
            isBetween = !(sunrise < now && now < sunset);
            result = isBetween ? result = AppConstants.nightImage : result = AppConstants.dayImage;
            return result;
        }
    }
}
