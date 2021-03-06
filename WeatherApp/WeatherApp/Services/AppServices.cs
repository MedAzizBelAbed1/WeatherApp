﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp.Constants;
using WeatherApp.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WeatherApp.Services
{
    public class AppServices : IAppServices
    {

        public delegate void AlertChangedListener(string message);
        public static event AlertChangedListener AlertChanged;

        public delegate void DataChangedListener();
        public static event DataChangedListener DataChanged;
        ConfigurationModel configuration;
        AddressModel address;
        public AppServices()
        {
            configuration = new ConfigurationModel();
            address = new AddressModel();


        }

        /// <summary>
        /// try to get user location (longitude & latitude)
        /// </summary>
        private async Task<string> GetCurrentUserLocation()
        {
            string locationLatLong = string.Empty;
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.High);
                var location = await Geolocation.GetLocationAsync(request);
                if (location != null)
                {
                    locationLatLong = $"{location.Latitude},{location.Longitude}";
                }
            }
            catch (FeatureNotSupportedException)
            {
                AlertChanged(ResourcesValues.LocationNotSupportedMessage);
            }
            catch (FeatureNotEnabledException)
            {
                AlertChanged(ResourcesValues.LocationNotEnabledMessage);

            }
            catch (PermissionException)
            {
                AlertChanged(ResourcesValues.LocationPermissionMessage);

            }
            catch (Exception)
            {
                AlertChanged(ResourcesValues.LocationUnkonwenMessage);
            }
            return locationLatLong;
        }


        /// <summary>
        /// try to get user location (longitude & latitude)
        /// </summary>
        private async Task<string> GetLocationFromAddress()
        {
            string locationLatLong = string.Empty;
            var fullAddress = (Application.Current.Properties["fullAddress"].ToString());
            try
            {
                var locations = await Geocoding.GetLocationsAsync(fullAddress);
                var location = locations?.FirstOrDefault();
                if (location != null)
                {
                    locationLatLong = $"{location.Latitude},{location.Longitude}";
                }
            }
            catch (FeatureNotSupportedException)
            {
                AlertChanged(ResourcesValues.LocationNotEnabledMessage);
            }
            catch (Exception)
            {
                AlertChanged(ResourcesValues.LocationUnkonwenMessage);
            }
            return locationLatLong;
        }

        /// <summary>
        /// save last user location used (current user location or from address) and use it to get weaather data
        /// </summary>
        public async Task SaveLastUserLocation(bool isCurrentPostion)
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
            if (!string.IsNullOrEmpty(lastUserLocation))
            {
                Application.Current.Properties["lastUserLocation"] = lastUserLocation;
                await Application.Current.SavePropertiesAsync();
                DataChanged();
            }
        }

        /// <summary>
        /// init  app configuration on first application start 
        /// </summary>
        public async Task InitConfiguration(ConfigurationModel configuration)
        {
            configuration.APIKey = AppConstants.DefaultAPIKey;
            configuration.numberOfDays = AppConstants.DefaultNumberOfDays;
            configuration.runAnimation = true;
            configuration.textcolor = AppConstants.DefaultColor;
            configuration.lastUserLocation = await this.GetCurrentUserLocation();
            await this.SaveConfiguration(configuration, false);
        }

        /// <summary>
        /// save last configuration used , and check it needded to refredh data
        /// </summary>
        public async Task SaveConfiguration(ConfigurationModel configuration, bool refreshData = true)
        {
            //For Saving Value
            Application.Current.Properties["APIKey"] = configuration.APIKey;
            Application.Current.Properties["numberOfDays"] = configuration.numberOfDays;
            Application.Current.Properties["runAnimation"] = configuration.runAnimation;
            Application.Current.Properties["synchronization"] = configuration.synchronization;
            Application.Current.Properties["duration"] = configuration.duration;
            Application.Current.Properties["lastUserLocation"] = configuration.lastUserLocation;
            Application.Current.Properties["textcolor"] = configuration.textcolor;
            await Application.Current.SavePropertiesAsync();
            if (refreshData) { DataChanged(); }
        }

        /// <summary>
        /// get last address input
        /// </summary>
        public ConfigurationModel GetConfiguration()
        {
            //For Saving Value
            try
            {
                configuration.APIKey = (Application.Current.Properties["APIKey"]?.ToString());
                configuration.numberOfDays = (Application.Current.Properties["numberOfDays"]?.ToString());
                configuration.runAnimation = Convert.ToBoolean((Application.Current.Properties["runAnimation"]?.ToString()));
                configuration.synchronization = Convert.ToBoolean((Application.Current.Properties["synchronization"]?.ToString()));
                configuration.duration = (Application.Current.Properties["duration"]?.ToString());
                configuration.lastUserLocation = (Application.Current.Properties["lastUserLocation"]?.ToString());
                configuration.textcolor = (Application.Current.Properties["textcolor"]?.ToString());
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
        public async Task SaveAddress(AddressModel addressModel)
        {
            //For Saving Value
            Application.Current.Properties["street"] = addressModel.street;
            Application.Current.Properties["zipCode"] = addressModel.zipCode;
            Application.Current.Properties["city"] = addressModel.city;
            Application.Current.Properties["country"] = addressModel.country;
            Application.Current.Properties["fullAddress"] = addressModel.fullAddress; ;
            await Application.Current.SavePropertiesAsync();
            await SaveLastUserLocation(false);
        }

        /// <summary>
        /// get last address input
        /// </summary>
        public AddressModel GetAddress()
        {
            //For Saving Value
            try
            {
                address.street = (Application.Current.Properties["street"]?.ToString());
                address.zipCode = (Application.Current.Properties["zipCode"]?.ToString());
                address.city = (Application.Current.Properties["city"]?.ToString());
                address.country = (Application.Current.Properties["country"]?.ToString());
                address.fullAddress = (Application.Current.Properties["fullAddress"]?.ToString());

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Can't get address {ex.Message}");
            }
            return address;
        }

    }
}
