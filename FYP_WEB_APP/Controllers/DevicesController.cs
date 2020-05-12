using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using FYP_APP.Models.MongoModels;
using FYP_WEB_APP.Controllers.Mongodb;
using FYP_WEB_APP.Models.MongoModels;
using FYP_WEB_APP.Models;
using System;
using System.Diagnostics;

namespace FYP_APP.Controllers
{
	public class DevicesController : Controller
	{
		public List<DevicesListModel> MongoDevicesList = new List<DevicesListModel> { };
		public IMongoCollection<DevicesListModel> getMDLMconn()
		{
			ConnectDB conn = new ConnectDB();
			var database = conn.Conn();
			var collection = database.GetCollection<DevicesListModel>("DEVICES_LIST");
			return collection;
		}
		[Route("Devices/Devices")]
		public IActionResult Devices()
		{
			ViewBag.sortIMG = "sort.png";

			//Request.Query["sortOrder"] check 
			if (Request.Query.ContainsKey("sortOrder"))
			{
				this.MongoDevicesList = getDevicesbyid();
				ViewData["MongoDevicesListModel"] = this.MongoDevicesList;
				ViewData["RoomListModel"] = GetRoomData();


			}
			else if (!Request.Query.ContainsKey("sortOrder") && Request.Query.Keys.Count() > 0)
			{
				SearchDevices();
			}
			else
			{

				this.MongoDevicesList = getAllDevices();
				ViewBag.roomIdSortParm = "?sortOrder=roomId";
				ViewData["MongoDevicesListModel"] = this.MongoDevicesList;
				ViewData["RoomListModel"] = GetRoomData();


			}


			return View();
		}
		[Route("Devices/Devices/{id}")]
		public IActionResult Devices(string id) {
			ViewData["MongoDevicesListModel"] = getAllDevices().Where(x=>x.roomId==id).ToList();
			ViewData["RoomListModel"] = GetRoomData();

			return View();
		}

		
		[Route("Devices/DevicesChartByRoomid")]
		public ActionResult DevicesChartByRoomid()
		{
			/*string id = "";
			id = Request.Query["roomID"];
			//  $("#sensorChartHS").load("@Url.Action("SensorsChartByRoomid", "Sensors", new { roomID = ViewData["roomID"], title = "Humidity Sensor Log Record", chartType = "line", position = "top", sensorType = "HS" })", function () {});
			//$("#sensorChartLS").load("@Url.Action("SensorsChartByRoomid", "Sensors", new { roomID = ViewData["roomID"], title = "Luminosity Sensor Log Record", chartType = "line", position = "top", sensorType = "LS" })", function () {});

			getdb();
			List<SensorsListModel> lists = GetSensorsData().Where(x => x.roomId.Contains(id)).ToList();
			Debug.WriteLine("list: " + lists.ToJson().ToString());
			Debug.WriteLine("list: " + Request.QueryString);
			Debug.WriteLine("roomID: " + Request.Query["roomID"]);
			Debug.WriteLine("title: " + Request.Query["title"]);
			Debug.WriteLine("chartType: " + Request.Query["chartType"]);
			Debug.WriteLine("position: " + Request.Query["position"]);
			Debug.WriteLine("sensorType: " + Request.Query["sensorType"]);
			//Debug.WriteLine(Setgroup(lists).ToJson().ToString());
			ViewBag.charttitle = Request.Query["title"];
			ViewBag.chartType = Request.Query["chartType"];
			ViewBag.position = Request.Query["position"];
			ViewBag.download = Request.Query["download"];

			ViewBag.day = getChartTime();
			ViewBag.datasets = chartData(lists, Request.Query["sensorType"]);
			ViewBag.divId = getRandomDivId();
			*/
			return PartialView("_DevicesChart");
		}
		[Route("Devices/SearchDevicesByRoomid")]
		public IActionResult SearchDevicesByRoomid()
		{
			string id = Request.Query["roomID"];

			ViewData["MongoDevicesListModel"] = getAllDevices().Where(x => x.roomId == id).ToList();
			ViewData["RoomListModel"] = GetRoomData();
			//getAllDevices().Where(x => x.roomId == id).ToList().ToJson().ToString()
			return PartialView("_DevicesList");

		}
		public void SearchDevices()
		{

			ViewData["Title"] = "Search Devices";
			var list = getAllDevices();
			List<DevicesListModel> sum = new List<DevicesListModel> { };

			List<DevicesListModel> get=new List<DevicesListModel> { };
			if (Request.Query.ContainsKey("roomId")) {
				/*			List<MongoDevicesListModel> list = getAllDevices().Where(x => x.roomId == id).ToList();
			Debug.WriteLine(list);
			ViewData["MongoDevicesListModel"] = list;
			ViewData["RoomListModel"] = GetRoomData();
*/
				if (Request.Query["roomId"] != string.Empty || Request.Query["roomId"] != "")
				{ 
					Debug.WriteLine("\n Request ContainsKey roomId");
				list = list.Where(ls => ls.roomId.Contains(Request.Query["roomId"])).ToList();
				Debug.WriteLine(list.ToJson().ToString());
				}
			}
			foreach (String key in Request.Query.Keys)
			{
				Debug.WriteLine("in foreach ....");

				string skey = key;
				string keyValue = Request.Query[key];


				switch (skey) {					
					case "AC":
					case "LT":
					case "HD":
					case "EF":
						get = list.Where(ls => ls.devicesId.Contains(skey) ).ToList();
						break;
				}
				if (key != "roomId") {
				sum.AddRange(get);
				sum = sum.Distinct().ToList();//delet double data
					Debug.WriteLine("befor Distinct \n" + sum.ToJson().ToString());

				}

			}
			if (Request.Query.Keys.Count() !=1 || Request.Query["roomId"] != "")
			{
				list = list.Intersect(sum).ToList();
				Debug.WriteLine("befor Intersect \n" + list.ToJson().ToString());
			}
			


			ViewData["MongoDevicesListModel"] = list;

			ViewData["RoomListModel"] = GetRoomData();
		}
		[Route("Devices/AddDevices")]
		public ActionResult AddDevices()//display add sensors form
		{
			ViewBag.viewType = "Add";
			ViewData["RoomListModel"] = GetRoomData();
			ViewBag.action = "AddDevicesData";

			return PartialView("_AddDevices");
		}
		[Route("Devices/EditDevices/{id}")]
		public ActionResult EditDevices(string id)
		{
			ViewData["EditDevicesListModel"] = getAllDevices().Where(t => t.devicesId == id).ToList();

			ViewBag.viewType = "Edit";
			ViewBag.action = "UpdateDevicesData";

			return PartialView("_AddDevices");
		}
		[Route("Devices/DropDevices/{id}")]
		public ActionResult DropDevices(string id)//display Drop sensors form
		{
			ViewData["EditDevicesListModel"] = getAllDevices().Where(t => t.devicesId == id).ToList();

			ViewBag.viewType = "Drop";
			ViewBag.action = "DropDevicesData";

			return PartialView("_AddDevices");
		}
		[Route("Devices/AddDevicesData")]
		[HttpPost]
		public ActionResult AddDevicesData(MongoDevicesListModel postData)//post
		{
			DevicesListModel insertList = new DevicesListModel { };

			var all = getAllDevices();
			string count = "";
			if (all.Count < 10) { count = "00" + (all.Count+1).ToString(); }
			else if (all.Count < 100) { count = "0" + (all.Count + 1).ToString(); }
			DateTime nowData = new DateTime();

			//sensortype = postData.Sensortype;
			insertList.roomId = postData.roomId;
			insertList.devicesId = postData.devicesId + count;
			insertList.devices_Name = postData.devices_Name;
			insertList.location = postData.location;
			insertList.desc = postData.desc;
			insertList.lastest_checking_time = DateTime.UtcNow;
			insertList.total_run_time = nowData;
			insertList.power = 0;

			getMDLMconn().InsertOneAsync(insertList);
			return RedirectToAction("Devices");
		}
		[Route("Devices/DropDevicesData")]
		[HttpPost]
		public ActionResult DropDevicesData(MongoDevicesListModel postData)//post
		{
			var DeleteResult = getMDLMconn().DeleteOne(de => de.devicesId==postData.devicesId & de.roomId == postData.roomId);

			return RedirectToAction("Devices");
		}
		[Route("Devices/UpdateDevicesData")]
		[HttpPost]
		public ActionResult UpdateDevicesData(MongoDevicesListModel postData)
		{
			var filter = Builders<DevicesListModel>.Filter.Eq("devicesId", postData.devicesId);
			filter = filter & Builders<DevicesListModel>.Filter.Eq("roomId", postData.roomId);
			var type = postData.GetType();
			var props = type.GetProperties();
			foreach (var property in props)
			{
				if (!property.Name.Equals("_id"))
				{
					if (property.GetValue(postData) != null)
					{
						UpdateDefinition<DevicesListModel> up;
						if (property.Name == "latest_checking_time")
						{
							up = Builders<DevicesListModel>.Update.Set(property.Name.ToString(), DateTime.UtcNow);
						}
						else
						{
							up = Builders<DevicesListModel>.Update.Set(property.Name.ToString(), property.GetValue(postData).ToString());

						}
						var Updated = getMDLMconn().UpdateOne(filter, up);
						//this.isUpdated = Updated.IsAcknowledged;
					}
				}
			}
			return RedirectToAction("Devices");
		}


