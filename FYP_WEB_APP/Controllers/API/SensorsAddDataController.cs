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
            if (data != null && data.Count!=0)
            {
                foreach (var S in data)
                {

                    if (S.Equals(S.Sensorid))
                    {
                        Debug.WriteLine("is has value !!");
                        string id = S.Sensorid;
                        string Value = S.Value;
                        DateTime utcNow = DateTime.UtcNow;
                        str += "{ SensorId , " + id + "},{ Value," + Value + "},{ UserPostDate," + utcNow + "}\n";

                        collection.InsertOne(new BsonDocument { { "SensorId", id }, { "Value", Value }, { "UserPostDate", utcNow } });
                    }
                    else
                    {
                        str="Parameter cannot be null or [{\"SensorId\": \"..\",\"Value\": \"..\"}]\", \"original";
                    }
                }

            }
            else {
                str="Parameter cannot be null or [{\"SensorId\": \"..\",\"Value\": \"..\"}]\", \"original";
            }
            Debug.WriteLine(str);

            return str;
        }
    }
}
