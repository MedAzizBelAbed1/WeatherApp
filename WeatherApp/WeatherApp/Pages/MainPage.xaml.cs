using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using WeatherApp.Constants;
using WeatherApp.Models;
using WeatherApp.Pages;
using WeatherApp.Services;
using WeatherApp.ViewModels;
using Xamarin.Forms;
using static WeatherApp.ViewModels.WeatherViewModel;

namespace WeatherApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        IAppServices appServices;
        MainPageViewModel vm;
        ConfigurationModel configuration;
        Timer animationTimer;
        Timer refreshTimer;
        bool canRefresh;
        bool isDataLoaded;
        public MainPage()
        {
            InitializeComponent();
            appServices = (Application.Current as App).AppServices;
            vm = new MainPageViewModel();
            MainPageViewModel.AlertChanged += Vm_AlertChangedListener;
            AppServices.AlertChanged += Vm_AlertChangedListener;
        }

        // on start , check app configuration & do the binding with ViewModel , show main pageView
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            UserDialogs.Instance.ShowLoading();
            await CheckAppConfiguration();
            if (!isDataLoaded)
            {
                await vm.BuildCompleteViewModel();
                isDataLoaded = true;
            }
            DailyWeatherView.BindingContext = vm.dailyWeatherVM;
            forecastlistView.ItemsSource = vm.forecastListVM;
            MainView.IsVisible = true;
            canRefresh = false;
            UserDialogs.Instance.HideLoading();
        }

        //Alert Popup Listener
        void Vm_AlertChangedListener(string message)
        {
            Device.BeginInvokeOnMainThread(async() =>
            {
                 await DisplayAlert(ResourcesValues.AppName, message, ResourcesValues.OkMessage);
            });
        }

        //Refresh forecast list view by user
        async void ForecastLisView_Refreshing(object sender, System.EventArgs e)
        {
            forecastlistView.IsRefreshing = true;
            await vm.BuildCompleteViewModel();
            forecastlistView.IsRefreshing = false;
        }

        // get weather data of user current position
        async void CurrentLocationClicked(object sender, System.EventArgs e)
        {
            await appServices.SaveLastUserLocation(true);
        }

        //addredd page clicked
        void AddressLocationClicked(object sender, System.EventArgs e)
        {
            Navigation.PushModalAsync(new AddressPage(), false);
        }

        // configuration page clicked
        void ConfigurationClicked(object sender, System.EventArgs e)
        {
            Navigation.PushModalAsync(new ConfigurationPage(), false);
        }

        /// <summary>
        /// item from forecast list view clicked
        /// Increment with 1 because the first item is the current day weather
        /// </summary>
        void ForecastList_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var index = (forecastlistView.ItemsSource as ObservableCollection<ForecastWeatherViewModel>).IndexOf(e.Item as ForecastWeatherViewModel);
            Navigation.PushModalAsync(new WeatherDetailPage(vm.GetDetailedWeather(index + 1)), false);
        }
        // detailed weather page clicked page clicked
        void DailyDetailedWeather_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushModalAsync(new WeatherDetailPage(vm.GetDetailedWeather(0)), false);
        }

        //dispose animation & refresh timmer
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

        async Task CheckAppConfiguration()
        {
            //try to get configuration
            configuration = appServices.GetConfiguration();
            // init configuration if it's empty ( use default api key & user current location etc ..) 
            if (string.IsNullOrEmpty(configuration.APIKey))
            {
                await appServices.InitConfiguration(configuration);
            }
            //run synchronization if exist
            if (configuration.synchronization)
            {
                refreshTimer = new Timer(async (e) =>
                {
                    if(canRefresh)
                        await vm.BuildCompleteViewModel();
                    canRefresh = true;
                }, null, 0, (int.Parse(configuration.duration)) * 60 * 1000);
            }
            // run animation id exist
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
