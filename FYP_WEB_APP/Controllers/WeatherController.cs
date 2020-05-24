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
        private string dbName = "mydb";
        private string CollectionName = "regional_weather";
        private string ConnectionString = "mongodb+srv://hkteam1:IUXsr2ZYKQuPu0Sj@issf2020hk-la5xb.gcp.mongodb.net/test?retryWrites=true&w=majority";





        // GET: api/<WeatherDataController>
        [HttpGet]
        public RegionalWeatherModel GetWeather()
        {

            MongoClient dbClient = new MongoClient(ConnectionString);

            IMongoDatabase database = dbClient.GetDatabase(dbName);


            var sort = Builders<RegionalWeatherModel>.Sort.Descending("_id");
            return database.GetCollection<RegionalWeatherModel>(CollectionName).Find<RegionalWeatherModel>(schedule => true).Sort(sort).FirstOrDefault(); ;

        }
        [Route("Weather/Weather")]
            public ActionResult Weather()
        {

            ViewData["RegionalWeatherModel"] = GetWeather();
            return PartialView("Weather");
        }
    }

}
