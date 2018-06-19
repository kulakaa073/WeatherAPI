using System;
using System.Text.RegularExpressions;

namespace WeatherAPI.Utils
{
    static public class Utils
    {
        // Util function for convertation Unit time to DateTime
        static public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimeStamp).ToLocalTime();
        }

        //Validate city name string
        static public void ValidateCityName(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new Exception("City name field is empty.");
            else if (String.IsNullOrWhiteSpace(name))
                throw new Exception("City name field is empty.");
            else if (!Regex.IsMatch(name, @"^[a-zA-Z]*$"))
                throw new Exception("City name contain non-alphabetic characters.");
        }
    }
}
