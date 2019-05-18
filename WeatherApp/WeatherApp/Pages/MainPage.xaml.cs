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
        MainPageViewModel vm;
        ConfigurationModel configuration;
        Timer animationTimer;
        Timer refreshTimer;
        bool isDataLoaded;
        public MainPage()
        {
            InitializeComponent();
            vm = new MainPageViewModel();
            MainPageViewModel.AlertChanged += Vm_AlertChangedListener;
            AppServices.AlertChanged += Vm_AlertChangedListener;
        }

        //Alert Popup Listener
        void Vm_AlertChangedListener(string message)
        {
            DisplayAlert(ResourcesValues.AppName, message, ResourcesValues.OkMessage);
        }

        //Refresh forecast list view by user
        async void ForecastLisView_Refreshing(object sender, System.EventArgs e)
        {
            forecastlistView.IsRefreshing = true;
            await vm.BuildCompleteViewModel();
            forecastlistView.IsRefreshing = false;
        }

        async void CurrentLocationClicked(object sender, System.EventArgs e)
        {
            await AppServices.SaveLastUserLocation(true);
        }
        void AddressLocationClicked(object sender, System.EventArgs e)
        {
            Navigation.PushModalAsync(new AddressPage(),false);
        }
        void ConfigurationClicked(object sender, System.EventArgs e)
        {
            Navigation.PushModalAsync(new ConfigurationPage(),false);
        }
        void ForecastList_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            // Increment with 1 because the first item is the current day weather
            var index = (forecastlistView.ItemsSource as ObservableCollection<ForecastWeatherViewModel>).IndexOf(e.Item as ForecastWeatherViewModel);
            Navigation.PushModalAsync(new WeatherDetailPage(vm.GetDetailedWeather(index + 1)),false);
        }

        void DailyDetailedWeather_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushModalAsync(new WeatherDetailPage(vm.GetDetailedWeather(0)),false);
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
            await CheckAppConfiguration();
            if (!isDataLoaded)
            {
                await vm.BuildCompleteViewModel();
                isDataLoaded = true;
            }
            DailyWeatherView.BindingContext = vm.dailyWeatherVM;
            forecastlistView.ItemsSource = vm.forecastListVM;
            MainView.IsVisible = true;
            UserDialogs.Instance.HideLoading();
        }

        async Task CheckAppConfiguration ()
        {
            //try to get configuration
            configuration = AppServices.GetConfiguration();
            // init configuration if it's empty ( use default api key & user current location etc ..) 
            if (string.IsNullOrEmpty(configuration.APIKey))
            {
               await AppServices.InitConfiguration(configuration);
            }
            if (configuration.synchronization)
            {
                refreshTimer = new Timer(async (e) =>
                {
                    //FIXME
                   // await vm.PullCompleteData();
                }, null,0, (int.Parse(configuration.duration)) * 60 * 1000);
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
