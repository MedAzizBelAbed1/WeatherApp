using System;
using System.Collections.Generic;
using WeatherApp.Models;
using WeatherApp.Services;
using Xamarin.Forms;

namespace WeatherApp.Pages
{
    public partial class ConfigurationPage : ContentPage
    {
        ConfigurationModel configurationModel;
        public ConfigurationPage()
        {
            InitializeComponent();
            configurationModel = AppServices.GetConfiguration();
            BindingContext = configurationModel;
        }
        void Close_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync();
        }
        async void SavleClicked(object sender, System.EventArgs e)
        {
            if (configurationModel.synchronization && string.IsNullOrEmpty(configurationModel.duration))
            {
               //FIXME SHOW ALERT
            }
            else
            {
                await AppServices.SaveConfiguration(configurationModel);
                await Navigation.PopModalAsync();
            }
        }
       
    }
}
