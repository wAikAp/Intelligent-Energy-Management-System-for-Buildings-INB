using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FYP_WEB_APP.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FYP_WEB_APP.Controllers
{
    public class WeatherController : Controller
    {
        private string CollectionName = "regional_weather";
        RegionalWeatherModel model = new RegionalWeatherModel();

        
        // GET: api/<WeatherDataController>
        [HttpGet]
        public RegionalWeatherModel GetWeather()
        {
            this.calHeatIndex(27,80);
            try
            {
                var sort = Builders<RegionalWeatherModel>.Sort.Descending("_id");
                model = new DBManger().DataBase.GetCollection<RegionalWeatherModel>(CollectionName).Find<RegionalWeatherModel>(schedule => true).Sort(sort).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Weather controller get data error: " + ex);
                return null;
            }
            return model;
        }

        [Route("Weather/Weather")]
        public ActionResult Weather()
        {

            ViewData["RegionalWeatherModel"] = GetWeather();
            return PartialView("Weather");
        }

        public void calHeatIndex(double T, double RH) {
            //heat index for out door indoor 
            T = 1.8 * T + 32;
            var HI = 0.5 * (T + 61 + (T - 68) * 1.2 + RH * 0.094);
            if (HI >= 80)
            {
                HI = -42.379 + 2.04901523 * T + 10.14333127 * RH - .22475541 * T * RH
                    - .00683783 * T * T - .05481717 * RH * RH + .00122874 * T * T * RH
                    + .00085282 * T * RH * RH - .00000199 * T * T * RH * RH;
                if (RH < 13 && 80 < T && T < 112)
                {
                    var ADJUSTMENT = (13 - RH) / 4 * Math.Sqrt((17 - Math.Abs(T - 95)) / 17);
                    HI -= ADJUSTMENT;
                }
                else if (RH > 85 && 80 < T && T < 87) {
                    var ADJUSTMENT = (RH - 85) * (87 - T) / 50;
                    HI += ADJUSTMENT;
                }
            }
            HI = Math.Round((HI - 32) / 1.8, 2);
            Debug.WriteLine("HeatIndex = "+ HI);
        }


    }

}
