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
using FYP_WEB_APP.Models;

namespace FYP_WEB_APP.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorUpdateCurrentValueController : ControllerBase
    {[HttpPost]
        public string Post(object SensorJson)
        {
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(SensorJson);
          
            var data = JsonConvert.DeserializeObject<List<SensorUpdateCurrentValueModel>> (json);
            
            var str = "";
           bool isdone=true;

            if (SensorJson != null)
            {

                foreach (var S in data)
                {
                        if (string.IsNullOrEmpty(S.sensorId) || string.IsNullOrEmpty(S.value))
                        {
                            throw new System.ArgumentException("Parameter error", "original");

                        }
                        else { 
                        //  string id = S.sensorId;
                        //  string Value = S.value;
                        string valueNmae="";
                    string dbname = "";
                    bool isErrorData = false;
                    DateTime utcNow = DateTime.UtcNow;
                    switch (S.sensorId.Substring(0, 2)) {
                        case "TS":
                            dbname = "TMP_SENSOR";
                                    if (Convert.ToDouble(S.value) <=-120) {
                                        throw new System.ArgumentException("Sensor error", "The Sensor disConnnection");

                                    }
                                    else if (Convert.ToDouble(S.value) < 0) {
                                        throw new System.ArgumentException("Sensor value error", "value < 0");

                                    }
                                    else if( Convert.ToDouble(S.value) > 50) {
                                        throw new System.ArgumentException("Sensor value error", "value >50");
                                    }
                                    break;
                        case "HS":
                            dbname = "HUM_SENSOR";
                                    if (Convert.ToDouble(S.value) == -999) {
                                        throw new System.ArgumentException("Sensor error", "The Sensor disConnnection");
                                    }
                                    else if (Convert.ToDouble(S.value) < 0) {
                                        throw new System.ArgumentException("Sensor value error", "value < 0");

                                    }
                                    else if (Convert.ToDouble(S.value) > 100)
                                    {
                                        throw new System.ArgumentException("Sensor value error", "value >100");
                                    }
                                    break;
                        case "LS":
                            dbname = "LIGHT_SENSOR";
                                    if (Convert.ToDouble(S.value) == 0) {
                                        throw new System.ArgumentException("Sensor error", "The Sensor disConnnection");

                                    }
                                    else if (Convert.ToDouble(S.value) < 0) {
                                        throw new System.ArgumentException("Sensor value error", "value < 0");

                                    }
                                    else if (Convert.ToDouble(S.value) > 999)
                                    {
                                        throw new System.ArgumentException("Sensor value error", "value >999");

                                    }
                                    break;
                        default:
                            isdone = false;
                            str = isdone+": sensorId type not found!";
                            break;
                    }
                        FYP_APP.Controllers.SensorsController sensorC = new FYP_APP.Controllers.SensorsController();
                        var hasSensorInList=sensorC.GetAllSensors().Where(s => s.sensorId.Contains(S.sensorId));

                    if (isdone != false && isErrorData!=true && hasSensorInList.Count()!=0)
                    {

                        new DBManger().DataBase.GetCollection<BsonDocument>(dbname).InsertOne(new BsonDocument { { "sensorId", S.sensorId }, { "current", Convert.ToDouble(S.value)  }, { "latest_checking_time", utcNow.AddHours(8) } });

                        str += "{ sensorId , " + S.sensorId + "},{ value," + S.value + "},{ latest_checking_time," + utcNow + "}\n";

                    }
                    else {
                        return S.sensorId + " update false";
                            throw new System.ArgumentException("Parameter value error", "original");

                        }
                        }
                    }

            }
            else
            {
                isdone = false;

                str = isdone+": Parameter cannot be null ";

                    throw new System.ArgumentException("Parameter cannot be null", "original");
                }
                return isdone+": "+str;
            }
            catch (Exception e) { return e.ToString(); }
        }
    }
}
