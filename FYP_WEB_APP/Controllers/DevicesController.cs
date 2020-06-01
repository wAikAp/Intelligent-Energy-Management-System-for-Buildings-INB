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
using FYP_WEB_APP.Controllers;
using FYP_WEB_APP.Models.LogicModels;
using Microsoft.AspNetCore.Http;

namespace FYP_APP.Controllers
{
	public class DevicesController : Controller
	{
		public List<DevicesListModel> MongoDevicesList = new List<DevicesListModel> { };
		public IMongoCollection<DevicesListModel> Getconn()
		{

			return new DBManger().DataBase.GetCollection<DevicesListModel>("DEVICES_LIST");
		}
		[Route("Devices/Devices")]
		public IActionResult Devices()
		{
			ViewBag.sortIMG = "sort.png";

			//Request.Query["sortOrder"] check 
			if (Request.Query.ContainsKey("sortOrder"))
			{
				this.MongoDevicesList = GetDevicesbyid();
				ViewData["MongoDevicesListModel"] = this.MongoDevicesList;
				ViewData["RoomListModel"] = GetRoomData();


			}
			else if (!Request.Query.ContainsKey("sortOrder") && Request.Query.Keys.Count() > 0)
			{
				SearchDevices();
				ViewData["queryString"] ="&"+ Request.QueryString.ToString().Substring(1, Request.QueryString.ToString().Length-1);
			}
			else
			{

				this.MongoDevicesList = GetAllDevices();
				ViewBag.roomIdSortParm = "?sortOrder=roomId";
				ViewData["MongoDevicesListModel"] = this.MongoDevicesList;
				ViewData["RoomListModel"] = GetRoomData();


			}


			return View();
		}
		public ActionResult ReturnUrl()
		{
			if (Request.Cookies.TryGetValue("returnUrl", out string url))
			{
				Response.Cookies.Delete("returnUrl");
				return Redirect(url);

			}
			else
			{
				return RedirectToAction("Devices");
			}
		}
		[Route("Devices/Devices/{id}")]
		public IActionResult Devices(string id) {
			ViewData["MongoDevicesListModel"] = GetAllDevices().Where(x=>x.roomId==id).ToList();
			ViewData["RoomListModel"] = GetRoomData();

			return View();
		}

