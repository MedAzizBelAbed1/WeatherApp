using System;
using System.ComponentModel;

namespace WeatherApp.ViewModels
{
    public class WeatherViewModel
    {
        // daily weather view Model
        public class DailyWeatherViewModel : INotifyPropertyChanged
        {
            private string _textcolor;
            private string _region;
            private string _city;
            private string _country;
            private string _temperature;
            private string _icon;
            private string _condition;
            private string _feelLike;
            private string _humidity;
            private string _minMaxTemp;
            private string _wind;
            private string _backgroundImage;

            public string textcolor
            {
                get
                {
                    return _textcolor;
                }
                set
                {
                    _textcolor = value;
                    OnPropertyChanged("textcolor");
                }
            }
            public string region
            {
                get
                {
                    return _region;
                }
                set
                {
                    _region = value;
                    OnPropertyChanged("region");
                }
            }
            public string city
            {
                get
                {
                    return _city;
                }
                set
                {
                    _city = value;
                    OnPropertyChanged("city");
                }
            }
            public string country
            {
                get
                {
                    return _country;
                }
                set
                {
                    _country = value;
                    OnPropertyChanged("country");
                }
            }
            public string temperature
            {
                get
                {
                    return _temperature;
                }
                set
                {
                    _temperature = value;
                    OnPropertyChanged("temperature");
                }
            }
            public string icon
            {
                get
                {
                    return _icon;
                }
                set
                {
                    _icon = value;
                    OnPropertyChanged("icon");
                }
            }
            public string condition
            {
                get
                {
                    return _condition;
                }
                set
                {
                    _condition = value;
                    OnPropertyChanged("condition");
                }
            }
            public string feelLike
            {
                get
                {
                    return _feelLike;
                }
                set
                {
                    _feelLike = value;
                    OnPropertyChanged("feelLike");
                }
            }
            public string humidity
            {
                get
                {
                    return _humidity;
                }
                set
                {
                    _humidity = value;
                    OnPropertyChanged("humidity");
                }
            }
            public string minMaxTemp
            {
                get
                {
                    return _minMaxTemp;
                }
                set
                {
                    _minMaxTemp = value;
                    OnPropertyChanged("minMaxTemp");
                }
            }
            public string wind
            {
                get
                {
                    return _wind;
                }
                set
                {
                    _wind = value;
                    OnPropertyChanged("wind");
                }
            }
            public string backgroundImage
            {
                get
                {
                    return _backgroundImage;
                }
                set
                {
                    _backgroundImage = value;
                    OnPropertyChanged("backgroundImage");
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string propertyName)
            {
                var handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs(propertyName));
            }
    }

        // weekly weather view Model
        public class ForecastWeatherViewModel
        {
            public ForecastWeatherViewModel(string textcolor, string minTemp, string maxTemp, string day, string icon)
            {
                this.textcolor = textcolor;
                this.minTemp = minTemp;
                this.maxTemp = maxTemp;
                this.day = day;
                this.icon = icon;
            }

            public string textcolor { get; set; }
            public string minTemp { get; set; }
            public string maxTemp { get; set; }
            public string day { get; set; }
            public string icon { get; set; }
        }

        public class DetailedWeatherViewModel
        {
            public string day { get; set; }
            public string icon { get; set; }
            public string maxTemp { get; set; }
            public string minTemp { get; set; }
            public string maxWind { get; set; }
            public string humidity { get; set; }
            public string sunrise { get; set; }
            public string sunset { get; set; }
            public string moonrise { get; set; }
            public string moonset { get; set; }
        }
    }
}