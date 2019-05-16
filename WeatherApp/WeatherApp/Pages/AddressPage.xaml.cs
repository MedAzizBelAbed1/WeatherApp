using System;
using System.Collections.Generic;
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
            Navigation.PopModalAsync();
        }
        async void SavleClicked(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(addressModel.country))
            {
                await AppServices.SaveAddress(addressModel);
                await Navigation.PopModalAsync();
            }
            else
            { // FIXME Add ALert
            }
        }
    }
}
