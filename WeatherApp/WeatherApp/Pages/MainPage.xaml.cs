using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using WeatherApp.API;
using WeatherApp.Constants;
using WeatherApp.Models;
using WeatherApp.Pages;
using WeatherApp.Services;
using WeatherApp.ViewModels;
using Xamarin.Forms;

namespace WeatherApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        WeatherViewModel vm;
        ConfigurationModel configuration;
        Timer animationTimer;
        Timer refreshTimer;
        public MainPage()
        {
            InitializeComponent();
        }

        async void CurrentLocationClicked(object sender, System.EventArgs e)
        {
            await AppServices.SaveLastUserLocation(true);
        }

        void AddressLocationClicked(object sender, System.EventArgs e)
        {
            Navigation.PushModalAsync(new AddressPage());
        }
        void ConfigurationClicked(object sender, System.EventArgs e)
        {
            Navigation.PushModalAsync(new ConfigurationPage());
        }
        protected override void OnDisappearing()
        {
            if (configuration.runAnimation)
            {
                animationTimer.Dispose();
            }
            if (configuration.synchronization)
            {
                refreshTimer.Dispose();
            }
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            UserDialogs.Instance.ShowLoading();
            configuration = AppServices.GetConfiguration();
            await CheckAppConfiguration();
            vm = new WeatherViewModel();
            await vm.LoadAllData(configuration);
            DailyWeatherView.BindingContext = vm.dailyWeather;
            forecastlistView.ItemsSource = vm.forecastList;
            UserDialogs.Instance.HideLoading();
        }

        void ForecastList_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var index = (forecastlistView.ItemsSource as List<WeekLyWeatherModel>).IndexOf(e.Item as WeekLyWeatherModel);
            // Increment with 1 because the first item is the current day weather
            Navigation.PushModalAsync(new WeatherDetailPage(vm.GetDetailedWeather(index + 1)));
        }

        void DailyDetailedWeather_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushModalAsync(new WeatherDetailPage(vm.GetDetailedWeather(0)));
        }

        async Task CheckAppConfiguration ()
        {
            configuration = AppServices.GetConfiguration();
            if (string.IsNullOrEmpty(configuration.APIKey))
            {
                configuration.APIKey = AppConstants.APIKey;
                configuration.numberOfDays = "5";
                configuration.runAnimation = true;
                await AppServices.SaveConfiguration(configuration);
            }
            if(configuration.synchronization)
            {
                refreshTimer = new Timer(async (e) =>
                {
                    await vm.LoadAllData(configuration);
                }, null, 0, int.Parse(configuration.duration)*60);
            }
            if (configuration.runAnimation)
            {
                animationTimer = new Timer((e) =>
                {
                    UIServices.ScaleImage(weatherImage);
                }, null, 0, AppConstants.AnimationDuration);
            }
        }
       
    }
}
