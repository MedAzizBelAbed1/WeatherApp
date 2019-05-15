using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

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
        private static async Task<string> GetCurrentUserLocation()
        {
            string locationLatLong = string.Empty;
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location != null)
                {
                    locationLatLong = $"{location.Longitude},{location.Latitude}";
                }
            }
            catch (Exception ex)
            {
                // Unable to get location
                Debug.WriteLine(ex.Message);
            }
            return locationLatLong;    
        }


        /// <summary>
        /// try to get user location (longitude & latitude)
        /// </summary>
        private static async Task<string> GetLocationFromAddress()
        {
            string locationLatLong = string.Empty;
            var fullAddress = (Application.Current.Properties["fullAddress "].ToString());
            try
            {
                var locations = await Geocoding.GetLocationsAsync(fullAddress);
                var location = locations?.FirstOrDefault();
                if (location != null)
                {
                    locationLatLong = $"{location.Latitude},{location.Longitude}";

                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Handle exception that may have occurred in geocoding
            }
            return locationLatLong;
        }


        public static async Task SaveLastUserLocation (bool isCurrentPostion)
        {
            string lastUserLocation = string.Empty;
            if (isCurrentPostion)
            {
                lastUserLocation = await GetCurrentUserLocation();
            }
            else
            {
                lastUserLocation = await GetLocationFromAddress();
            }
            Application.Current.Properties["lastUserLocation"] = lastUserLocation;
            await Application.Current.SavePropertiesAsync();

        }

        public static string GetLastUserLocation()
        {
            try
            {
                return (Application.Current.Properties["lastUserLocation"].ToString());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Can't get location {ex.Message}");
                return null;
            }
        }


        /// <summary>
        /// save last address input
        /// </summary>
        public static async Task SaveConfiguration(ConfigurationModel configuration)
        {
            //For Saving Value
            Application.Current.Properties["APIKey"] = configuration.APIKey;
            Application.Current.Properties["numberOfDays"] = configuration.numberOfDays;
            Application.Current.Properties["runAnimation"] = configuration.runAnimation;
            await Application.Current.SavePropertiesAsync();
        }   /// <summary>
            /// get last address input
            /// </summary>
        public static ConfigurationModel GetConfiguration()
        {
            ConfigurationModel configuration = new ConfigurationModel();
            //For Saving Value
            try
            {
                configuration.APIKey = (Application.Current.Properties["APIKey"].ToString());
                configuration.numberOfDays = (Application.Current.Properties["numberOfDays"].ToString());
                // configuration.runAnimation = Convert.ToBoolean((Application.Current.Properties["runAnimation"].ToString()));
                configuration.runAnimation = true;
             }
            catch (Exception ex)
            {
                Debug.WriteLine($"Can't get configuration {ex.Message}");
            }
            return configuration;
        }
        /// <summary>
        /// save last address input
        /// </summary>
        public static async Task SaveAddress(AddressModel addressModel)
        {
            //For Saving Value
            Application.Current.Properties["street"] = addressModel.street;
            Application.Current.Properties["zipCode"] = addressModel.zipCode;
            Application.Current.Properties["city"] = addressModel.city;
            Application.Current.Properties["country"] = addressModel.country;
            Application.Current.Properties["fullAddress"] = addressModel.fullAddress; ;
            await Application.Current.SavePropertiesAsync();
        }

        /// <summary>
        /// get last address input
        /// </summary>
        public static AddressModel GetAddress()
        {
            AddressModel address = new AddressModel();
            //For Saving Value
            try
            {
                address.street = (Application.Current.Properties["street"].ToString());
                address.zipCode = (Application.Current.Properties["zipCode"].ToString());
                address.city = (Application.Current.Properties["city"].ToString());
                address.country = (Application.Current.Properties["country"].ToString());
                address.fullAddress = (Application.Current.Properties["fullAddress"].ToString());

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Can't get address {ex.Message}");
            }
            return address;
        }

    }
}
