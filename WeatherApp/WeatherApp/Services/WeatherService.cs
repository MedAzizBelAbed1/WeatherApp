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
        public async Task<WeatherModel> GetRootModel(string ApiKey,int numberOfDays,string location)
        {
            return await weatherAPI.GetForecastWeather(ApiKey, location, numberOfDays);
        }
    }
}
