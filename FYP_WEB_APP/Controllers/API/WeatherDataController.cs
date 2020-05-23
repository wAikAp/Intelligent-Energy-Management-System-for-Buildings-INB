using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FYP_WEB_APP.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherDataController : ControllerBase
    {
        private readonly IMongoCollection<RegionalWeatherModel> _weather;

        // GET: api/<WeatherDataController>
        [HttpGet]
        public string Get()
        {
            var sort = Builders<RegionalWeatherModel>.Sort.Descending("_id");
            return _weather.Find<RegionalWeatherModel>(schedule => true).Sort(sort).FirstOrDefault().ToJson();
          //  return new string[] { "value1", "value2" };
        }

    }
    public class RegionalWeatherModel
    {
        public class DetailsModel
        {
            [Required]
            [BsonElement("temp_feels_like")]
            
            public double TempFeelsLike { get; set; }

            [Required]
            [BsonElement("temp_min")]
            public double TempMin { get; set; }

            [Required]
            [BsonElement("temp_max")]
            public double TempMax { get; set; }
        }

        public class HistoryModel
        {
            [Required]
            [BsonElement("time")]
            public string time { get; set; }

            [Required]
            [BsonElement("temperature")]
            public double Temperature { get; set; }

            [Required]
            [BsonElement("description")]
            public string Description { get; set; }

            [Required]
            [BsonElement("wind")]
            public double Wind { get; set; }

            [Required]
            [BsonElement("humidity")]
            public double Humidity { get; set; }

            [Required]
            [BsonElement("details")]
            public DetailsModel Details { get; set; }

        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [BsonElement("date")]
        public string Date { get; set;}

        [Required]
        [DataType(DataType.Text)]
        [BsonElement("last_update")]
        public string LastUpdate { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [BsonElement("location")]
        public string Location { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [BsonElement("temperature")]
        public double Temperature { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [BsonElement("description")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [BsonElement("humidity")]
        public double Humidity { get; set; }

        [Required]
        [BsonElement("details")]
        public DetailsModel Details { get; set; }

        [Required]
        [BsonElement("history")]
        public List<HistoryModel> History { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [BsonElement("records")]
        public double Records { get; set; }
    }
}
