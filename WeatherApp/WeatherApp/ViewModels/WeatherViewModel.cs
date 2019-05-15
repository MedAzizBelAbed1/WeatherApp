using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        WeatherModel rootModel;
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
        public async Task LoadAllData(ConfigurationModel configuration)
        {
            var location = AppServices.GetLastUserLocation();
            if (string.IsNullOrEmpty(location))
            {
                location=  await AppServices.SaveLastUserLocation(true);
            }
            if (!string.IsNullOrEmpty(location))
            {
                rootModel = await weatherService.GetRootModel(configuration.APIKey, int.Parse(configuration.numberOfDays)+1,location);
                var dailyAstro = rootModel.forecast.forecastday[0].astro;
                // current weather mapping
                _dailyWeather.region = rootModel.location.region;
                _dailyWeather.city = rootModel.location.name;
                _dailyWeather.country = rootModel.location.country;
                _dailyWeather.temperature = $"{rootModel.current.temp_c.ToString()}{AppConstants.celsius}";
                _dailyWeather.icon = $"{AppConstants.httpStart}{rootModel.current.condition.icon}";
                _dailyWeather.condition = rootModel.current.condition.text;
                _dailyWeather.feelLike = $"Feel Like {rootModel.current.feelslike_c.ToString()}{AppConstants.celsius}";
                _dailyWeather.humidity = $"{rootModel.current.humidity.ToString()} {AppConstants.persent}";
                //FIXME
                _dailyWeather.minMaxTemp = $"{rootModel.current.vis_km.ToString()} {AppConstants.celsius}";
                _dailyWeather.wind = $"{rootModel.current.wind_kph.ToString()} {AppConstants.wind}";
                _dailyWeather.wind = $"{rootModel.current.wind_kph.ToString()} {AppConstants.wind}";
                _dailyWeather.backgroundImage = AppHelper.CheckIfNight(dailyAstro.sunrise, dailyAstro.sunset);
                // weekly weather mapping 
                foreach (var item in rootModel.forecast.forecastday.Skip(1))
                {
                    _forecastList.Add(new WeekLyWeatherModel(
                        $"{item.day.mintemp_c.ToString()}{AppConstants.celsius}"
                        , $"{item.day.maxtemp_c.ToString()}{AppConstants.celsius}"
                        , AppHelper.GetDayOfWeek(item.date)
                        , $"{AppConstants.httpStart}{item.day.condition.icon}")
                        );
                }
            }
            else
            {

            }
        }
        public DetailedWeatherModel GetDetailedWeather(int poition)
        {
            DetailedWeatherModel detailedWeather = new DetailedWeatherModel();
            var item = rootModel.forecast.forecastday[poition];
            detailedWeather.day = AppHelper.GetDayOfWeek(item.date);
            detailedWeather.icon = $"{AppConstants.httpStart}{item.day.condition.icon}";
            detailedWeather.maxTemp= $"{item.day.maxtemp_c.ToString()}{AppConstants.celsius}";
            detailedWeather.minTemp = $"{item.day.mintemp_c.ToString()}{AppConstants.celsius}";
            detailedWeather.maxWind = $"{item.day.maxwind_mph.ToString()}{AppConstants.wind}";
            detailedWeather.humidity = $"{item.day.avghumidity.ToString()}{AppConstants.persent}";
            detailedWeather.sunrise = $"{item.astro.sunrise}";
            detailedWeather.sunset = $"{item.astro.sunset}";
            detailedWeather.moonrise = $"{item.astro.moonrise}";
            detailedWeather.moonset = $"{item.astro.moonset}";
            return detailedWeather;
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
        public string backgroundImage { get; set; }

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

    public class DetailedWeatherModel
    {
        public string day { get; set; }
        public string icon { get; set; }
        public string maxTemp { get; set; }
        public string minTemp { get; set; }
        public string maxWind { get; set; }
        public string humidity { get; set; }
        public string sunrise { get; set; }
        public string sunset { get; set; }
        public string moonrise { get; set; }
        public string moonset { get; set; }
    }


}