		[Route("Devices/DevicesChart")]
		public ActionResult DevicesChart()
		{
			//string id  = Request.Query["roomID"];

			List<DevicesListModel> lists = GetAllDevices();

			ViewBag.charttitle = Request.Query["title"];
			ViewBag.chartType = Request.Query["chartType"];
			ViewBag.position = Request.Query["position"];
			ViewBag.download = Request.Query["download"];

			ViewBag.datasets = ChartData(lists, Request.Query["sensorType"]);

			ChartController chart = new ChartController();

			ViewBag.day = chart.GetChartTime();
			ViewBag.divId = chart.GetRandomDivId();

			return PartialView("_DevicesChart");
		}
		[Route("Devices/DevicesChartByRoomid")]
		public ActionResult DevicesChartByRoomid()
		{
			string id = "";
			id = Request.Query["roomID"];

			List<DevicesListModel> lists = GetAllDevices().Where(x => x.roomId.Contains(id)).ToList();

			ViewBag.charttitle = Request.Query["title"];
			ViewBag.chartType = Request.Query["chartType"];
			ViewBag.position = Request.Query["position"];
			ViewBag.download = Request.Query["download"];

			ViewBag.datasets = ChartData(lists, Request.Query["sensorType"]);

			ChartController chart = new ChartController();

			ViewBag.day = chart.GetChartTime();
			ViewBag.divId = chart.GetRandomDivId();

			return PartialView("_DevicesChart");
		}
		[Route("Devices/SearchDevicesByRoomid")]
		public IActionResult SearchDevicesByRoomid()
		{
			string id = Request.Query["roomID"];

			ViewData["MongoDevicesListModel"] = GetAllDevices().Where(x => x.roomId == id).ToList();
			ViewData["RoomListModel"] = GetRoomData();
			ViewData["roomID"] = id;
			//getAllDevices().Where(x => x.roomId == id).ToList().ToJson().ToString()
			return PartialView("_DevicesList");

		}
		public void SearchDevices()
		{

			ViewData["Title"] = "Search Devices";
			
			var roomid = ""; //get room id if have
 			if (Request.Query.ContainsKey("roomId")) {
				if (Request.Query["roomId"] != string.Empty || Request.Query["roomId"] != "")
				{
					roomid = Request.Query["roomId"];
				}
			}
			var dev_id = "";//get device id if have
			if (Request.Query.ContainsKey("device_id"))
			{
				if (Request.Query["device_id"] != string.Empty || Request.Query["device_id"] != "")
				{
					dev_id = Request.Query["device_id"];
				}
			}

            //get what type need to search if have
			List<string> typeList = new List<string>();
			//record the check box
			string chkStr = "checked";
			List<string> typeCheckedList = new List<string>() {"", "", "", "", "" };//last one is all checked

            if (Request.Query.Keys.Count() > 2) { 
			    foreach (string key in Request.Query.Keys)
			    {
				    switch (key) {
                        case "AC":
							typeCheckedList[0] = chkStr;
							typeList.Add(key);
							break;
						case "LT":
							typeCheckedList[1] = chkStr;
							typeList.Add(key);
							break;
						case "HD":
							typeCheckedList[2] = chkStr;
							typeList.Add(key);
							break;
						case "EF":
							typeCheckedList[3] = chkStr;
							typeList.Add(key);
							break;
						default:
							break;
				    }
			    }
			}

			var deivcelist = GetAllDevices();
			List<DevicesListModel> newSearchedlist = new List<DevicesListModel>();
			if (!Request.Query.ContainsKey("All")) {
				
				//if not all type need check which type the user selected.
				foreach (string dtype in typeList)
				{
					newSearchedlist.AddRange(deivcelist.Where(ls => ls.devicesId.Contains(dtype)).ToList());
				}
			}else {
				typeCheckedList[4] = chkStr;
				//if all type, then get all devices
				newSearchedlist = deivcelist;
			}

			if (!roomid.Equals(" ") || roomid.Length > 0){
                //search by room id
				newSearchedlist = newSearchedlist.Where(ls => ls.roomId.Contains(roomid)).ToList();
			}

			if (!dev_id.Equals(" ") || dev_id.Length > 0){
				//search by device id
				newSearchedlist = newSearchedlist.Where(ls => ls.devicesId.Contains(dev_id)).ToList();
			}
			ViewData["searchedRoomId"] = roomid;
			ViewData["searchedDeviceId"] = dev_id;
			ViewData["typeCheckedList"] = typeCheckedList;
			ViewData["MongoDevicesListModel"] = newSearchedlist;
			//Debug.WriteLine("Find list = " + deivcelist.ToJson().ToString());
			ViewData["RoomListModel"] = GetRoomData();
		}
		public List<DevicesListModel> GetSearchDevices()
		{

			ViewData["Title"] = "Search Devices";

			var roomid = ""; //get room id if have
			if (Request != null)
			{
				if (Request.Query.ContainsKey("roomId"))
				{
					if (Request.Query["roomId"] != string.Empty || Request.Query["roomId"] != "")
					{
						roomid = Request.Query["roomId"];
					}
				}

				var dev_id = "";//get device id if have
				if (Request.Query.ContainsKey("device_id"))
				{
					if (Request.Query["device_id"] != string.Empty || Request.Query["device_id"] != "")
					{
						dev_id = Request.Query["device_id"];
					}
				}

				//get what type need to search if have
				List<string> typeList = new List<string>();
				//record the check box
				string chkStr = "checked";
				List<string> typeCheckedList = new List<string>() { "", "", "", "", "" };//last one is all checked

				if (Request.Query.Keys.Count() > 2)
				{
					foreach (string key in Request.Query.Keys)
					{
						switch (key)
						{
							case "AC":
								typeCheckedList[0] = chkStr;
								typeList.Add(key);
								break;
							case "LT":
								typeCheckedList[1] = chkStr;
								typeList.Add(key);
								break;
							case "HD":
								typeCheckedList[2] = chkStr;
								typeList.Add(key);
								break;
							case "EF":
								typeCheckedList[3] = chkStr;
								typeList.Add(key);
								break;
							default:
								break;
						}
					}
				}


				var deivcelist = GetAllDevices();
				List<DevicesListModel> newSearchedlist = new List<DevicesListModel>();
				if (!Request.Query.ContainsKey("All"))
				{

					//if not all type need check which type the user selected.
					foreach (string dtype in typeList)
					{
						newSearchedlist.AddRange(deivcelist.Where(ls => ls.devicesId.Contains(dtype)).ToList());
					}
				}
				else
				{
					typeCheckedList[4] = chkStr;
					//if all type, then get all devices
					newSearchedlist = deivcelist;
				}

				if (!roomid.Equals(" ") || roomid.Length > 0)
				{
					//search by room id
					newSearchedlist = newSearchedlist.Where(ls => ls.roomId.Contains(roomid)).ToList();
				}

				if (!dev_id.Equals(" ") || dev_id.Length > 0)
				{
					//search by device id
					newSearchedlist = newSearchedlist.Where(ls => ls.devicesId.Contains(dev_id)).ToList();
				}
				ViewData["searchedRoomId"] = roomid;
				ViewData["searchedDeviceId"] = dev_id;
				ViewData["typeCheckedList"] = typeCheckedList;
				ViewData["MongoDevicesListModel"] = newSearchedlist;
				//Debug.WriteLine("Find list = " + deivcelist.ToJson().ToString());
				ViewData["RoomListModel"] = GetRoomData();
				return newSearchedlist;
			}
			else {

				return GetAllDevices();
			}
		}


