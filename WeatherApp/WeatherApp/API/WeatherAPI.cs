using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.API
{
    public class WeatherAPI
    {
        string path = "https://api.apixu.com/v1/current.json?key=9202ffa8dc8a4bd68b1110943191405&q=Dusseldorf";
        HttpClient httpClient;
        WeatherModel weatherModel;
        public WeatherAPI()
        {
            httpClient = new HttpClient();
            weatherModel = new WeatherModel();

        }

        /// <summary>
        /// call apixu daily api with (location $ key) as params. and get daily weather data.
        /// return WeatherModel
        /// </summary>
        public async Task<WeatherModel> GetDailyWeather(string key , string location)
        {
            var response = await httpClient.GetAsync($"{path}key={key}&q={location}");
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                weatherModel =  Newtonsoft.Json.JsonConvert.DeserializeObject<WeatherModel>(result);
            }
            else
            {
                weatherModel = null;
            }
            return weatherModel;
        }
    }
}
