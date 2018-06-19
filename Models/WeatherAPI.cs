using System;

namespace WeatherAPI.Models
{
    /// <summary>
    ///     Represents Weather Model
    /// </summary>
    public class Weather
    {
        /// <summary>
        /// Date
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Temperature
        /// </summary>
        public double Temp { get; set; }
        /// <summary>
        /// Wind speed
        /// </summary>
        public double WindSpeed { get; set; }
        /// <summary>
        /// Clouds
        /// </summary>
        public string Clouds { get; set; } 
    }
}