		[Route("Devices/AddDevices")]
		public ActionResult AddDevices()//display add sensors form
		{
			ViewBag.viewType = "Add";
			ViewData["RoomListModel"] = GetRoomData();
			ViewBag.action = "AddDevicesData";

			return PartialView("_AddDevices");
		}
		[Route("Devices/AddDevices/{id}")]
		public ActionResult AddDevices(string id)//display add sensors form
		{
			ViewBag.viewType = "Add";
			ViewData["RoomListModel"] = GetRoomData();
			ViewBag.action = "AddDevicesData";
			ViewBag.CheckRoomid = id;

			return PartialView("_AddDevices");
		}
		[Route("Devices/EditDevices/{id}")]
		public ActionResult EditDevices(string id)
		{
			ViewData["EditDevicesListModel"] = GetAllDevices().Where(t => t.devicesId == id).ToList();
			//Debug.WriteLine(GetAllDevices().Where(t => t.devicesId == id).ToList().ToJson().ToString());
			ViewBag.viewType = "Edit";
			ViewBag.action = "UpdateDevicesData";

			return PartialView("_AddDevices");
		}
		[Route("Devices/DropDevices/{id}")]
		public ActionResult DropDevices(string id)//display Drop sensors form
		{
			ViewData["EditDevicesListModel"] = GetAllDevices().Where(t => t.devicesId == id).ToList();

			ViewBag.viewType = "Drop";
			ViewBag.action = "DropDevicesData";

			return PartialView("_AddDevices");
		}
		[Route("Devices/AddDevicesData")]
		[HttpPost]
		public ActionResult AddDevicesData(MongoDevicesListModel postData)//post
		{
			MongoDevicesListModel insertList = new MongoDevicesListModel { };

			var all = GetAllDevices().Where(d => d.devicesId.Contains(postData.devicesId));


			int count = all.Count();
			string id = postData.devicesId + count;
			all = GetAllDevices().Where(c => c.devicesId == (id));



			while (all.Count() != 0)
			{
				count += 1;
				id = postData.devicesId + count;
				all = GetAllDevices().Where(c => c.devicesId == (id));

				// code block to be executed
			}
			string desc = "";
			if (postData.desc != null) {
				desc = postData.desc;
			}
			var iso8601String = "00010101T00:00:00Z";
			DateTime dateISO8602 = DateTime.ParseExact(iso8601String, "yyyyMMddTHH:mm:ssZ",
								System.Globalization.CultureInfo.InvariantCulture);
			//DateTime turnOnDate = Convert.ToDateTime(datestr);
			//new Date('0001-01-01 00:00:00');
			//DateTime.UtcNow
			insertList.roomId = postData.roomId;
			insertList.devicesId = postData.devicesId + count;
			insertList.devices_Name = postData.devices_Name;
			insertList.pos_x = postData.pos_x;
			insertList.pos_y = postData.pos_y;
			insertList.desc = desc;
			insertList.lastest_checking_time = dateISO8602;
			insertList.turn_on_time = dateISO8602;
			insertList.power = 0;
			insertList.status = false;
			insertList.set_value = 0.0;

            try
			{
				new DBManger().DataBase.GetCollection<MongoDevicesListModel>("DEVICES_LIST").InsertOneAsync(insertList);
			}
			catch (Exception)
			{
				//Debug.WriteLine("line 262 error ");

				return ReturnUrl();
			};
			return ReturnUrl();
		}

