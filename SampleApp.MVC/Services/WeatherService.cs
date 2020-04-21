using Newtonsoft.Json;
using SampleApp.MVC.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SampleApp.MVC.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<WeatherForecast>> GetWeather()
        {
            var uri = "/weatherforecast";
            var responseString = await _httpClient.GetStringAsync(uri);
            var forecast = JsonConvert.DeserializeObject<IEnumerable<WeatherForecast>>(responseString);
            return forecast;
        }
    }

    public interface IWeatherService
    {
        Task<IEnumerable<WeatherForecast>> GetWeather();
    }
}
