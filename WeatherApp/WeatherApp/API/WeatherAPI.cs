using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.API
{
    public class WeatherAPI
    {
        string path = "https://api.apixu.com/v1/";
        HttpClient httpClient;
        WeatherModel weatherModel;
        public WeatherAPI()
        {
            httpClient = new HttpClient();
            weatherModel = new WeatherModel();
        }

        /// <summary>
        /// call apixu forecast api with (location $ key) as params. and get daily weather data.
        /// return WeatherModel
        /// </summary>
        public async Task<WeatherModel> GetForecastWeather(string key, string location,int numberOfDays)
        {
            var response = await httpClient.GetAsync($"{path}forecast.json?key={key}&q={location}&days={numberOfDays}");
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                weatherModel = Newtonsoft.Json.JsonConvert.DeserializeObject<WeatherModel>(result);
            }
            else
            {
                weatherModel = null;
            }
            return weatherModel;
        }
    }
}
    