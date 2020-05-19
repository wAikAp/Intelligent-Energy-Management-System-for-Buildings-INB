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

namespace FYP_WEB_APP.Models.LogicModels
{
	public class DevicesPowerUseOutputUtil
	{
		DBManger dbManager;
		List<MongoDevicesPowerUse> PowerUsesList;
		int count;
		List<String> roomList;
		


		public DevicesPowerUseOutputUtil()
		{
			dbManager = new DBManger();
			var collection = dbManager.DataBase.GetCollection<MongoDevicesPowerUse>("DEVICES_POWER_USE");
			roomList = collection.AsQueryable<MongoDevicesPowerUse>().Select(c => c.roomId).Distinct().ToList();
			foreach (var element in roomList)
			{
				Debug.WriteLine("Count: " + element);
				count++;
			}
		}

		public List<MongoDevicesPowerUse> getPowerUseList() //Get all record without selection
		{
			var collection = dbManager.DataBase.GetCollection<MongoDevicesPowerUse>("DEVICES_POWER_USE");
			PowerUsesList = collection.Find(new BsonDocument()).ToList();
			if (PowerUsesList != null)
			{
				foreach(MongoDevicesPowerUse element in PowerUsesList)
				{
					Debug.WriteLine("room Id: "+ element.roomId + " recoded_time: "+ element.recorded_time + " power use : " + element.power_used);
				}
			}
			else
			{
				Debug.WriteLine("not find");
			}
			return PowerUsesList;
		}

		public int getCountOfDifferentRooms() // How many different room is.
		{
			return count;
		}

		public List<String> getRoomList() // different room name.
		{
			return roomList;
		}


		public List<DailyUsageModel> Dailyusage() // get the total power usage of the room !!!!!
		{
			PowerUsesList = getPowerUseList();
			List<DailyUsageModel> datelist = new List<DailyUsageModel>();  //  a list store only two attribute the day (recorded_date) and (power_used ) total power consum in that day. 
			foreach (MongoDevicesPowerUse powerUse in PowerUsesList)
			{
				Boolean isNewRecord = true;
				
				var date = powerUse.recorded_time.ToString("yyyy-MM-dd");

				Debug.WriteLine("date: " + date);
				foreach(DailyUsageModel datesUsage in datelist)
				{
					if(datesUsage.recorded_date == date && datesUsage.roomId == powerUse.roomId)
					{
						datesUsage.power_used += powerUse.power_used;
						isNewRecord = false;
					}
					else if(datesUsage.recorded_date == date && datesUsage.roomId != powerUse.roomId)
					{
						isNewRecord = true;
					}
					else if(datesUsage.recorded_date != date && datesUsage.roomId == powerUse.roomId)
					{
						isNewRecord = true;
					}
					else
					{
						isNewRecord = true;
					}
				}

				if (isNewRecord == true)
				{
					Debug.WriteLine("isNewRecord" + date);
					Debug.WriteLine("isNewRecord roomID: " + powerUse.roomId);
					var newDateRecord = new DailyUsageModel();
					newDateRecord.recorded_date = date;
					newDateRecord.roomId = powerUse.roomId;
					newDateRecord.power_used = powerUse.power_used;
					datelist.Add(newDateRecord);
				}

			}
			if(datelist != null)
			{
				datelist.RemoveAt(datelist.Count-1);
			}
			int i = 0;
			foreach (DailyUsageModel newDateRecord in datelist)
			{
				Debug.WriteLine("************************** ");
				Debug.WriteLine("datelist "+i+" " + newDateRecord.recorded_date);
				Debug.WriteLine("Room Id " + i + " " + newDateRecord.roomId);
				Debug.WriteLine("Power consumtion " + i + " " + newDateRecord.power_used +"kWh");
				Debug.WriteLine("\\\\\\\\\\\\\\\\\\\\ " );
				i++;
			}


			return datelist;
		}

		/*public List<DailyRoomUsageModel> DailyRoomusage() //get the total power usage of a room !!!!
		{

		}*/
	}
	
}
