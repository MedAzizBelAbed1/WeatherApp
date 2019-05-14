using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.API;
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
        public MainPage()
        {
            InitializeComponent();
            vm = new WeatherViewModel();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await vm.LoadAllData();
            DailyWeatherView.BindingContext = vm.dailyWeather;
            forecastlistView.ItemsSource = vm.forecastList;
        }
    }
}