		[Route("Devices/DropDevicesData")]
		[HttpPost]
		public ActionResult DropDevicesData(MongoDevicesListModel postData)//post
		{
			var DeleteResult = new DBManger().DataBase.GetCollection<MongoDevicesListModel>("DEVICES_LIST").DeleteOne(de => de.devicesId==postData.devicesId & de.roomId == postData.roomId);
			if (DeleteResult.IsAcknowledged)
			{
				if (DeleteResult.DeletedCount != 1)
				{
					return ReturnUrl();

					throw new Exception(string.Format("Count [value:{0}] after delete is invalid", DeleteResult.DeletedCount));

				}
			}
			else
			{
				return ReturnUrl();

				throw new Exception(string.Format("Delete for [_id:{0}] was not acknowledged", postData.devicesId.ToString()));

			}
			return ReturnUrl();
		}
		[Route("Devices/UpdateDevicesData")]
		[HttpPost]
		public ActionResult UpdateDevicesData(MongoDevicesListModel postData)
		{
			//Debug.WriteLine("postData :==>>" + postData.ToJson().ToString());
			var filter = Builders<MongoDevicesListModel>.Filter.Eq(d=>d.devicesId, postData.devicesId);
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
						//Debug.WriteLine("error: line 303 =>> " + property.Name + ":" + property.GetValue(postData));

						var UpdateResult = new DBManger().DataBase.GetCollection<MongoDevicesListModel>("DEVICES_LIST").FindOneAndUpdateAsync(u => u.devicesId== postData.devicesId, up);
						if (UpdateResult == null)
						{
							return ReturnUrl();

							//Debug.WriteLine("error: line 306 =>> "+ property.Name);
								//throw new Exception(string.Format("Count [value:{0}] after Modifi is invalid"));							
						}
						
						//this.isUpdated = Updated.IsAcknowledged;
					}
				}
			}
			return ReturnUrl();
		}

		public ActionResult turnOnOffDevice(IFormCollection postFrom) {

            
            var deviceID = postFrom["deviceID"];
			var statusStr = postFrom["deviceCurrentStatus"];//"Turning On" or "Turning Off" 
			Boolean status = statusStr.Equals("Turning On") ? false : true; // if is turning on then turn off the device
			try
			{
				DateTime nowTime = DateTime.UtcNow.AddHours(8);
				var update = Builders<MongoDevicesListModel>.Update
					.Set(x => x.status, status)
					.Set(x => x.turn_on_time, nowTime);
				var UpdateResult = new DBManger().DataBase.GetCollection<MongoDevicesListModel>("DEVICES_LIST")
					.FindOneAndUpdateAsync(u => u.devicesId == deviceID, update);
				return ReturnUrl();

			}
			catch (Exception ex){
				//Debug.WriteLine("DevicesController turnOnOffDevice error: "+ex);
				return ReturnUrl();
			}
		}


		public List<DevicesListModel> GetAllDevices() {			
			var collection = Getconn();
			IQueryable<DevicesListModel> query = from d in collection.AsQueryable<DevicesListModel>() select d;
			List<DevicesListModel> list=new List<DevicesListModel>();

            DevicesPowerUseOutputUtil devicesPowerUseOutputUtil = new DevicesPowerUseOutputUtil();

			foreach (var get in query.ToList()) {

                DateTime nowData = DateTime.Now;
                DateTime runtime = get.turn_on_time;
                TimeSpan count = new TimeSpan(nowData.Ticks - runtime.Ticks);
				//Debug.WriteLine("currentValue " + get.devicesId + ":");

				double currentValue = GetCurrentValue(get.devicesId);
				//Debug.WriteLine("currentValue "+ get.devicesId+ ":"+currentValue);
				double currentMonth_Usage = devicesPowerUseOutputUtil.getSpecApplianceTurnOnPowerUse(get.devicesId);
				double monthTotalUseTime = devicesPowerUseOutputUtil.getSpecApplianceMonthlyPowerUseTime(get.devicesId) * 0.00027777777777778;
				monthTotalUseTime = Convert.ToDouble(monthTotalUseTime.ToString("0.000"));


				double turnedOnUsage = devicesPowerUseOutputUtil.getSpecApplianceTurnOnPowerUse(get.devicesId);
				double turnedOnTime = devicesPowerUseOutputUtil.getSpecApplianceTurnOnPowerUseTime(get.devicesId) * 0.00027777777777778;
				turnedOnTime = Convert.ToDouble(turnedOnTime.ToString("0.000"));

				//double avgPowers = currentMonth_Usage / count.TotalDays;
				double avgPowers = currentMonth_Usage / 30;
				avgPowers = Convert.ToDouble(avgPowers.ToString("0.00"));

				var data = new DevicesListModel()
				{
					_id = get._id,
					roomId = get.roomId,
					devicesId = get.devicesId,
					devices_Name = get.devices_Name,
					power = get.power,
					lastest_checking_time = get.lastest_checking_time,
					turn_on_time = get.turn_on_time,
					pos_x = get.pos_x,
					pos_y = get.pos_y,
					desc = get.desc,
					current = currentValue,
					powerOnOff = false,
					avgPower = avgPowers,
					status = get.status,
					set_value = get.set_value,
					currentMonthTotalUseTime = monthTotalUseTime,
					currentMonthUsage = currentMonth_Usage,
					turnedOnTime = turnedOnTime,
					turnedOnUsage = turnedOnUsage
				};
				//Debug.WriteLine(data.current);
				list.Add(data);
			}

			MongoDevicesList = query.ToList();
			return list.ToList();
		}
		public List<DevicesListModel> GetDevicesListByRoomid(string id)
		{
			Debug.WriteLine(GetAllDevices().Where(d => d.roomId.Contains(id)).ToList().ToJson().ToString());
			return GetAllDevices().Where(d => d.roomId.Contains(id)).ToList();
		}
		public List<DevicesListModel> GetDevicesbyid() {
			List<DevicesListModel> list = null;
			string sortOrder = Request.Query["sortOrder"];
			sortOrder=ChangeSortLink(sortOrder);

			if (Request.Query.ContainsKey("sortOrder")is false)
			{
				ViewBag.sortIMG = "sort.png";

			}
			else if (sortOrder.Contains("Desc"))
			{
				list = GetAllDevices().OrderByDescending(item => item.roomId).ToList();
				//.Sort.Descending(sortOrder[0..^5]);
				ViewBag.sortIMG = "sort_desc.png";

			}
			else
			{

				ViewBag.sortIMG = "sort.png";

				list = GetAllDevices().OrderBy(item => item.roomId).ToList();

			}
			return list;
		}
		public string ChangeSortLink(string sortOrder)
		{
			//int count = Request.Query.Keys.Count;
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
		public double GetCurrentValue(string Id)
		{
			ConnectDB conn = new ConnectDB();
			var database = conn.Conn();

			IMongoCollection<BsonDocument> collection;
			FilterDefinition<BsonDocument> filter;
			var dbStr =  Id.Substring(0, 2) switch
			{
				"EF" => "EXH_FAN",
				"AC" => "AC",
				"LT" => "LIGHT",
				"HD" => "HUM",
                "CA" => "PI_CAM",
				_ => ""
			};
			var json=new DBManger().DataBase.GetCollection<CurrentDataModel>(dbStr).Find(Builders<CurrentDataModel>.Filter.Eq("devicesId", Id)).Sort(Builders<CurrentDataModel>.Sort.Descending(s => s.latest_checking_time)).ToList();

			if (json.Count()>0 && json!=null)
			{
				//Debug.WriteLine("current =="+ json.First().current+"   "+ json.First().latest_checking_time);
				return json.First().current;
	
			}
			else {
				//Debug.WriteLine("current ==0" );

				return 0;
			}
		}
		public string ChartData(List<DevicesListModel> DevicesDataList, string type)
		{
			switch (type)
			{
				case "AC":
					DevicesDataList = DevicesDataList.Where(x => x.devicesId.Contains("AC")).ToList();
					ViewBag.unit = " C";
					ViewBag.unitName = "Air Conditioner";
					break;
				case "LT":
					DevicesDataList = DevicesDataList.Where(x => x.devicesId.Contains("LT")).ToList();
					ViewBag.unit = " ";
					ViewBag.unitName = "Light";
					break;
				case "HD":
					DevicesDataList = DevicesDataList.Where(x => x.devicesId.Contains("HD")).ToList();
					ViewBag.unit = " %";
					ViewBag.unitName = "Humidifier";
					break;
				case "EF":
					DevicesDataList = DevicesDataList.Where(x => x.devicesId.Contains("EF")).ToList();
					ViewBag.unit = " rpm";
					ViewBag.unitName = "FAN";
					break;
				default:
					
					break;
			}
			return GetChartData(DevicesDataList);
		}

		public string GetChartData(List<DevicesListModel> DevicesDataList)
		{
				ChartController chart = new ChartController();

					//chart color
					List<string> Color = new List<string>();
					//sensor log


					List<CurrentDataModel> DevicesCurrentList = new List<CurrentDataModel>();
					//only ts

					DateTime today = DateTime.Now;


					//end set time
					List<string> labelss = new List<string>();
					List<double> data = new List<double>();

					List<object> datasets = new List<object>();
					List<object> datas = new List<object>();

					foreach (DevicesListModel get in DevicesDataList)
					{
						Color.Add(chart.GetRandomColor());
						labelss.Add(get.devicesId);
						DevicesCurrentList = GetChartDataList(get.devicesId).Where(x => x.latest_checking_time > today.AddDays(-1)).OrderBy(x => x.latest_checking_time).ToList();
						//Debug.WriteLine(get.devicesId+" line 596 :" + DevicesCurrentList.ToJson().ToString());

						DateTime ca = today;
				//TimeSpan catime = ca - ca.AddDays(-1);
				TimeSpan catime = ca - ca.AddHours(-6);


				int counttime = Convert.ToInt32(catime.TotalMinutes / 5);

						for (int x = 0; x <= counttime; x++)
						{
							data.Add(0);

						}
				//Debug.WriteLine("line 603 : " + DevicesCurrentList.Count());

				if (DevicesCurrentList.Count() != 0)
						{
					foreach (CurrentDataModel getCurrent in DevicesCurrentList)
							{
						//Debug.WriteLine("line 615 : "+ DevicesCurrentList.ToJson().ToString());

						var value = Convert.ToDouble(Convert.ToDouble(getCurrent.current).ToString("0.00"));

								ca = today.AddHours(-6);

								for (int x = 0; x <= counttime; x++)
								{
									var bo = getCurrent.latest_checking_time >= ca && getCurrent.latest_checking_time <= ca.AddMinutes(5);

									ca = ca.AddMinutes(5);
									if (getCurrent.latest_checking_time > ca && getCurrent.latest_checking_time < ca.AddMinutes(5))
									{
										data[x] = value;
									}
								}
							}
						}
						else
						{
						}
						datas.Add(data.ToArray());
					}

					for (int i = 0; i < DevicesDataList.Count; i++)
					{
						labelss.Add(DevicesDataList[i].devicesId);
					}

					foreach (var get in datas) {
						//Debug.WriteLine(get.ToJson());

					}
			return chart.LineChart(DevicesDataList.Count, labelss, datas);
			
		}
		public List<CurrentDataModel> GetChartDataList(string id)
		{

					string tableName = "";
					List<CurrentDataModel> List = new List<CurrentDataModel>();
					IMongoCollection<CurrentDataModel> collection;
					
			//db collection
			tableName = id.Substring(0, 2) switch
			{
				"EF" => "EXH_FAN",
				"AC" => "AC",
				"LT" => "LIGHT",
				"HD" => "HUM",
				_ => ""
			};
			collection = new DBManger().DataBase.GetCollection<CurrentDataModel>(tableName);
					IQueryable<CurrentDataModel> query;
					query = from c in collection.AsQueryable<CurrentDataModel>() orderby c.latest_checking_time descending where c.devicesId.Contains(id) select c;
					return query.ToList();	
		}
		public string[] typeName= { "Air Conditioner", "Light", "Humidifier", "Exhaust Fan" };

		public int GetDeviceCount(string TypeName) {
			string shortName="";
			switch (TypeName)
			{
				case "Air Conditioner":
					shortName = "AC";
					break;
				case "Light":
					shortName = "LT";
					break;
				case "Humidifier":
					shortName = "HD";
					break;
				case "EXH_FAN":
					shortName = "EF";
					break;
				default:
					break;
			}
			return GetAllDevices().Where(c => c.devicesId.Contains(shortName)).Count();
			
		}
	}
}