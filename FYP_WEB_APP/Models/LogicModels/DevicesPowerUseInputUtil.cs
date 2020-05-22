using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP_APP.Models.MongoModels;
using FYP_WEB_APP.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics;
using FYP_WEB_APP.Models.MongoModels;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using MongoDB.Bson.Serialization;

namespace FYP_WEB_APP.Models.LogicModels
{
	public class DevicesPowerUseInputUtil
	{
		DBManger dbManager;


		public DevicesPowerUseInputUtil()
		{
			dbManager = new DBManger();
		}

		public Boolean insertDevicesPowerUse(String devicesId)// requested devicesId //if insert successfully, return true;
		{
			try
			{
				var DEVICES_LISTcollection = dbManager.DataBase.GetCollection<BsonDocument>("DEVICES_LIST");
				var filter = Builders<BsonDocument>.Filter.Eq("devicesId", devicesId);
				var firstDocument = DEVICES_LISTcollection.Find(filter).FirstOrDefault();
				var device = BsonSerializer.Deserialize<MongoDevicesListModel>(firstDocument);
				var DEVICES_POWER_USEcollection = dbManager.DataBase.GetCollection<BsonDocument>("DEVICES_POWER_USE");

				DateTime turnOnTime = device.turn_on_time;
				DateTime currentTime = DateTime.Now;

				int usedTime = Convert.ToInt32(currentTime.Subtract(turnOnTime).TotalSeconds);

				Debug.WriteLine("turnOnTime " + turnOnTime);
				Debug.WriteLine("currentTime " + currentTime);
				Debug.WriteLine("usedTime " + usedTime);

				MongoDevicesPowerUse powerUseRecord = new MongoDevicesPowerUse();
				powerUseRecord.devicesId = device.devicesId;
				powerUseRecord.roomId = device.roomId;
				powerUseRecord.recorded_time = DateTime.Now;
				powerUseRecord.recorded_used_time = usedTime;
				powerUseRecord.power_used = Math.Round((device.power * (usedTime * 0.000277777778)), 2); //to kWh

				Debug.WriteLine("powerUseRecord " + powerUseRecord.ToBsonDocument());

				DEVICES_POWER_USEcollection.InsertOne(powerUseRecord.ToBsonDocument());

				return true;
			}
			catch (Exception e)
			{
				Debug.WriteLine("Exception :" + e);
				return false;
			}

		}

	}
}
