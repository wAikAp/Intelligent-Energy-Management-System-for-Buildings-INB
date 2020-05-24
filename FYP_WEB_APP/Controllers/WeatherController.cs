using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            try
            {
                var sort = Builders<RegionalWeatherModel>.Sort.Descending("_id");
           
                model = new DBManger().Weatherdatabase.GetCollection<RegionalWeatherModel>(CollectionName).Find<RegionalWeatherModel>(schedule => true).Sort(sort).FirstOrDefault();
              
            }
            catch (Exception e) {
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
    }

}
