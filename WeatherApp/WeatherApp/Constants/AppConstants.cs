using System;
namespace WeatherApp.Constants
{
    public class AppConstants
    {
        //API URL
        public const string apiUrl = "https://api.apixu.com/v1/";

        //app constants
        //public const string DefaultAPIKey = "9202ffa8dc8a4bd68b1110943191405";
        public const string DefaultAPIKey = "7b493c0a6bf443ebb79151015191905";
        public const string DefaultNumberOfDays = "5";
        public const string DefaultColor = "White";
        public const int AnimationDuration = 2000;
        public const string celsius = "°C";
        public const string persent = "%";
        public const string wind = "m/s";
        public const string httpStart = "http:";

        //UI constants
        public const string dayImage = "backgroundDay.jpg";
        public const string nightImage = "backgroundNight.jpg";
    }
}
