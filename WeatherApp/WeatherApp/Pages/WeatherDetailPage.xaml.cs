using System;
using System.Collections.Generic;
using WeatherApp.ViewModels;
using Xamarin.Forms;
using static WeatherApp.ViewModels.WeatherViewModel;

namespace WeatherApp.Pages
{
    public partial class WeatherDetailPage : ContentPage
    {
        public WeatherDetailPage(DetailedWeatherViewModel detailedWeather)
        {
            InitializeComponent();
            BindingContext = detailedWeather;
        }

        void Close_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync();
        }
      
    }
}
