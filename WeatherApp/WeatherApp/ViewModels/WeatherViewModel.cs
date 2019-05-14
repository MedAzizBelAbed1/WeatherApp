using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using WeatherApp.Constants;
using WeatherApp.Helper;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp.ViewModels
{
    public class WeatherViewModel
    {
        public List<WeekLyWeatherModel> forecastList => _forecastList;
        private List<WeekLyWeatherModel> _forecastList { get; set; }

        public DailyWeatherModel dailyWeather => _dailyWeather;
        private DailyWeatherModel _dailyWeather { get; set; }

        IWeatherService weatherService;

        public WeatherViewModel()
        {
            _dailyWeather = new DailyWeatherModel();
            _forecastList = new List<WeekLyWeatherModel>();
            weatherService = new WeatherService();
        }

        /// <summary>
        /// get all weather data from the weather service ( current & forecast)
        /// map data with our view models 
        /// </summary>
        public async Task LoadAllData()
        {
            WeatherModel rootModel = await weatherService.GetRootModel();

            // current weather mapping
            _dailyWeather.region = rootModel.location.region;
            _dailyWeather.city = rootModel.location.name;
            _dailyWeather.country = rootModel.location.country;

            _dailyWeather.temperature = $"{rootModel.current.temp_c.ToString()}{AppConstants.celsius}";
            _dailyWeather.icon = $"{AppConstants.httpStart}{rootModel.current.condition.icon}";
            _dailyWeather.condition = rootModel.current.condition.text;
            _dailyWeather.feelLike = rootModel.current.feelslike_c.ToString();
            _dailyWeather.humidity = rootModel.current.humidity.ToString();
            //FIXME
            _dailyWeather.minMaxTemp = rootModel.current.vis_km.ToString();
            _dailyWeather.wind = rootModel.current.wind_kph.ToString();
            // weekly weather mapping 
            foreach (var item in rootModel.forecast.forecastday)
            {
                _forecastList.Add(new WeekLyWeatherModel(
                    $"{item.day.mintemp_c.ToString()}{AppConstants.celsius}"
                    , $"{item.day.maxtemp_c.ToString()}{AppConstants.celsius}"
                    , AppHelper.GetDayOfWeek(item.date)
                    , $"{AppConstants.httpStart}{item.day.condition.icon}")
                    );
            }
        }
    }

    // daily weather view Model
    public class DailyWeatherModel
    {

        public string region { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string temperature { get; set; }
        public string icon { get; set; }
        public string condition { get; set; }
        public string feelLike { get; set; }
        public string humidity { get; set; }
        public string minMaxTemp { get; set; }
        public string wind { get; set; }

    }

    // weekly weather view Model
    public class WeekLyWeatherModel
    {
        public WeekLyWeatherModel(string minTemp, string maxTemp, string day, string icon)
        {
            this.minTemp = minTemp;
            this.maxTemp = maxTemp;
            this.day = day;
            this.icon = icon;
        }

        public string minTemp { get; set; }
        public string maxTemp { get; set; }
        public string day { get; set; }
        public string icon { get; set; }
    }

}