		public List<DevicesListModel> getAllDevices() {			
			var collection = getMDLMconn();
			IQueryable<DevicesListModel> query = from d in collection.AsQueryable<DevicesListModel>() select d;
			List<DevicesListModel> list=new List<DevicesListModel>();
			foreach (var get in query.ToList()) {

				DateTime nowData = DateTime.Now;
				DateTime runtime = get.total_run_time;
				TimeSpan count = new TimeSpan(nowData.Ticks - runtime.Ticks);

				double currentValue = getCurrentValue(get.devicesId);
				double avgPowers = get.power/ count.TotalDays;
				avgPowers = Convert.ToDouble(avgPowers.ToString("0.00"));

				var data = new DevicesListModel()
				{
					_id = get._id,
					listId = get.listId,
					roomId = get.roomId,
					devicesId = get.devicesId,
					devices_Name= get.devices_Name,
					power= get.power,
					lastest_checking_time = get.lastest_checking_time,
					total_run_time = get.total_run_time,
					location= get.location,
					desc = get.desc,
					current= currentValue,
					powerOnOff = false,
					avgPower= avgPowers
				};
				Debug.WriteLine(data.current);
				list.Add(data);
			}

			MongoDevicesList = query.ToList();
			return list.ToList();
		}
		public List<DevicesListModel> getDevicesbyid() {
			List<DevicesListModel> list = null;
			string sortOrder = Request.Query["sortOrder"];
			sortOrder=ChangeSortLink(sortOrder);

			if (Request.Query.ContainsKey("sortOrder")is false)
			{
				ViewBag.sortIMG = "sort.png";

			}
			else if (sortOrder.Contains("Desc"))
			{
				list = getAllDevices().OrderByDescending(item => item.roomId).ToList();
				//.Sort.Descending(sortOrder[0..^5]);
				ViewBag.sortIMG = "sort_desc.png";

			}
			else
			{

				ViewBag.sortIMG = "sort.png";

				list = getAllDevices().OrderBy(item => item.roomId).ToList();

			}
			return list;
		}
		public string ChangeSortLink(string sortOrder)
		{
			int count = Request.Query.Keys.Count;
			var rs = "";
			bool isfirst = true;
			foreach (String key in Request.Query.Keys)
			{
				string skey = key;
				string keyValue = Request.Query[key];
				if (!key.Equals("sortOrder"))
				{
					if (isfirst == true)
					{
						rs = skey + "=" + keyValue;
						isfirst = false;
					}
					else
					{
						rs += "&" + skey + "=" + keyValue;

					}
				}
			}
			/*
			string sort = null;
			switch (sortOrder) {
				case "devicesId":
					string viewsortOrderdevicesId = sortOrder == "devicesId" ? "devicesId_Desc" : "devicesId";

					ViewBag.devicesIdSortParm = "?sortOrder=" + viewsortOrderdevicesId + "&" + rs;
					if (viewsortOrderdevicesId.Equals("desc")) { sort = "sort_desc"; } else { sort = "sort"; }
					ViewBag.sortdevicesIdIMG = sort+".png";
					break;
				case "roomId":
					string viewsortOrderroomId = sortOrder == "roomId" ? "roomId_Desc" : "roomId";

					ViewBag.roomIdSortParm = "?sortOrder=" + viewsortOrderroomId + "&" + rs;
					if (viewsortOrderroomId.Equals("desc")) { sort = "sort_desc"; } else { sort = "sort"; }
					ViewBag.sortroomIdIMG = sort + ".png"; break;
				case "devices_Name":
					string viewsortOrderdevices_Name = sortOrder == "devices_Name" ? "devices_Name_Desc" : "devices_Name";

					ViewBag.devices_NameSortParm = "?sortOrder=" + viewsortOrderdevices_Name + "&" + rs;
					if (viewsortOrderdevices_Name.Equals("desc")) { sort = "sort_desc"; } else { sort = "sort"; }
					ViewBag.sortdevices_NameIMG = sort + ".png"; break;
				case "power":
					string viewsortOrderpower = sortOrder == "power" ? "power_Desc" : "power";

					ViewBag.powerSortParm = "?sortOrder=" + viewsortOrderpower + "&" + rs;
					if (viewsortOrderpower.Equals("desc")) { sort = "sort_desc"; } else { sort = "sort"; }
					ViewBag.sorpowerdIMG = sort + ".png";
					break;
				case "lastest_checking_time":
					string viewsortOrderlastest_checking_time = sortOrder == "lastest_checking_time" ? "roomId_Desc" : "lastest_checking_time";

					ViewBag.lastest_checking_timeSortParm = "?sortOrder=" + viewsortOrderlastest_checking_time + "&" + rs;
					if (viewsortOrderlastest_checking_time.Equals("desc")) { sort = "sort_desc"; } else { sort = "sort"; }
					ViewBag.sortlastest_checking_timeIMG = sort + ".png";
					break;
				case "total_run_time":

					break;
			}*/
			return sortOrder;
		}
		public List<RoomsListModel> GetRoomData()
		{
			ConnectDB conn = new ConnectDB();
			var database = conn.Conn(); 
			var RoomDataList = new List<RoomsListModel> { };
			IMongoCollection<RoomsListModel> collection = database.GetCollection<RoomsListModel>("ROOM");

			// sorting

			var sort = Builders<RoomsListModel>.Sort.Ascending("roomId");

			//end sorting

			var RoomsDocuments = collection.Find(new BsonDocument()).Sort(sort);

			foreach (RoomsListModel set in RoomsDocuments.ToList())
			{
				var data = new RoomsListModel()
				{
					roomId = set.roomId,
				};
				RoomDataList.Add(data);
			}

			return RoomDataList;
		}
		public double getCurrentValue(string Id)
		{
			ConnectDB conn = new ConnectDB();
			var database = conn.Conn();
			double value = 0;

			IMongoCollection<BsonDocument> collection;
			FilterDefinition<BsonDocument> filter;
			var dbStr = "";
			switch (Id.Substring(0, 2))
			{				
				case "EF":
					dbStr = "EXH_FAN";
					break;
				case "AC":
					dbStr = "AC";
					break;
				case "LT":
					dbStr = "LIGHT";
					break;
				case "HD":
					dbStr = "HUM";
					break;
				default:
					break;
			}
			collection = database.GetCollection<BsonDocument>(dbStr);
			filter = Builders<BsonDocument>.Filter.Eq("devicesId", Id);

			var json = collection.Find(filter).FirstOrDefault();
			if (json != null)
			{

				switch (Id.Substring(0, 2))
				{
					case "EF":
						value = Convert.ToDouble(json["current_rmp"].ToString());
						break;
					case "AC":
						value = Convert.ToDouble(json["current_tmp"].ToString());
						break;
					case "LT":
						value = Convert.ToDouble(json["current_lum"].ToString());
						break;
					case "HD":
						value = Convert.ToDouble(json["current_hum"].ToString());
						break;
					default:
						value = 0;
							break;
				}




			}
			else { 
						value = 0;
			}
			Debug.WriteLine("\n geting current data value ==>"+value+"\n");
			return value;

		}

	}
}