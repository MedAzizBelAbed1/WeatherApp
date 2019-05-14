using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace WeatherApp.Services
{
    public class AppServices
    {
        public AppServices()
        {
        }


        /// <summary>
        /// try to get user location (longitude & latitude)
        /// </summary>
        public static async Task<Location> GetUserLocation()
        {
            Location location = null;
            try
            {
                location = await Geolocation.GetLastKnownLocationAsync();
                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
            }
            catch (Exception ex)
            {
                // Unable to get location
                Debug.WriteLine(ex.Message);
            }
            return location;    
        }
    }
}
