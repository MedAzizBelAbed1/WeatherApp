using System;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public interface IAppServices
    {
        Task SaveAddress(AddressModel addressModel);
        AddressModel GetAddress();
        Task SaveConfiguration(ConfigurationModel configuration, bool refreshData = true);
        ConfigurationModel GetConfiguration();
        Task InitConfiguration(ConfigurationModel configuration);
        Task SaveLastUserLocation(bool isCurrentPostion);
    }
}
