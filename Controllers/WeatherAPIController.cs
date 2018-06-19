using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherAPI.Models;

namespace WeatherAPI.Controllers
{
    #region snippet_Controller
    /// <summary>
    ///     Represents Weather controler
    /// </summary>
    [Route("[controller]")]
    public class WeatherAPIController : ControllerBase
    {
        private readonly WeatherAPISettings _weatherAPISettings;

        public WeatherAPIController(IOptions<WeatherAPISettings> weatherAPISettings)
        {
            _weatherAPISettings = weatherAPISettings.Value;
        }
        #endregion

        #region snippet_GetWeather
        /// <summary>
        /// Get current weather for specific city
        /// </summary>
        /// <param name="city"></param> 
        /// <returns>Current weather in city</returns>
        /// <response code="200">Return current weather</response>
        /// <response code="204">Data error</response>
        /// <response code="400">Bad request</response> 
        [HttpGet]
        [Route("GetWeather")]
        [Produces("application/json", Type = typeof(Weather))]
        [ProducesResponseType(typeof(Weather), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetWeather(string city)
        {
            try
            {
                // Validate city name field
                Utils.Utils.ValidateCityName(city);
            }
            catch (Exception exception)
            {
                return BadRequest($"Error: {exception.Message}");
            }
            using (var client = new HttpClient())
            {
                try
                {
                    // Building request url
                    client.BaseAddress = new Uri("http://api.openweathermap.org");
                    var appId = _weatherAPISettings.appId;
                    var response = await client.GetAsync($"/data/2.5/weather?q={city}&appid={appId}&units=metric");

                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();

                    //Parsing request result and building weather object
                    JObject fullWeather = JObject.Parse(stringResult);
                    try
                    {
                        Weather weather = new Weather
                        {
                            Date = Utils.Utils.UnixTimeStampToDateTime((double)fullWeather["dt"]),
                            Temp = (double)fullWeather["main"]["temp"],
                            WindSpeed = (double)fullWeather["wind"]["speed"],
                            Clouds = (string)fullWeather["clouds"]["all"]
                        };
                        return Ok(weather);
                    }
                    catch
                    {
                        return NoContent();
                    }
                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
                }
            }
            }
        #endregion

        #region snippet_GetWeatherForecast
        /// <summary>
        /// Get forecast for specific city
        /// </summary>
        /// <param name="city"></param> 
        /// <returns>Current weather in city</returns>
        /// <response code="200">Return current weather</response>
        /// <response code="204">Data error</response>
        /// <response code="400">Bad request</response> 
        [HttpGet]
        [Route("GetForecast")]
        [Produces("application/json", Type = typeof(Weather))]
        [ProducesResponseType(typeof(Weather), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetForecast(string city)
        {
            try
            {
                // Validate city name field
                Utils.Utils.ValidateCityName(city);
            }
            catch (Exception exception)
            {
                return BadRequest($"Error: {exception.Message}");
            }
            using (var client = new HttpClient())
            {
                try
                {
                    // Building request url
                    client.BaseAddress = new Uri("http://api.openweathermap.org");
                    var appId = _weatherAPISettings.appId;
                    var response = await client.GetAsync($"/data/2.5/forecast?q={city}&appid={appId}&units=metric");

                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();

                    //Parsing request result and building weather object
                    JObject fullWeather = JObject.Parse(stringResult);
                    List<Weather> weatherForecast = new List<Weather>();
                    try
                    {
                        foreach (var timePeriod in fullWeather["list"])
                        {
                            Weather weather = new Weather
                            {
                                Date = Utils.Utils.UnixTimeStampToDateTime((double)timePeriod["dt"]),
                                Temp = (double)timePeriod["main"]["temp"],
                                WindSpeed = (double)timePeriod["wind"]["speed"],
                                Clouds = (string)timePeriod["clouds"]["all"]
                            };
                            weatherForecast.Add(weather);
                        }
                        return Ok(weatherForecast);
                    }
                    catch
                    {
                        return NoContent();
                    }
                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
                }
            }
        }
        #endregion
    }
}