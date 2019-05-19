using System;
using System.Collections.Generic;
using WeatherApp.Constants;
using WeatherApp.Models;
using WeatherApp.Services;
using Xamarin.Forms;

namespace WeatherApp.Pages
{
    public partial class ConfigurationPage : ContentPage
    {
        IAppServices appServices;
        ConfigurationModel configurationModel;
        public ConfigurationPage()
        {
            InitializeComponent();
            appServices = (Application.Current as App).AppServices;
            configurationModel = appServices.GetConfiguration();
            BindingContext = configurationModel;
        }
        //close button clicked
        void Close_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync(false);
        }
        //save button clicked
        async void SavleClicked(object sender, System.EventArgs e)
        {
            if (configurationModel.synchronization && string.IsNullOrEmpty(configurationModel.duration))
            {
                await DisplayAlert(ResourcesValues.AppName, ResourcesValues.FillDurationMessage, ResourcesValues.OkMessage);
            }
            else
            {
                await appServices.SaveConfiguration(configurationModel);
                await Navigation.PopModalAsync(false);
            }
        }
       
    }
}
