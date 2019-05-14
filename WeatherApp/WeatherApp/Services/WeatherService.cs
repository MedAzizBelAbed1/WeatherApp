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
        public async Task<WeatherModel> GetRootModel(string location)
        {
            return await weatherAPI.GetForecastWeather("9202ffa8dc8a4bd68b1110943191405", location, 5);
        }
    }
}
