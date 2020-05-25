using System;
using System.Collections.Generic;
using System.Diagnostics;
using FYP_WEB_APP.Controllers.Mongodb;
using FYP_WEB_APP.Models.API;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;


namespace FYP_WEB_APP.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorsAddDataController : ControllerBase
    {
        public object Post([FromBody]object SensorJson)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(SensorJson);

            var data = JsonConvert.DeserializeObject<List<SensorsAddDataModel>>(json);


            var str = "";
            ConnectDB conn = new ConnectDB();
            IMongoDatabase database = conn.Conn();

            var collection = database.GetCollection<BsonDocument>("SENSOR");
            if (SensorJson != null)
            {
     
                foreach (var S in data)
                {
                    
                    string id = S.Sid;
                        string Value = S.V;
                        DateTime utcNow = DateTime.UtcNow;
                        str += "{ SId , " + id + "},{ V," + Value + "},{ PD," + utcNow + "}\n";

                      collection.InsertOne(new BsonDocument { { "SId", id }, { "V", Value }, { "PD", utcNow } });
                }

            }
            else
            {
                str = "Parameter cannot be null ";
            }

            return str;
        }
    }
}
