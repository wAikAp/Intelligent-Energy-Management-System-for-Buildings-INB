﻿using System;
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
				//Debug.WriteLine("Count: " + element);
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
					//Debug.WriteLine("room Id: "+ element.roomId + " recoded_time: "+ element.recorded_time + " power use : " + element.power_used);
				}
			}
			else
			{
				//Debug.WriteLine("not find");
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
			int i = 0; //counter 
			foreach (MongoDevicesPowerUse powerUse in PowerUsesList)
			{
				var tempRecord = powerUse;
				var tempRecordDate = tempRecord.recorded_time.ToString("yyyy-MM-dd");
				int y = 0;
				Boolean isChecked = false;
				foreach (DailyUsageModel outputlist in datelist)
				{
					if (outputlist.recorded_date == tempRecordDate && outputlist.roomId == tempRecord.roomId)
					{
						isChecked = true;
						break;
					}
				}
				if(isChecked != true)
				{
					foreach (MongoDevicesPowerUse sencondCheckPowerUse in PowerUsesList)
					{
						var sencondCheckRecordDate = sencondCheckPowerUse.recorded_time.ToString("yyyy-MM-dd");
						//Debug.WriteLine("Comparing tempRecordDate " + tempRecordDate + " -- " + "sencondCheckRecordDate " + sencondCheckRecordDate);
						//Debug.WriteLine("And tempRecord.roomId " + tempRecord.roomId + " -- " + "sencondCheckPowerUse.roomId " + sencondCheckPowerUse.roomId);
						//Debug.WriteLine("y " + y + " -- " + "i " + i);
						if (tempRecordDate == sencondCheckRecordDate && tempRecord.roomId == sencondCheckPowerUse.roomId && y > i && isChecked != true)
						{
							//Debug.WriteLine("ADDD ");
							//Debug.WriteLine("tempRecord.power_used " + tempRecord.power_used + "sencondCheckPowerUse.power_used " + sencondCheckPowerUse.power_used);
							tempRecord.power_used += sencondCheckPowerUse.power_used;
							//Debug.WriteLine("Sum =  " + tempRecord.power_used);
						}
						y++;
					}
					var newDateRecord = new DailyUsageModel();
					newDateRecord.recorded_date = tempRecordDate;
					newDateRecord.roomId = tempRecord.roomId;
					newDateRecord.power_used = tempRecord.power_used;

					//Debug.WriteLine("datelist " + newDateRecord.recorded_date);
					//Debug.WriteLine("Room Id " + newDateRecord.roomId);
					//Debug.WriteLine("Power consumtion " + newDateRecord.power_used + "kWh");
					datelist.Add(newDateRecord);
				}
				
				i++;
			}
			/*i = 0;
			foreach (DailyUsageModel newDateRecord in datelist)
			{
				Debug.WriteLine("");
				Debug.WriteLine("\\\\\\\\\\\\\\\\\\\\ ");
				Debug.WriteLine("datelist "+i+" " + newDateRecord.recorded_date);
				Debug.WriteLine("Room Id " + i + " " + newDateRecord.roomId);
				Debug.WriteLine("Power consumtion " + i + " " + newDateRecord.power_used +"kWh");
				Debug.WriteLine("\\\\\\\\\\\\\\\\\\\\ " );
				Debug.WriteLine("");
				i++;
			}*/


			return datelist;
		}

		public double getACPowerUse()//get the total power usage of the AC in the current month.
		{
			PowerUsesList = getPowerUseList();
			double monthlyAcUse = 0; //kWh
			DateTime localDate = DateTime.Now;
			String currentMonthly = localDate.ToString("yyyy-MM");


			foreach (MongoDevicesPowerUse powerUse in PowerUsesList)
			{
				var date = powerUse.recorded_time.ToString("yyyy-MM");
				if (currentMonthly == date)
				{
					//Debug.WriteLine("Is the current month record" + powerUse.recorded_time);
					if (powerUse.devicesId.Contains("AC"))
					{
						Debug.WriteLine("This record is AC" + powerUse.devicesId+" " + powerUse.recorded_time);
						monthlyAcUse += powerUse.power_used;
					}
				}
				
			}
			Debug.WriteLine("This month AC usage = " +monthlyAcUse +"(kWh)");
			return monthlyAcUse;
		}

		public double getLPowerUse()//get the total power usage of the light in the current month.
		{
			PowerUsesList = getPowerUseList();
			double monthlyAcUse = 0; //kWh
			DateTime localDate = DateTime.Now;
			String currentMonthly = localDate.ToString("yyyy-MM");


			foreach (MongoDevicesPowerUse powerUse in PowerUsesList)
			{
				var date = powerUse.recorded_time.ToString("yyyy-MM");
				if (currentMonthly == date)
				{
					//Debug.WriteLine("Is the current month record" + powerUse.recorded_time);
					if (powerUse.devicesId.Contains("LT"))
					{
						Debug.WriteLine("This record is Light" + powerUse.devicesId + " " + powerUse.recorded_time);
						monthlyAcUse += powerUse.power_used;
					}
				}

			}
			Debug.WriteLine("This month Light usage = " + monthlyAcUse + "(kWh)");
			return monthlyAcUse;
		}

		public double getHUMPowerUse()//get the total power usage of the hum in the current month.
		{
			PowerUsesList = getPowerUseList();
			double monthlyAcUse = 0; //kWh
			DateTime localDate = DateTime.Now;
			String currentMonthly = localDate.ToString("yyyy-MM");


			foreach (MongoDevicesPowerUse powerUse in PowerUsesList)
			{
				var date = powerUse.recorded_time.ToString("yyyy-MM");
				if (currentMonthly == date)
				{
					//Debug.WriteLine("Is the current month record" + powerUse.recorded_time);
					if (powerUse.devicesId.Contains("HD"))
					{
						Debug.WriteLine("This record is Hum" + powerUse.devicesId + " " + powerUse.recorded_time);
						monthlyAcUse += powerUse.power_used;
					}
				}

			}
			Debug.WriteLine("This month Hum usage = " + monthlyAcUse + "(kWh)");
			return monthlyAcUse;
		}

		public double getEXHFPowerUse()//get the total power usage of the exhf in the current month.
		{
			PowerUsesList = getPowerUseList();
			double monthlyAcUse = 0; //kWh
			DateTime localDate = DateTime.Now;
			String currentMonthly = localDate.ToString("yyyy-MM");


			foreach (MongoDevicesPowerUse powerUse in PowerUsesList)
			{
				var date = powerUse.recorded_time.ToString("yyyy-MM");
				if (currentMonthly == date)
				{
					//Debug.WriteLine("Is the current month record" + powerUse.recorded_time);
					if (powerUse.devicesId.Contains("EF"))
					{
						Debug.WriteLine("This record is EXHF" + powerUse.devicesId + " " + powerUse.recorded_time);
						monthlyAcUse += powerUse.power_used;
					}
				}

			}
			Debug.WriteLine("This month Exhf usage = " + monthlyAcUse + "(kWh)");
			return monthlyAcUse;
		}

		public double getTotalPowerUse()//get the total power usagein the current month
		{
			double monthlyUse = getACPowerUse() + getEXHFPowerUse() + getHUMPowerUse() + getLPowerUse();
			return monthlyUse;
		}


		//
		//
		// for Wai get spec devices data
		public double getSpecApplianceMonthlyPowerUse(String devicesId)// get Specific Appliance Monthly Power Use
		{
			PowerUsesList = getPowerUseList();
			double monthlyUse = 0; //kWh
			DateTime localDate = DateTime.Now;
			String currentMonthly = localDate.ToString("yyyy-MM");

			foreach (MongoDevicesPowerUse powerUse in PowerUsesList)
			{
				var date = powerUse.recorded_time.ToString("yyyy-MM");
				if (currentMonthly == date)
				{
					if (powerUse.devicesId == devicesId)
					{
						Debug.WriteLine("This record is " + powerUse.devicesId + " " + powerUse.recorded_time +" power used : "+ powerUse.power_used);
						monthlyUse += powerUse.power_used;
					}
				}

			}
			Debug.WriteLine("This month Id: " +devicesId+ " monthly Use is "+ monthlyUse + "(kWh)");	
			return Math.Round(monthlyUse, 2);
		}

		public double getSpecApplianceTurnOnPowerUse(String devicesId) //get switched on devices current power using
		{
			List<DevicesListModel> devicesListModel = new List<DevicesListModel>();
			DateTime currentTime = DateTime.Now;
			double devicePower = 0;
			double usedTime = 0;
			double powerUse;
			

			//fetch collection data
			var DEVICES_LISTCollection = dbManager.DataBase.GetCollection<BsonDocument>("DEVICES_LIST");
			var devices_listDocs = DEVICES_LISTCollection.Find(new BsonDocument()).ToList();
			foreach (BsonDocument bsonElements in devices_listDocs)
			{
				devicesListModel.Add(BsonSerializer.Deserialize<DevicesListModel>(bsonElements));
			}
			
			foreach(DevicesListModel device in devicesListModel)
			{
				if(device.devicesId == devicesId && device.status == true)
				{
					devicePower = device.power;
					usedTime = new TimeSpan(currentTime.Ticks - device.turn_on_time.Ticks).TotalSeconds;
					Debug.WriteLine("Used time = :" + usedTime);
				}
			}
			powerUse = Math.Round((usedTime * 0.000277777778 )* devicePower,2);
			Debug.WriteLine("powerUse = :" + powerUse);
			return powerUse;
		}

		public double getSpecApplianceMonthlyPowerUseTime(String devicesId)// get Specific Appliance Monthly Power Use time
		{
			PowerUsesList = getPowerUseList();
			double monthlyUse = 0; //kWh
			DateTime localDate = DateTime.Now;
			String currentMonthly = localDate.ToString("yyyy-MM");

			foreach (MongoDevicesPowerUse powerUse in PowerUsesList)
			{
				var date = powerUse.recorded_time.ToString("yyyy-MM");
				if (currentMonthly == date)
				{
					if (powerUse.devicesId == devicesId)
					{
						Debug.WriteLine("This record is " + powerUse.devicesId + " " + powerUse.recorded_time + " power used : " + powerUse.power_used);
						monthlyUse += powerUse.recorded_used_time;
					}
				}

			}
			Debug.WriteLine("This month Id: " + devicesId + " monthly Use is " + monthlyUse + "(seconds)");
			return monthlyUse;
		}

		public double getSpecApplianceTurnOnPowerUseTime(String devicesId) //get switched on devices current turn on use time
		{
			List<DevicesListModel> devicesListModel = new List<DevicesListModel>();
			DateTime currentTime = DateTime.Now;
			double devicePower = 0;
			double usedTime = 0;


			//fetch collection data
			var DEVICES_LISTCollection = dbManager.DataBase.GetCollection<BsonDocument>("DEVICES_LIST");
			var devices_listDocs = DEVICES_LISTCollection.Find(new BsonDocument()).ToList();
			foreach (BsonDocument bsonElements in devices_listDocs)
			{
				devicesListModel.Add(BsonSerializer.Deserialize<DevicesListModel>(bsonElements));
			}

			foreach (DevicesListModel device in devicesListModel)
			{
				if (device.devicesId == devicesId && device.status == true)
				{
					devicePower = device.power;
					usedTime = new TimeSpan(currentTime.Ticks - device.turn_on_time.Ticks).TotalSeconds;
					Debug.WriteLine("Used time = :" + usedTime);
				}
			}
			usedTime = Math.Round((usedTime * 0.000277777778), 2);
			return usedTime;
		}

		//
		//
		//
		//
		//
		//
		//get power use time (second not minute).

		public double getACPowerUseTime()//get the total power usage time of the AC in the current month.
		{
			PowerUsesList = getPowerUseList();
			double monthlyAcUseTime = 0; //kWh
			DateTime localDate = DateTime.Now;
			String currentMonthly = localDate.ToString("yyyy-MM");


			foreach (MongoDevicesPowerUse powerUse in PowerUsesList)
			{
				var date = powerUse.recorded_time.ToString("yyyy-MM");
				if (currentMonthly == date)
				{
					//Debug.WriteLine("Is the current month record" + powerUse.recorded_time);
					if (powerUse.devicesId.Contains("AC"))
					{
						Debug.WriteLine("This record is AC" + powerUse.devicesId + " " + powerUse.recorded_time);
						monthlyAcUseTime += powerUse.recorded_used_time;
					}
				}

			}
			Debug.WriteLine("This month AC usage = " + monthlyAcUseTime + "(kWh)");
			return monthlyAcUseTime;
		}

		public double getLPowerUseTime()//get the total power usage time of the light in the current month.
		{
			PowerUsesList = getPowerUseList();
			double monthlyLUseTime = 0; //kWh
			DateTime localDate = DateTime.Now;
			String currentMonthly = localDate.ToString("yyyy-MM");


			foreach (MongoDevicesPowerUse powerUse in PowerUsesList)
			{
				var date = powerUse.recorded_time.ToString("yyyy-MM");
				if (currentMonthly == date)
				{
					//Debug.WriteLine("Is the current month record" + powerUse.recorded_time);
					if (powerUse.devicesId.Contains("LT"))
					{
						Debug.WriteLine("This record is light" + powerUse.devicesId + " " + powerUse.recorded_time);
						monthlyLUseTime += powerUse.recorded_used_time;
					}
				}

			}
			Debug.WriteLine("This month AC usage = " + monthlyLUseTime + "(kWh)");
			return monthlyLUseTime;
		}

		public double getHUMPowerUseTime()//get the total power usage time of the hum in the current month.
		{
			PowerUsesList = getPowerUseList();
			double monthlyHumUseTime = 0; //kWh
			DateTime localDate = DateTime.Now;
			String currentMonthly = localDate.ToString("yyyy-MM");


			foreach (MongoDevicesPowerUse powerUse in PowerUsesList)
			{
				var date = powerUse.recorded_time.ToString("yyyy-MM");
				if (currentMonthly == date)
				{
					//Debug.WriteLine("Is the current month record" + powerUse.recorded_time);
					if (powerUse.devicesId.Contains("HD"))
					{
						Debug.WriteLine("This record is hum" + powerUse.devicesId + " " + powerUse.recorded_time);
						monthlyHumUseTime += powerUse.recorded_used_time;
					}
				}

			}
			Debug.WriteLine("This month hum usage = " + monthlyHumUseTime + "(kWh)");
			return monthlyHumUseTime;
		}

		public double getEXHFPowerUseTime()//get the total power usage time of the exhf in the current month.
		{
			PowerUsesList = getPowerUseList();
			double monthlyExhfUseTime = 0; //kWh
			DateTime localDate = DateTime.Now;
			String currentMonthly = localDate.ToString("yyyy-MM");


			foreach (MongoDevicesPowerUse powerUse in PowerUsesList)
			{
				var date = powerUse.recorded_time.ToString("yyyy-MM");
				if (currentMonthly == date)
				{
					//Debug.WriteLine("Is the current month record" + powerUse.recorded_time);
					if (powerUse.devicesId.Contains("EF"))
					{
						Debug.WriteLine("This record is exhf" + powerUse.devicesId + " " + powerUse.recorded_time);
						monthlyExhfUseTime += powerUse.recorded_used_time;
					}
				}

			}
			Debug.WriteLine("This month exhf usage = " + monthlyExhfUseTime + "(kWh)");
			return monthlyExhfUseTime;
		}

		public double getTotalPowerUseTime()//get the total power usagein the current month
		{
			double monthlyUse = getACPowerUseTime() + getLPowerUseTime() + getHUMPowerUseTime() + getEXHFPowerUseTime();
			return monthlyUse;
		}

		public JObject getRoomPowerTrend()// gen a json for the chart.
		{
			List<String> RoomList = getRoomList();
			PowerUsesList = getPowerUseList();


			String oString = "{ labels: [";
			String temp = "";
			for (int i = 6; i > 0; i--)//gen x line tags
			{
				DateTime dt = DateTime.Now;
				DateTime currentHourTime = dt.AddHours(-i);
				if (currentHourTime.ToString("HH").Contains("24"))
				{
					temp += "'00:00' , ";
				}
				else
				{
					temp += "'" + currentHourTime.ToString("HH") + ":00' , ";
				}				
			}
			oString += temp;
			oString += "], datasets:[ ";
			temp = "";
			foreach (String room in RoomList)
			{
				temp += "{ label: '" + room + "',backgroundColor: '" + GetRandomColor() + "' ,borderColor: '" + GetRandomColor() + "', data:[";
				// data insert
				String tempdata = ""; // for store temp data 
				for (int i = 6; i > 0; i--)
				{
					String currentHourTime = DateTime.Now.AddHours(-i).ToString("yyyy/MM/dd:HH"); //get current time
					String fetchdata = "";
					foreach (MongoDevicesPowerUse powerUse in PowerUsesList)
					{
						//Debug.WriteLine("Comparing ctime: " + currentHourTime +" and fetch time: "+ powerUse.recorded_time.ToString("yyyy/MM/dd:HH"));
						//Debug.WriteLine("Comparing room: " + room + " and powerUse.roomId: " + powerUse.roomId);
						if (currentHourTime.Contains(powerUse.recorded_time.ToString("yyyy/MM/dd:HH")) && room == powerUse.roomId)
						{
							//Debug.WriteLine("one passed record");
							fetchdata ="'"+powerUse.power_used+"'";
						}
					}
					if (fetchdata == "")
					{
						tempdata += "''";
					}
					else
					{
						tempdata += fetchdata;
					}
					if(i!= 1)
					{
						tempdata += ",";
					}
				}
				temp += tempdata;
				temp += "],fill: false}";
				if (RoomList.IndexOf(room) != RoomList.Count - 1)
				{
					temp += ",";
				}
			}
			oString += temp;
			oString += "]}";
			Debug.WriteLine("oString = " + oString);
			JObject json = JObject.Parse(oString);

			return json;
		}

		public string GetRandomColor()
		{
			var random = new Random();
			var rmcolor = String.Format("#{0:X6}", random.Next(0x1000000));
			return rmcolor;
		}
	}

}
