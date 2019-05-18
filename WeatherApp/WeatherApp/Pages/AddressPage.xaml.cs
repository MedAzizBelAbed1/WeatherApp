using System;
using WeatherApp.Constants;
using Acr.UserDialogs;
using WeatherApp.Models;
using WeatherApp.Services;
using Xamarin.Forms;

namespace WeatherApp.Pages
{
    public partial class AddressPage : ContentPage
    {
        AddressModel addressModel;
        public AddressPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            addressModel = AppServices.GetAddress();
            BindingContext = addressModel;

        }
        void Close_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync(false);
        }
        async void SavleClicked(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(addressModel.country))
            {
                UserDialogs.Instance.ShowLoading();
                await AppServices.SaveAddress(addressModel);
                await Navigation.PopModalAsync(false);
            }
            else
            {
               await  DisplayAlert(ResourcesValues.AppName, ResourcesValues.FillCountryMessage, ResourcesValues.OkMessage);
            }
        }
    }
}
