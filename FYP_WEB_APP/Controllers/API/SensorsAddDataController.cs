using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP_WEB_APP.Controllers.Mongodb;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FYP_WEB_APP.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorsAddDataController : ControllerBase
    {
        public object Post(dynamic SensorJson)
        {
            var str = "";
            ConnectDB conn = new ConnectDB();
            IMongoDatabase database = conn.Conn();

            var collection = database.GetCollection<BsonDocument>("SENSOR");

            foreach (var S in SensorJson)
            {
                string id = S.SensorId;
                double Value = S.Value;
                DateTime utcNow = DateTime.UtcNow;
                str += "{ SensorId , " + id + "},{ Value," + Value + "},{ UserPostDate," + utcNow + "}\n";

                collection.InsertOne(new BsonDocument { { "SensorId", id }, { "Value", Value }, { "UserPostDate", utcNow } });

            }
            return "data==" + str;
        }
    }
}
