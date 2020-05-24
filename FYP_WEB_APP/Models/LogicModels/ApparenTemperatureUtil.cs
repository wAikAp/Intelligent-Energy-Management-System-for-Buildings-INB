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
            Debug.WriteLine("HeatIndex = " + HI);
            return HI;
            
        }

        public double calApparenTemperature(double T, double RH, double V)
        {
            //Where AT is somatosensory temperature (° C), T is air temperature (° C),
            //e is water pressure (hPa), V is wind speed (m / sec), RH is relative humidity (%)
            var e = RH / 100 * 6.105 * Math.Exp((17.26 * T) / (237.7 + T));
            var AT = 1.07 * T + 0.2 * e - 0.65 * V - 2.7;
            AT = Math.Round(AT, 2);
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
                    Debug.WriteLine(document.sensorId);
                    if (document.sensorId.Contains("TS"))
                    {
                        count.Add(document.sensorId);
                    }
                }
                foreach (String sensorId in count)
                {
                    Debug.WriteLine("Matched sensors:" + sensorId);
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
                    Debug.WriteLine(document.sensorId);
                    if (document.sensorId.Contains("HS"))
                    {
                        count.Add(document.sensorId);
                    }
                }
                foreach (String sensorId in count)
                {
                    Debug.WriteLine("Matched sensors:" + sensorId);
                    var Tcollection = dbManager.DataBase.GetCollection<MongoHumSemsor_Model>("HUM_SENSOR");
                    var sort = Builders<MongoHumSemsor_Model>.Sort.Descending("latest_checking_time");
                    var Tfilter = Builders<MongoHumSemsor_Model>.Filter.Eq("sensorId", sensorId);
                    var Tdocuments = Tcollection.Find(Tfilter).Sort(sort).FirstOrDefault();
                    if (Tdocuments != null)
                    {
                        Debug.WriteLine(Tdocuments.sensorId + " current " + Tdocuments.current + " record time:" + Tdocuments.latest_checking_time);
                        avgHum += Tdocuments.current;
                        sensorsCount++;
                    }
                }

                Debug.WriteLine("avg current " + avgHum + "/" + sensorsCount + "=" + avgHum / sensorsCount);
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
                    Debug.WriteLine(document.sensorId);
                    if (document.sensorId.Contains("LS"))
                    {
                        count.Add(document.sensorId);
                    }
                }
                foreach (String sensorId in count)
                {
                    Debug.WriteLine("Matched sensors:" + sensorId);
                    var Tcollection = dbManager.DataBase.GetCollection<MongoLigSensor_Model>("LIGHT_SENSOR");
                    var sort = Builders<MongoLigSensor_Model>.Sort.Descending("latest_checking_time");
                    var Tfilter = Builders<MongoLigSensor_Model>.Filter.Eq("sensorId", sensorId);
                    var Tdocuments = Tcollection.Find(Tfilter).Sort(sort).FirstOrDefault();
                    if (Tdocuments != null)
                    {
                        Debug.WriteLine(Tdocuments.sensorId + " current " + Tdocuments.current + " record time:" + Tdocuments.latest_checking_time);
                        avg += Tdocuments.current;
                        sensorsCount++;
                    }
                }

                Debug.WriteLine("avg current " + avg + "/" + sensorsCount + "=" + avg / sensorsCount);
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
            int sensorsCount = 0;
            List<String> count = new List<String>();
            try
            {
                var SensorsListcollection = dbManager.DataBase.GetCollection<MongoSensorsListModel>("DEVICES_LIST");
                var filter = Builders<MongoSensorsListModel>.Filter.Eq("roomId", roomId);
                var documents = SensorsListcollection.Find(filter).ToList();
                foreach (MongoSensorsListModel document in documents)
                {
                    Debug.WriteLine(document.sensorId);
                    if (document.sensorId.Contains("LS"))
                    {
                        count.Add(document.sensorId);
                    }
                }
                foreach (String sensorId in count)
                {
                    Debug.WriteLine("Matched sensors:" + sensorId);
                    var Tcollection = dbManager.DataBase.GetCollection<MongoLigSensor_Model>("LIGHT_SENSOR");
                    var sort = Builders<MongoLigSensor_Model>.Sort.Descending("latest_checking_time");
                    var Tfilter = Builders<MongoLigSensor_Model>.Filter.Eq("sensorId", sensorId);
                    var Tdocuments = Tcollection.Find(Tfilter).Sort(sort).FirstOrDefault();
                    if (Tdocuments != null)
                    {
                        Debug.WriteLine(Tdocuments.sensorId + " current " + Tdocuments.current + " record time:" + Tdocuments.latest_checking_time);
                        avg += Tdocuments.current;
                        sensorsCount++;
                    }
                }

                Debug.WriteLine("avg current " + avg + "/" + sensorsCount + "=" + avg / sensorsCount);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return Math.Round(avg / sensorsCount, 2);
        }


        public string getUIColor(double AT) {
            
            var ATbgCol = "badge-secondary";
            if (AT > 25.5 && AT < 30)
            {
                ATbgCol = "badge-warning";
            }
            else if (AT > 30)
            {
                ATbgCol = "badge-danger";
            }
            else if (AT <= 25.5 && AT > 21)
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

    }
}
