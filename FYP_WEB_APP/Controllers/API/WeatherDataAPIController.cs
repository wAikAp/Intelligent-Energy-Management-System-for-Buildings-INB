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
    public class WeatherDataAPIController : ControllerBase
    {

        // GET: api/<WeatherDataController>
        [HttpGet]
        public string Get()
        {
            return new WeatherDataController().GetWeather().ToJson();
        }

    }
}
