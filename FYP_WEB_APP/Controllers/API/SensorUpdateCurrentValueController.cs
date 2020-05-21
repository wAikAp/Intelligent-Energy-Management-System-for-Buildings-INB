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
    {
        DateTime utcNow = DateTime.UtcNow.AddHours(8);
        
        [HttpPost]
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
                        if (string.IsNullOrEmpty(S.sensorId))
                        {
                            throw new System.ArgumentException("Parameter error", "original");

                        }
                        else {
                            //  string id = S.sensorId;
                            //  string Value = S.value;
                            string valueNmae = "";
                            string dbname = "";
                            bool isErrorData = false;
                            switch (S.sensorId.Substring(0, 2)) {
                                case "TS":
                                    dbname = "TMP_SENSOR";
                                    if (Convert.ToDouble(S.values.First()) <= -120) {
                                        isErrorData = true;
                                        doException(S.sensorId, false, "The Sensor is disConnnection");
                                    }
                                    else if (Convert.ToDouble(S.values.First()) < 0) {
                                        isErrorData = true;

                                        doException(S.sensorId, "Sensor value error, value < 0");
                                    }
                                    else if (Convert.ToDouble(S.values.First()) > 50) {
                                        isErrorData = true;

                                        doException(S.sensorId, "Sensor value error, value > 50");
                                    }
                                    break;
                                case "HS":
                                    dbname = "HUM_SENSOR";
                                    if (Convert.ToDouble(S.values.First()) == -999) {
                                        isErrorData = true;

                                        doException(S.sensorId, false, "The Sensor is disConnnection");

                                    }
                                    else if (Convert.ToDouble(S.values.First()) < 0) {
                                        isErrorData = true;

                                        doException(S.sensorId, "Sensor value error,  value  " + S.values.First() + " < 0");
                                    }
                                    else if (Convert.ToDouble(S.values.First()) > 100)
                                        isErrorData = true;
                                    {
                                        doException(S.sensorId, "Sensor value error,  value " + S.values.First() + " > 100");
                                    }
                                    break;
                                case "LS":
                                    dbname = "LIGHT_SENSOR";
                                    if (Convert.ToDouble(S.values.First()) < 0) {
                                        isErrorData = true;

                                        doException(S.sensorId, false, "The Sensor is disConnnection");
                                    }
                                    else if (Convert.ToDouble(S.values.First()) < 0) {
                                        isErrorData = true;

                                        doException(S.sensorId, "Sensor value error,  value  " + S.values.First() + " < 0");
                                    }
                                    else if (Convert.ToDouble(S.values.First()) > 999)
                                    {
                                        isErrorData = true;

                                        doException(S.sensorId, "Sensor value error, value " + S.values.First() + " > 999");
                                    }
                                    break;
                                case "AS":
                                    dbname = "AS_SENSOR";
                                    Debug.WriteLine("\n\n AS SENSOR ==>");

                                    break;
                                default:
                                    isdone = false;
                                    str = isdone + ": sensorId type not found!";
                                    break;
                            }
                            FYP_APP.Controllers.SensorsController sensorC = new FYP_APP.Controllers.SensorsController();
                            var hasSensorInList = sensorC.GetAllSensors().Where(s => s.sensorId.Contains(S.sensorId));
                            if (S.values.Length > 0) {
                                if (S.values.Length == 1)
                                {
                                    Debug.WriteLine(S.values.First().GetType());
                                    
                                    if (isdone != false && isErrorData != true && hasSensorInList.Count() != 0 && dbname != "AS_SENSOR" && !string.IsNullOrEmpty(S.value))
                                    {
                                        new DBManger().DataBase.GetCollection<BsonDocument>(dbname).InsertOne(new BsonDocument { { "sensorId", S.sensorId }, { "current", S.value.First() }, { "latest_checking_time", utcNow } });
                                        trueStatus(S.sensorId);
                                        str += "{ sensorId , " + S.sensorId + "},{ value," + S.value + "},{ latest_checking_time," + utcNow + "}\n";

                                    }
                                }
                                else if (S.values.Length > 1)
                                {
                                    Debug.WriteLine(S.values);
                                    new DBManger().DataBase.GetCollection<BsonDocument>(dbname).InsertOne(new BsonDocument {
                                    { "sensorId", S.sensorId },
                                    { "C_CO", S.values[0] },
                                    { "C_CO2", S.values[1] },
                                    { "C_CI2", S.values[2] },
                                    { "C_CH20", S.values[3] },
                                    { "C_H2", S.values[4] },
                                    { "C_CH4", S.values[5] },
                                    { "C_H2S", S.values[6] },
                                     { "C_NO2", S.values[7] },
                                   { "C_O3", S.values[8] },
                                    { "C_C2CI4", S.values[9] },
                                   { "C_SO2", S.values[10] },
                                   { "C_VOC", S.values[11] },
                                    { "C_AVG_PM25", S.values[12] },
                                    { "latest_checking_time", utcNow } });


                                }
                                else {
                                    isdone = false;

                                    throw new System.ArgumentException("Parameter cannot be null", "original");

                                }
                            }
                  /* if (isdone != false && isErrorData!=true && hasSensorInList.Count()!=0 && dbname!= "AS_SENSOR" && !string.IsNullOrEmpty(S.value))
                    {
                        new DBManger().DataBase.GetCollection<BsonDocument>(dbname).InsertOne(new BsonDocument { { "sensorId", S.sensorId }, { "current", Convert.ToDouble(S.value)  }, { "latest_checking_time", utcNow } });
                                trueStatus(S.sensorId);
                        str += "{ sensorId , " + S.sensorId + "},{ value," + S.value + "},{ latest_checking_time," + utcNow + "}\n";

                    }else if (dbname == "AS_SENSOR") {
                                Debug.WriteLine("\n\n AS SENSOR ==>");

                                new DBManger().DataBase.GetCollection<BsonDocument>(dbname).InsertOne(new BsonDocument { 
                                    { "sensorId", S.sensorId },
                                    { "C_CO", S.C_CO },
                                    { "C_CO2", S.C_CO2 },
                                    { "C_CI2", S.C_CI2 },
                                    { "C_CH20", S.C_CH20 },
                                    { "C_H2", S.C_H2 },
                                    { "C_CH4", S.C_CH4 },
                                    { "C_H2S", S.C_H2S },
                                     { "C_NO2", S.C_NO2 },
                                   { "C_O3", S.C_O3 },
                                    { "C_C2CI4", S.C_C2CI4 },
                                   { "C_SO2", S.C_SO2 },
                                   { "C_VOC", S.C_VOC },
                                    { "C_AVG_PM25", S.C_AVG_PM25 },
                                    { "latest_checking_time", utcNow } });
                                trueStatus(S.sensorId);
                                //str += "{ sensorId , " + S.sensorId + "},{ value," + S.value + "},{ latest_checking_time," + utcNow + "}\n";

                            }
                            else
                            {
                                return S.sensorId + "error data " + S.value;
                                //throw new System.ArgumentException("Parameter value error", "original");

                            }
                    */
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
        public void doException(string id,string error) {
            var filter = Builders<MongoSensorsListModel>.Filter.Eq("sensorId", id);
            var up = Builders<MongoSensorsListModel>.Update.Set("Exception", error);
            var Updated = new DBManger().DataBase.GetCollection<MongoSensorsListModel>("SENSOR_LIST").UpdateOne(filter, up);
            up = Builders<MongoSensorsListModel>.Update.Set("latest_checking_time", utcNow);
            Updated = new DBManger().DataBase.GetCollection<MongoSensorsListModel>("SENSOR_LIST").UpdateOne(filter, up);
        }
        public void doException(string id,bool status, string error)
        {
            var filter = Builders<MongoSensorsListModel>.Filter.Eq("sensorId", id);
            var up = Builders<MongoSensorsListModel>.Update.Set("Exception", error);
            var Updated = new DBManger().DataBase.GetCollection<MongoSensorsListModel>("SENSOR_LIST").UpdateOne(filter, up);
            up = Builders<MongoSensorsListModel>.Update.Set("status", status);
            Updated = new DBManger().DataBase.GetCollection<MongoSensorsListModel>("SENSOR_LIST").UpdateOne(filter, up);
            up = Builders<MongoSensorsListModel>.Update.Set("latest_checking_time", utcNow);
            Updated = new DBManger().DataBase.GetCollection<MongoSensorsListModel>("SENSOR_LIST").UpdateOne(filter, up);
        }
        public void trueStatus(string id)
        {

            var filter = Builders<MongoSensorsListModel>.Filter.Eq("sensorId", id);
            var up = Builders<MongoSensorsListModel>.Update.Set("Exception", "");
            var Updated = new DBManger().DataBase.GetCollection<MongoSensorsListModel>("SENSOR_LIST").UpdateOne(filter, up);
            up = Builders<MongoSensorsListModel>.Update.Set("status", true);
            Updated = new DBManger().DataBase.GetCollection<MongoSensorsListModel>("SENSOR_LIST").UpdateOne(filter, up);
            up = Builders<MongoSensorsListModel>.Update.Set("latest_checking_time", utcNow);
            Updated = new DBManger().DataBase.GetCollection<MongoSensorsListModel>("SENSOR_LIST").UpdateOne(filter, up);
        }
    }
}
