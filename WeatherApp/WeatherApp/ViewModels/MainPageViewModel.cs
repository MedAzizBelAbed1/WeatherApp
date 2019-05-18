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

        public ObservableCollection<ForecastWeatherViewModel> forecastListVM => _forecastViewModel;
        private ObservableCollection<ForecastWeatherViewModel> _forecastViewModel { get; set; }

        public DailyWeatherViewModel dailyWeatherVM => _dailyViewModel;
        private DailyWeatherViewModel _dailyViewModel { get; set; }

        WeatherModel rootModel;
        IWeatherService weatherService;
        DetailedWeatherViewModel _detailedWeatheVM;
        bool IsConnected;

        public MainPageViewModel()
        {
            _dailyViewModel = new DailyWeatherViewModel();
            _forecastViewModel = new ObservableCollection<ForecastWeatherViewModel>();
            _detailedWeatheVM = new DetailedWeatherViewModel();
            weatherService = new WeatherService();
            AppServices.DataChanged += RefreshWeatherData;
        }

        async void RefreshWeatherData()
        {
            UserDialogs.Instance.ShowLoading();
            await BuildCompleteViewModel();
            UserDialogs.Instance.HideLoading();
        }

        /// <summary>
        /// get all weather data from the weather service ( current & forecast)
        /// map data with our view models 
        /// </summary>
        public async Task BuildCompleteViewModel()
        {
            //check internet connection
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
                    //pull complete weather data 
                    rootModel = await weatherService.GetRootModel(configuration.APIKey, int.Parse(configuration.numberOfDays) + 1, configuration.lastUserLocation);
                    var dailyAstro = rootModel.forecast.forecastday[0].astro;
                    // daily weather mapping
                    _dailyViewModel.textcolor = configuration.textcolor;
                    _dailyViewModel.region = rootModel.location.region;
                    _dailyViewModel.city = rootModel.location.name;
                    _dailyViewModel.country = rootModel.location.country;
                    _dailyViewModel.temperature = $"{rootModel.current.temp_c.ToString()}{AppConstants.celsius}";
                    _dailyViewModel.icon = $"{AppConstants.httpStart}{rootModel.current.condition.icon}";
                    _dailyViewModel.condition = rootModel.current.condition.text;
                    _dailyViewModel.feelLike = $"Feel Like {rootModel.current.feelslike_c.ToString()}{AppConstants.celsius}";
                    _dailyViewModel.humidity = $"{rootModel.current.humidity.ToString()} {AppConstants.persent}";
                    _dailyViewModel.minMaxTemp = $"{rootModel.current.temp_c.ToString()} {AppConstants.celsius}";
                    _dailyViewModel.wind = $"{rootModel.current.wind_kph.ToString()} {AppConstants.wind}";
                    _dailyViewModel.wind = $"{rootModel.current.wind_kph.ToString()} {AppConstants.wind}";
                    _dailyViewModel.backgroundImage = AppHelper.CheckDayState(rootModel.location.localtime, dailyAstro.sunrise, dailyAstro.sunset);
                    // weekly weather mapping 
                    _forecastViewModel.Clear();
                    foreach (var item in rootModel.forecast.forecastday.Skip(1))
                    {
                        _forecastViewModel.Add(new ForecastWeatherViewModel(configuration.textcolor,
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
        }


        public DetailedWeatherViewModel GetDetailedWeather(int poition)
        {
            try
            {
                var item = rootModel.forecast.forecastday[poition];
                _detailedWeatheVM.day = AppHelper.GetDayOfWeek(item.date);
                _detailedWeatheVM.icon = $"{AppConstants.httpStart}{item.day.condition.icon}";
                _detailedWeatheVM.maxTemp = $"{item.day.maxtemp_c.ToString()}{AppConstants.celsius}";
                _detailedWeatheVM.minTemp = $"{item.day.mintemp_c.ToString()}{AppConstants.celsius}";
                _detailedWeatheVM.maxWind = $"{item.day.maxwind_mph.ToString()}{AppConstants.wind}";
                _detailedWeatheVM.humidity = $"{item.day.avghumidity.ToString()}{AppConstants.persent}";
                _detailedWeatheVM.sunrise = $"{item.astro.sunrise}";
                _detailedWeatheVM.sunset = $"{item.astro.sunset}";
                _detailedWeatheVM.moonrise = $"{item.astro.moonrise}";
                _detailedWeatheVM.moonset = $"{item.astro.moonset}";
            }
            catch
            {
                AlertChanged(ResourcesValues.NoInformationMessage);
            }
            return _detailedWeatheVM;
        }
    }
}
