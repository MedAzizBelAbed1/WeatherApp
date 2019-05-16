using System;
using System.ComponentModel;

namespace WeatherApp.Models
{
    public class ConfigurationModel : INotifyPropertyChanged
    {
        private bool _synchronization;
        public string APIKey { get; set; }
        public string numberOfDays { get; set; }
        public string lastUserLocation { get; set; }
        public bool runAnimation { get; set; }
        public bool synchronization
        {
            get
            {
                return _synchronization;
            }
            set
            {
                _synchronization = value;
                OnPropertyChanged("synchronization");
            }
        }
        public string duration { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
