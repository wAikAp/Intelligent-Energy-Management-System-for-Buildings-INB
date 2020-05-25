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
using Microsoft.AspNetCore.Routing.Patterns;

namespace FYP_WEB_APP.Models.LogicModels
{
	public class DevicesPowerUseInputUtil
	{
		DBManger dbManager;
		String currentDate;


		public DevicesPowerUseInputUtil()
		{
			dbManager = new DBManger();
			currentDate = DateTime.Now.ToString("yyyy-MM-dd");
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
				powerUseRecord.recorded_time = DateTime.UtcNow.AddHours(8);
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

		public Boolean updateRoomPower() //if update success return true;
		{
			List<MongoRoomModel> roomModels = new List<MongoRoomModel>();
			List<DevicesListModel> devicesListModel = new List<DevicesListModel>();
			double RoomPower = 0;
			try
			{
				Debug.WriteLine("////////////////////////////");
				//Fetch daily use data
				DevicesPowerUseOutputUtil devicesPowerUseOutputUtil = new DevicesPowerUseOutputUtil();
				List<DailyUsageModel> dailyusage = devicesPowerUseOutputUtil.Dailyusage();

				//Fetch room data
				var ROOMcollection = dbManager.DataBase.GetCollection<BsonDocument>("ROOM");
				var tempDoc = ROOMcollection.Find(new BsonDocument()).ToList();
				foreach (BsonDocument bsonElements in tempDoc)
				{
					roomModels.Add(BsonSerializer.Deserialize<MongoRoomModel>(bsonElements));
				}

				//Fetch devices list data
				var DEVICES_LISTCollection = dbManager.DataBase.GetCollection<BsonDocument>("DEVICES_LIST");
				var devices_listDocs = DEVICES_LISTCollection.Find(new BsonDocument()).ToList();
				foreach(BsonDocument bsonElements in devices_listDocs)
				{
					devicesListModel.Add(BsonSerializer.Deserialize<DevicesListModel>(bsonElements));
				}

				foreach (MongoRoomModel room in roomModels)
				{
					RoomPower = 0;
					//Debug.WriteLine("Now checking Room id = :" + room.roomId);
					foreach(DailyUsageModel dailyUsageModel in dailyusage)
					{
						if(room.roomId == dailyUsageModel.roomId && currentDate == dailyUsageModel.recorded_date)
						{
							//Debug.WriteLine("dailyUsageModel id =  "+ dailyUsageModel.roomId + " room.roomId " + room.roomId);
							//Debug.WriteLine("There are some record in dailyUsageModel");
							//Debug.WriteLine("old record " + room.power);
							room.power = dailyUsageModel.power_used;
							//Debug.WriteLine("new update : " + room.power);

							foreach(DevicesListModel deviceRecord in devicesListModel)
							{
								if(deviceRecord.roomId == room.roomId && deviceRecord.status == true )
								{
									//Debug.WriteLine("There are some record in turned on and no add to dailyUsageModel");
									//Debug.WriteLine("Device Id: "+ deviceRecord.devicesId +" now used " + devicesPowerUseOutputUtil.getSpecApplianceTurnOnPowerUse(deviceRecord.devicesId)+"(kwh)");
									RoomPower += devicesPowerUseOutputUtil.getSpecApplianceTurnOnPowerUse(deviceRecord.devicesId);
								} 
							}
							//Debug.WriteLine("turn on power using In Room Id "+room.roomId+ " power using :" + RoomPower + "(kwh)");
						}
						else
						{
							room.power = 0;
						}
					}

					room.power = room.power + RoomPower;
					//Debug.WriteLine("Total Room Power : " + room.power);
					//Debug.WriteLine("////////////////////////////");

					var filter = Builders<BsonDocument>.Filter.Eq("roomId", room.roomId);
					var update = Builders<BsonDocument>.Update.Set("power", room.power);
					ROOMcollection.UpdateOne(filter, update);
				}
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
