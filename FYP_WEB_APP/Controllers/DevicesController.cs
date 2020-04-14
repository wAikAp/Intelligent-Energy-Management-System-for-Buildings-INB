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
		public List<MongoDevicesListModel> MongoDevicesList = new List<MongoDevicesListModel> { };
		public IMongoCollection<MongoDevicesListModel> getMDLMconn()
		{
			ConnectDB conn = new ConnectDB();
			var database = conn.Conn();
			var collection = database.GetCollection<MongoDevicesListModel>("DEVICES_LIST");
			return collection;
		}
		[Route("Devices/Devices")]
		public IActionResult Devices()
		{
			ViewBag.sortIMG = "sort.png";

			//Request.Query["sortOrder"] check 
			if (Request.Query.ContainsKey("sortOrder"))
			{
				System.Diagnostics.Debug.WriteLine("----------have sortorder");
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
				System.Diagnostics.Debug.WriteLine("----------NOT have sortorder");

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
		[Route("Devices/SearchDevicesByRoomid/{id}")]
		public IActionResult SearchDevicesByRoomid(string id)
		{
			ViewData["MongoDevicesListModel"] = getAllDevices().Where(x => x.roomId == id).ToList();
			ViewData["RoomListModel"] = GetRoomData();
			//getAllDevices().Where(x => x.roomId == id).ToList().ToJson().ToString()
			return PartialView("_DevicesList");

		}
		public void SearchDevices()
		{
			System.Diagnostics.Debug.WriteLine("Start ------ SearchDevices");

			ViewData["Title"] = "Search Devices";
			var list = getAllDevices();
			List<MongoDevicesListModel> sum = new List<MongoDevicesListModel> { };

			List<MongoDevicesListModel> get=new List<MongoDevicesListModel>{ };
			if (Request.Query.ContainsKey("roomId")) {
				list=list.Where(ls => ls.roomId.Contains(Request.Query["roomId"])).ToList();
			}
			foreach (String key in Request.Query.Keys)
			{
				string skey = key;
				string keyValue = Request.Query[key];
				System.Diagnostics.Debug.WriteLine(skey.ToString());
				System.Diagnostics.Debug.WriteLine(keyValue.ToString());

				switch (skey) {					
					case "AC":
					case "LT":
					case "HD":
					case "EF":
						get = list.Where(ls => ls.devicesId.Contains(skey) ).ToList();
						break;
				}
				sum.AddRange(get);
				sum = sum.Distinct().ToList();//delet double data
				}
			list = list.Intersect(sum).ToList();
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
			System.Diagnostics.Debug.WriteLine("=========="+postData.ToJson().ToString());
			MongoDevicesListModel insertList = new MongoDevicesListModel { };

			var all = getAllDevices();
			string count = "";
			if (all.Count < 10) { count = "00" + (all.Count+1).ToString(); }
			else if (all.Count < 100) { count = "0" + (all.Count + 1).ToString(); }

			//sensortype = postData.Sensortype;
			insertList.roomId = postData.roomId;
			insertList.devicesId = postData.devicesId + count;
			insertList.devices_Name = postData.devices_Name;
			insertList.location = postData.location;
			insertList.desc = postData.desc;
			insertList.lastest_checking_time = DateTime.UtcNow;
			insertList.total_run_time = 0;
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
			var filter = Builders<MongoDevicesListModel>.Filter.Eq("devicesId", postData.devicesId);
			filter = filter & Builders<MongoDevicesListModel>.Filter.Eq("roomId", postData.roomId);
			var type = postData.GetType();
			var props = type.GetProperties();
			foreach (var property in props)
			{
				if (!property.Name.Equals("_id"))
				{
					if (property.GetValue(postData) != null)
					{
						UpdateDefinition<MongoDevicesListModel> up;
						if (property.Name == "latest_checking_time")
						{
							up = Builders<MongoDevicesListModel>.Update.Set(property.Name.ToString(), DateTime.UtcNow);
						}
						else
						{
							up = Builders<MongoDevicesListModel>.Update.Set(property.Name.ToString(), property.GetValue(postData).ToString());

						}
						var Updated = getMDLMconn().UpdateOne(filter, up);
						//this.isUpdated = Updated.IsAcknowledged;
					}
				}
			}
			return RedirectToAction("Devices");
		}


		public List<MongoDevicesListModel> getAllDevices() {			
			var collection = getMDLMconn();
			IQueryable<MongoDevicesListModel> query = from d in collection.AsQueryable<MongoDevicesListModel>() select d;
			MongoDevicesList = query.ToList();
			return query.ToList();
		}
		public List<MongoDevicesListModel> getDevicesbyid() {
			List<MongoDevicesListModel> list = null;
			string sortOrder = Request.Query["sortOrder"];
			sortOrder=ChangeSortLink(sortOrder);

			if (Request.Query.ContainsKey("sortOrder")is false)
			{
				ViewBag.sortIMG = "sort.png";
				System.Diagnostics.Debug.WriteLine("NOT change link");

			}
			else if (sortOrder.Contains("Desc"))
			{
				list = getAllDevices().OrderByDescending(item => item.roomId).ToList();
				//.Sort.Descending(sortOrder[0..^5]);
				ViewBag.sortIMG = "sort_desc.png";
				System.Diagnostics.Debug.WriteLine("Desc change link sort_desc.png");

			}
			else
			{

				ViewBag.sortIMG = "sort.png";

				list = getAllDevices().OrderBy(item => item.roomId).ToList();
				System.Diagnostics.Debug.WriteLine("change link sort.png");

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


	}
}