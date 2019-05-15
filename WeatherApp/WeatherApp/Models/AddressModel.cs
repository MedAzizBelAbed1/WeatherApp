using System;
namespace WeatherApp.Models
{
    public class AddressModel
    {
        private string _fullAddress;
        public string street { get; set; }
        public string zipCode { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string fullAddress
        {
            get
            {
                return $"{street} {zipCode} {city} {country}";
            }
            set
            {
                _fullAddress = value;
            }
        }
    }
}
