using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP_WEB_APP.Models.MongoModels;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FYP_WEB_APP.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceOnOffAPIController : ControllerBase
    {
        // GET: api/<DeviceOnOffAPIController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        

        [HttpPost]
        public void Post(object PostJson)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(PostJson);
            var data = JsonConvert.DeserializeObject<List<OnOffModel>>(json);
            foreach (var get in data)
            {

                var up = Builders<FYP_WEB_APP.Models.MongoModels.MongoDevicesListModel>.Update.Set(x => x.status, get.status);
                var UpdateResult = new Models.DBManger().DataBase.GetCollection<MongoDevicesListModel>("DEVICES_LIST").FindOneAndUpdateAsync(u => u.devicesId == get.deviceId, up);

            }
        }
    }
    public class OnOffModel
    {
        public string deviceId { get; set; }
        public bool status { get; set; }
    }
}
