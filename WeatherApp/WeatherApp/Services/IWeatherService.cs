using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public interface IWeatherService
    {
        Task<WeatherModel> GetRootModel(string ApiKey, int numberOfDays, string location);
    }
}
