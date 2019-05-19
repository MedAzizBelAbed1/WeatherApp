using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherApp.API;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class WeatherService : IWeatherService
    {
        WeatherAPI weatherAPI;

        public WeatherService()
        {
            weatherAPI = new WeatherAPI();
        }
        /// <summary>
        /// get weather root data from the API
        /// </summary>
        /// <typeparam name="ApiKey">Api key to be used</typeparam>
        /// <typeparam name="numberOfDays">number of forecast days</typeparam>
        /// <typeparam name="location">lstuserlocation</typeparam>
        public async Task<WeatherModel> GetRootModel(string ApiKey,int numberOfDays,string location)
        {
            return await weatherAPI.GetForecastWeather(ApiKey, location, numberOfDays);
        }
    }
}
