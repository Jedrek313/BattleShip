using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShip.Models
{
    public class WeatherForecast
    {
        public static String city = "Warsaw";
        public static String requestURL = "http://api.openweathermap.org/data/2.5/weather?q=" + city + ",pl&APPID=4878fd9db754512ef66c8a6fcd34cb58";


    public double getCurrentWeatherKelven()
    {
        return 0;
    }
public double getCurrentWeatherCelcius()
{
return getCurrentWeatherKelven() - 273.15;
}


}
}
