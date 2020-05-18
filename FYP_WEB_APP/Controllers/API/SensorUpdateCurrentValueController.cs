using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using FYP_WEB_APP.Models.MongoModels;
using System.Diagnostics;
using FYP_WEB_APP.Models.API;
using Newtonsoft.Json;
using FYP_WEB_APP.Controllers.Mongodb;
using MongoDB.Driver;

namespace FYP_WEB_APP.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorUpdateCurrentValueController : ControllerBase
    {[HttpPost]
        public string Post(object SensorJson)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(SensorJson);

            var data = JsonConvert.DeserializeObject<List<SensorUpdateCurrentValueModel>> (json);
            
            var str = "";
            ConnectDB conn = new ConnectDB();
            IMongoDatabase database = conn.Conn();
            bool isdone=true;
            var updateOptions = new UpdateOptions { IsUpsert = true };

            if (SensorJson != null)
            {

                foreach (var S in data)
                {

                    string id = S.sensorId;
                    string Value = S.value;
                    string valueNmae="";
                    string dbname = "";
                    DateTime utcNow = DateTime.UtcNow;
                    switch (id.Substring(0, 2)) {
                        case "TS":
                            valueNmae = "current";
                            dbname = "TMP_SENSOR";
                            break;
                        case "HS":
                            valueNmae = "current";
                            dbname = "HUM_SENSOR";
                            break;
                        case "LS":
                            valueNmae = "current";
                            dbname = "LIGHT_SENSOR";
                            break;
                        default:
                            isdone = false;
                            str = isdone+": sensorId type not found!";
                            break;
                    }
                    if (isdone != false)
                    {
                        var collection = database.GetCollection<BsonDocument>(dbname);

                        /*var filter = Builders<BsonDocument>.Filter.Eq("sensorId", id);
                        var up = Builders<BsonDocument>.Update.Set(valueNmae, Value);
                        var Updated = collection.UpdateOne(filter, up, updateOptions);
                         up = Builders<BsonDocument>.Update.Set("latest_checking_time", DateTime.UtcNow);
                         Updated = collection.UpdateOne(filter, up, updateOptions);
                        */
                        collection.InsertOne(new BsonDocument { { "sensorId", id }, { "current", Convert.ToDouble(Value)  }, { "latest_checking_time", utcNow } });

                        str += "{ sensorId , " + id + "},{ value," + Value + "},{ latest_checking_time," + utcNow + "}\n";

                    }
                    else {
                        return str;

                    }
                }

            }
            else
            {
                isdone = false;

                str = isdone+": Parameter cannot be null ";
                return str;

            }
            return isdone+": "+str;
        }
    }
}
