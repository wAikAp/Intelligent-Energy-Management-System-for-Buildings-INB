using FYP_WEB_APP.Models.MongoModels;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FYP_WEB_APP.Models.LogicModels
{
    public class ApparenTemperatureUtil
    {
        public double calHeatIndex(double T, double RH)
        {
            //heat index for out door indoor 
            T = 1.8 * T + 32;
            var HI = 0.5 * (T + 61 + (T - 68) * 1.2 + RH * 0.094);
            if (HI >= 80)
            {
                HI = -42.379 + 2.04901523 * T + 10.14333127 * RH - .22475541 * T * RH
                    - .00683783 * T * T - .05481717 * RH * RH + .00122874 * T * T * RH
                    + .00085282 * T * RH * RH - .00000199 * T * T * RH * RH;
                if (RH < 13 && 80 < T && T < 112)
                {
                    var ADJUSTMENT = (13 - RH) / 4 * Math.Sqrt((17 - Math.Abs(T - 95)) / 17);
                    HI -= ADJUSTMENT;
                }
                else if (RH > 85 && 80 < T && T < 87)
                {
                    var ADJUSTMENT = (RH - 85) * (87 - T) / 50;
                    HI += ADJUSTMENT;
                }
            }
            HI = Math.Round((HI - 32) / 1.8, 2);
           // Debug.WriteLine("HeatIndex = " + HI);
            return HI;
            
        }

        public double calApparenTemperature(double T, double RH, double V)
        {
            V = 1;
            //Where AT is somatosensory temperature (° C), T is air temperature (° C),
            //e is water pressure (hPa), V is wind speed (m / sec), RH is relative humidity (%)
            var e = RH / 100 * 6.105 * Math.Exp((17.26 * T) / (237.7 + T));
            var AT = 1.07 * T + 0.2 * e - 0.65 * V - 2.7;
            AT = Math.Round(AT, 2);
            Debug.WriteLine("Temp! = " + T);
            Debug.WriteLine("RH! = " + RH);
            Debug.WriteLine("calApparenTemperature = " + AT);
            return AT;
        }

        public double getAvgTemp(String roomId)
        {
            DBManger dbManager = new DBManger();
            double avgTemp = 0;
            int sensorsCount = 0;
            List<String> count = new List<String>();
            try
            {
                var SensorsListcollection = dbManager.DataBase.GetCollection<MongoSensorsListModel>("SENSOR_LIST");
                var filter = Builders<MongoSensorsListModel>.Filter.Eq("roomId", roomId);
                var documents = SensorsListcollection.Find(filter).ToList();
                foreach (MongoSensorsListModel document in documents)
                {
                  //  Debug.WriteLine(document.sensorId);
                    if (document.sensorId.Contains("TS"))
                    {
                        count.Add(document.sensorId);
                    }
                }
                foreach (String sensorId in count)
                {
                   // Debug.WriteLine("Matched sensors:" + sensorId);
                    var Tcollection = dbManager.DataBase.GetCollection<MongoTmpSensor_Model>("TMP_SENSOR");
                    var sort = Builders<MongoTmpSensor_Model>.Sort.Descending("latest_checking_time");
                    var Tfilter = Builders<MongoTmpSensor_Model>.Filter.Eq("sensorId", sensorId);
                    var Tdocuments = Tcollection.Find(Tfilter).Sort(sort).FirstOrDefault();
                    if (Tdocuments != null)
                    {
                        Debug.WriteLine(Tdocuments.sensorId + " current " + Tdocuments.current + " record time:" + Tdocuments.latest_checking_time);
                        avgTemp += Tdocuments.current;
                        sensorsCount++;
                    }
                }
                Debug.WriteLine("avg current " + avgTemp + "/" + sensorsCount + "=" + avgTemp / sensorsCount);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
            }
           

            
            return Math.Round(avgTemp / sensorsCount,2);
        } //get data from sensors

        public double getAvgHum(String roomId)
        {
            DBManger dbManager = new DBManger();
            double avgHum = 0;
            int sensorsCount = 0;
            List<String> count = new List<String>();
            try
            {
                var SensorsListcollection = dbManager.DataBase.GetCollection<MongoSensorsListModel>("SENSOR_LIST");
                var filter = Builders<MongoSensorsListModel>.Filter.Eq("roomId", roomId);
                var documents = SensorsListcollection.Find(filter).ToList();
                foreach (MongoSensorsListModel document in documents)
                {
                   // Debug.WriteLine(document.sensorId);
                    if (document.sensorId.Contains("HS"))
                    {
                        count.Add(document.sensorId);
                    }
                }
                foreach (String sensorId in count)
                {
                  //  Debug.WriteLine("Matched sensors:" + sensorId);
                    var Tcollection = dbManager.DataBase.GetCollection<MongoHumSemsor_Model>("HUM_SENSOR");
                    var sort = Builders<MongoHumSemsor_Model>.Sort.Descending("latest_checking_time");
                    var Tfilter = Builders<MongoHumSemsor_Model>.Filter.Eq("sensorId", sensorId);
                    var Tdocuments = Tcollection.Find(Tfilter).Sort(sort).FirstOrDefault();
                    if (Tdocuments != null)
                    {
                    //    Debug.WriteLine(Tdocuments.sensorId + " current " + Tdocuments.current + " record time:" + Tdocuments.latest_checking_time);
                        avgHum += Tdocuments.current;
                        sensorsCount++;
                    }
                }

              //  Debug.WriteLine("avg current " + avgHum + "/" + sensorsCount + "=" + avgHum / sensorsCount);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
            }
            return Math.Round(avgHum / sensorsCount, 2);
        }

        public double getAvgLig(String roomId)
        {
            DBManger dbManager = new DBManger();
            double avg = 0;
            int sensorsCount = 0;
            List<String> count = new List<String>();
            try
            {
                var SensorsListcollection = dbManager.DataBase.GetCollection<MongoSensorsListModel>("SENSOR_LIST");
                var filter = Builders<MongoSensorsListModel>.Filter.Eq("roomId", roomId);
                var documents = SensorsListcollection.Find(filter).ToList();
                foreach (MongoSensorsListModel document in documents)
                {
                  //  Debug.WriteLine(document.sensorId);
                    if (document.sensorId.Contains("LS"))
                    {
                        count.Add(document.sensorId);
                    }
                }
                foreach (String sensorId in count)
                {
                  //  Debug.WriteLine("Matched sensors:" + sensorId);
                    var Tcollection = dbManager.DataBase.GetCollection<MongoLigSensor_Model>("LIGHT_SENSOR");
                    var sort = Builders<MongoLigSensor_Model>.Sort.Descending("latest_checking_time");
                    var Tfilter = Builders<MongoLigSensor_Model>.Filter.Eq("sensorId", sensorId);
                    var Tdocuments = Tcollection.Find(Tfilter).Sort(sort).FirstOrDefault();
                    if (Tdocuments != null)
                    {
                   //     Debug.WriteLine(Tdocuments.sensorId + " current " + Tdocuments.current + " record time:" + Tdocuments.latest_checking_time);
                        avg += Tdocuments.current;
                        sensorsCount++;
                    }
                }

             //   Debug.WriteLine("avg current " + avg + "/" + sensorsCount + "=" + avg / sensorsCount);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return Math.Round(avg / sensorsCount, 2);
        }

        public Boolean setAcCurrent(String roomId, double current)//set data
        {
            DBManger dbManager = new DBManger();
            List<String> count = new List<String>();
            try
            {
                var devicesListcollection = dbManager.DataBase.GetCollection<MongoDevicesListModel>("DEVICES_LIST");
                var ACCollection = dbManager.DataBase.GetCollection<MongoAC_Model>("AC");
                var filter = Builders<MongoDevicesListModel>.Filter.Eq("roomId", roomId) & Builders<MongoDevicesListModel>.Filter.Regex("devicesId","AC");
                var documents = devicesListcollection.Find(filter).ToList();
                foreach (MongoDevicesListModel document in documents)
                {
                    Debug.WriteLine(document.devicesId);
                    var filter2 = Builders<MongoDevicesListModel>.Filter.Eq("devicesId", document.devicesId);
                    var update = Builders<MongoDevicesListModel>.Update.Set("set_value", current);
                    devicesListcollection.UpdateOne(filter2, update);

                    MongoAC_Model mongoAC_Model = new MongoAC_Model();
                    mongoAC_Model.devicesId = document.devicesId;
                    mongoAC_Model.current = current;
                    mongoAC_Model.latest_checking_time = DateTime.UtcNow.AddHours(8);
                    ACCollection.InsertOne(mongoAC_Model);
                }
                return true;
                
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
            
        }

        public Boolean setAcCurrentAndTurnON(String roomId, double current)//set data
        {
            DBManger dbManager = new DBManger();
            List<String> count = new List<String>();
            try
            {
                var devicesListcollection = dbManager.DataBase.GetCollection<MongoDevicesListModel>("DEVICES_LIST");
                var ACCollection = dbManager.DataBase.GetCollection<MongoAC_Model>("AC");
                var filter = Builders<MongoDevicesListModel>.Filter.Eq("roomId", roomId) & Builders<MongoDevicesListModel>.Filter.Regex("devicesId", "AC");
                var documents = devicesListcollection.Find(filter).ToList();
                foreach (MongoDevicesListModel document in documents)
                {
                    Debug.WriteLine(document.devicesId);
                    var filter2 = Builders<MongoDevicesListModel>.Filter.Eq("devicesId", document.devicesId);
                    var update = Builders<MongoDevicesListModel>.Update.Set("set_value", current).Set("status", true);
                    devicesListcollection.UpdateOne(filter2, update);

                    MongoAC_Model mongoAC_Model = new MongoAC_Model();
                    mongoAC_Model.devicesId = document.devicesId;
                    mongoAC_Model.current = current;
                    mongoAC_Model.latest_checking_time = DateTime.UtcNow.AddHours(8);
                    ACCollection.InsertOne(mongoAC_Model);
                }
                return true;

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }

        }

        public Boolean setLTCurrent(String roomId, double current)//set data
        {
            DBManger dbManager = new DBManger();
            List<String> count = new List<String>();
            try
            {
                var devicesListcollection = dbManager.DataBase.GetCollection<MongoDevicesListModel>("DEVICES_LIST");
                var Collection = dbManager.DataBase.GetCollection<MongoLig_Model>("LIGHT");
                var filter = Builders<MongoDevicesListModel>.Filter.Eq("roomId", roomId) & Builders<MongoDevicesListModel>.Filter.Regex("devicesId", "LT");
                var documents = devicesListcollection.Find(filter).ToList();
                foreach (MongoDevicesListModel document in documents)
                {
                    //Debug.WriteLine(document.devicesId);
                    var filter2 = Builders<MongoDevicesListModel>.Filter.Eq("devicesId", document.devicesId);
                    var update = Builders<MongoDevicesListModel>.Update.Set("set_value", current);
                    devicesListcollection.UpdateOne(filter2, update);

                    MongoLig_Model mongo_Model = new MongoLig_Model();
                    mongo_Model.devicesId = document.devicesId;
                    mongo_Model.current = current;
                    mongo_Model.latest_checking_time = DateTime.UtcNow.AddHours(8);
                    Collection.InsertOne(mongo_Model);
                }
                return true;

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }

        public Boolean setLTCurrentAndTurnON(String roomId, double current)//set data
        {
            DBManger dbManager = new DBManger();
            List<String> count = new List<String>();
            try
            {
                var devicesListcollection = dbManager.DataBase.GetCollection<MongoDevicesListModel>("DEVICES_LIST");
                var Collection = dbManager.DataBase.GetCollection<MongoLig_Model>("LIGHT");
                var filter = Builders<MongoDevicesListModel>.Filter.Eq("roomId", roomId) & Builders<MongoDevicesListModel>.Filter.Regex("devicesId", "LT");
                var documents = devicesListcollection.Find(filter).ToList();
                foreach (MongoDevicesListModel document in documents)
                {
                    //Debug.WriteLine(document.devicesId);
                    var filter2 = Builders<MongoDevicesListModel>.Filter.Eq("devicesId", document.devicesId);
                    var update = Builders<MongoDevicesListModel>.Update.Set("set_value", current).Set("status", true);
                    devicesListcollection.UpdateOne(filter2, update);

                    MongoLig_Model mongo_Model = new MongoLig_Model();
                    mongo_Model.devicesId = document.devicesId;
                    mongo_Model.current = current;
                    mongo_Model.latest_checking_time = DateTime.UtcNow.AddHours(8);
                    Collection.InsertOne(mongo_Model);
                }
                return true;

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }

        public string getUIColor(double AT) {
            
            var ATbgCol = "badge-secondary";
            if (AT > 26 && AT < 30)
            {
                ATbgCol = "badge-warning";
            }
            else if (AT > 30)
            {
                ATbgCol = "badge-danger";
            }
            else if (21 <AT && AT <= 26 )
            {
                ATbgCol = "badge-primary";
            }
            else if (AT <= 21 && AT > 0)
            {
                ATbgCol = "badge-info";
            }
            else {
                ATbgCol = "badge-secondary";
            }
            return ATbgCol;
        }

        public List<String> getRoomList()
        {
            DBManger dbManager = new DBManger();
            List<String> roomlist = new List<string>();
            var collection = dbManager.DataBase.GetCollection<RoomsListModel>("ROOM");
			var documents = collection.Find(new BsonDocument()).ToList();
			foreach (var element in documents)
			{
                roomlist.Add(element.roomId);
                //Debug.WriteLine("Room list" + element.roomId);
            }
            return roomlist;

        }
    }
}
