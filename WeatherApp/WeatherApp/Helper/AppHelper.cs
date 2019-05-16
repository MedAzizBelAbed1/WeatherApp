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

        public static string CheckIfNight(string timeSunRise, string timeSunSet)
        {
            string result = string.Empty;
            TimeSpan start;
            TimeSpan end;
            end = DateTime.ParseExact(timeSunSet,
                                 "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
            start = DateTime.ParseExact(timeSunRise,
                                    "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
           
            TimeSpan.TryParse(timeSunRise, out end);
            TimeSpan now = DateTime.Now.TimeOfDay;
            if ((now > start) && (now < end))
            {
                result = AppConstants.nightImage;
            }
            else
            {
                result = AppConstants.dayImage;
            }
            return result;
        }
    }
}
