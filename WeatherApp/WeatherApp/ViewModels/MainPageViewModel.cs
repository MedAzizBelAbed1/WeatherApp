using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using WeatherApp.Constants;
using WeatherApp.Helper;
using WeatherApp.Models;
using WeatherApp.Services;
using static WeatherApp.ViewModels.WeatherViewModel;

namespace WeatherApp.ViewModels
{
    public class MainPageViewModel
    {

        public delegate void AlertChangedListener(string message);
        public static event AlertChangedListener AlertChanged;

        public ObservableCollection<ForecastWeatherViewModel> forecastListVM => _forecastList;
        private ObservableCollection<ForecastWeatherViewModel> _forecastList { get; set; }

        public DailyWeatherViewModel dailyWeatherVM => _dailyWeather;
        private DailyWeatherViewModel _dailyWeather { get; set; }

        WeatherModel rootModel;
        IWeatherService weatherService;
        bool IsConnected;

        public MainPageViewModel()
        {
            _dailyWeather = new DailyWeatherViewModel();
            _forecastList = new ObservableCollection<ForecastWeatherViewModel>();
            weatherService = new WeatherService();
            AppServices.DataChanged += RefreshWeatherData;
        }

        async void RefreshWeatherData()
        {
            await PullCompleteData();
        }

        /// <summary>
        /// get all weather data from the weather service ( current & forecast)
        /// map data with our view models 
        /// </summary>
        public async Task PullCompleteData()
        {
            UserDialogs.Instance.ShowLoading();
            IsConnected = AppHelper.CheckInternet();
            if (IsConnected)
            {
                var configuration = AppServices.GetConfiguration();
                if (string.IsNullOrEmpty(configuration.lastUserLocation))
                {
                    AlertChanged(ResourcesValues.LocationUnkonwenMessage);
                    UserDialogs.Instance.HideLoading();
                    return;
                }
                    try
                    {
                        rootModel = await weatherService.GetRootModel(configuration.APIKey, int.Parse(configuration.numberOfDays) + 1, configuration.lastUserLocation);
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
                        _dailyWeather.minMaxTemp = $"{rootModel.current.temp_c.ToString()} {AppConstants.celsius}";
                        _dailyWeather.wind = $"{rootModel.current.wind_kph.ToString()} {AppConstants.wind}";
                        _dailyWeather.wind = $"{rootModel.current.wind_kph.ToString()} {AppConstants.wind}";
                        _dailyWeather.backgroundImage = AppHelper.CheckIfNight(dailyAstro.sunrise, dailyAstro.sunset);
                        // weekly weather mapping 
                        _forecastList.Clear();
                        foreach (var item in rootModel.forecast.forecastday.Skip(1))
                        {
                            _forecastList.Add(new ForecastWeatherViewModel(
                                $"{item.day.mintemp_c.ToString()}{AppConstants.celsius}"
                                , $"{item.day.maxtemp_c.ToString()}{AppConstants.celsius}"
                                , AppHelper.GetDayOfWeek(item.date)
                                , $"{AppConstants.httpStart}{item.day.condition.icon}")
                                );
                        }
                    }
                    catch
                    {
                        AlertChanged(ResourcesValues.tryAgainLaterMessage);
                    }
            }
            else
            {
                AlertChanged(ResourcesValues.CheckInternetMessage);
            }
            UserDialogs.Instance.HideLoading();
        }


        public DetailedWeatherViewModel GetDetailedWeather(int poition)
        {
            DetailedWeatherViewModel detailedWeather = new DetailedWeatherViewModel();
            try
            {
                var item = rootModel.forecast.forecastday[poition];
                detailedWeather.day = AppHelper.GetDayOfWeek(item.date);
                detailedWeather.icon = $"{AppConstants.httpStart}{item.day.condition.icon}";
                detailedWeather.maxTemp = $"{item.day.maxtemp_c.ToString()}{AppConstants.celsius}";
                detailedWeather.minTemp = $"{item.day.mintemp_c.ToString()}{AppConstants.celsius}";
                detailedWeather.maxWind = $"{item.day.maxwind_mph.ToString()}{AppConstants.wind}";
                detailedWeather.humidity = $"{item.day.avghumidity.ToString()}{AppConstants.persent}";
                detailedWeather.sunrise = $"{item.astro.sunrise}";
                detailedWeather.sunset = $"{item.astro.sunset}";
                detailedWeather.moonrise = $"{item.astro.moonrise}";
                detailedWeather.moonset = $"{item.astro.moonset}";
            }
            catch
            {
                AlertChanged(ResourcesValues.NoInformationMessage);
            }
            return detailedWeather;
        }
    }
}
