using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using FYP_WEB_APP.Controllers;
using FYP_WEB_APP.Controllers.Mongodb;
using FYP_WEB_APP.Models;
using FYP_WEB_APP.Models.MongoModels;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FYP_APP.Controllers
{
	public class SensorsController : Controller
	{
		private IMongoDatabase database;
		private string PageRoomId = "";
		private bool isUpdated;

		public void getdb()
		{
			ConnectDB conn = new ConnectDB();
			this.database = conn.Conn();
		}
		[Route("Sensors/")]
		[Route("Sensors/Sensors")]
		public ActionResult Sensors()
		{
			ViewData["NotGroup"] = "false";
			ViewBag.SearchRoomIdENorDisable = "";
			getdb();
			ViewData["SensorsListModel"] = Setgroup(GetSensorsData());

			ViewData["RoomListModel"] = GetRoomData();

			return View();
		}
		[Route("Sensors/SensorsChart")]
		public ActionResult SensorsChart()
		{
			string id = "";
			id = Request.Query["roomID"];

			getdb();
			List<SensorsListModel> lists = GetSensorsData();
			Debug.WriteLine(lists.ToJson().ToString());
			Debug.WriteLine(Request.Query["title"]);
			Debug.WriteLine(Request.Query["chartType"]);
			Debug.WriteLine(Request.Query["position"]);
			Debug.WriteLine(Request.Query["sensorType"]);
			//Debug.WriteLine(Setgroup(lists).ToJson().ToString());
			ViewBag.charttitle = Request.Query["title"];
			ViewBag.chartType = Request.Query["chartType"];
			ViewBag.position = Request.Query["position"];
			ViewBag.download = Request.Query["download"];

			ViewBag.datasets = chartData(lists, Request.Query["sensorType"]);

			ChartController chart = new ChartController();

			ViewBag.day = chart.getChartTime();
			ViewBag.divId = chart.getRandomDivId();

			return PartialView("_SensorsChart");
		}
		[Route("Sensors/SensorsChartByRoomid")]
		public ActionResult SensorsChartByRoomid()
		{
			string id = "";
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
			
			ViewBag.datasets = chartData(lists, Request.Query["sensorType"]);

			ChartController chart = new ChartController();

			ViewBag.day = chart.getChartTime();
			ViewBag.divId = chart.getRandomDivId();

			return PartialView("_SensorsChart");

		}
		[Route("Sensors/SensorsListByRoomid")]
		public ActionResult SearchSensorsByRoomid()
		{
			ViewData["NotGroup"] = "true";
			getdb();
			string id = "";
			id = Request.Query["roomID"];
			List<SensorsListModel> lists = GetSensorsData().Where(x => x.roomId.Contains(id)).ToList();

			//Debug.WriteLine(Setgroup(lists).ToJson().ToString());
			ViewData["SensorsListModel"] = Setgroup(lists);

			return PartialView("_SensorsList");
		}
		[Route("Sensors/SensorsListByRoomid")]
		public List<SensorsListModel> getSensorsListByRoomid()
		{
			getdb();
			string id = "";
			id = Request.Query["roomID"];
			List<SensorsListModel> lists = GetSensorsData().Where(x => x.roomId.Contains(id)).ToList();

			return lists;
		}

		[Route("Sensors/Sensors/{id}")]
		public ActionResult Sensors(string id)
		{
			ViewData["NotGroup"] = "true";
			ViewBag.roomID = this.PageRoomId = id;
			ViewBag.SearchRoomIdENorDisable = "disabled";
			getdb();


			return View();
		}
		[Route("Sensors/EditSensors/{id}")]
		public ActionResult EditSensors(string id)
		{
			getdb();
			List<SensorsListModel> list = GetSensorsData();
			list = list.Where(x => x.sensorId.Contains(id)).ToList();
			ViewData["EditSensorsListModel"] = list;
			ViewBag.viewType = "Edit";
			ViewBag.action = "UpdateSensors";
			return PartialView("_AddSensors", list);
		}
		[Route("Sensors/UpdateSensors")]
		[HttpPost]
		public ActionResult UpdateSensors(MongoSensorsListModel postData)
		{

			getdb();

			var collection = database.GetCollection<MongoSensorsListModel>("SENSOR_LIST");
			var filter = Builders<MongoSensorsListModel>.Filter.Eq("sensorId", postData.sensorId);

			var type = postData.GetType();
			var props = type.GetProperties();

			foreach (var property in props)
			{
				if (!property.Name.Equals("_id"))
				{
					if (property.GetValue(postData) != null)
					{
						UpdateDefinition<MongoSensorsListModel> up;
						if (property.Name == "latest_checking_time")
						{
							up = Builders<MongoSensorsListModel>.Update.Set(property.Name.ToString(), DateTime.UtcNow);
							//Debug.WriteLine(DateTime.UtcNow);
						}
						else
						{
							up = Builders<MongoSensorsListModel>.Update.Set(property.Name.ToString(), property.GetValue(postData).ToString());

						}
						var Updated = collection.UpdateOne(filter, up);
						this.isUpdated = Updated.IsAcknowledged;
					}
				}
			}



			return RedirectToAction("Sensors");
		}
		[Route("Sensors/AddSensors")]
		public ActionResult AddSensors()//display add sensors form
		{
			ViewBag.viewType = "Add";
			ViewBag.ChangeType = "readonly";
			ViewData["RoomListModel"] = GetRoomData();
			ViewBag.action = "AddSensorsData";

			return PartialView("_AddSensors");
		}
		[Route("Sensors/AddSensorsData")]
		[HttpPost]
		public ActionResult AddSensorsData(SensorsListModel postData)//post
		{
			getdb();
			var collection = database.GetCollection<MongoSensorsListModel>("SENSOR_LIST");

			MongoSensorsListModel insertList = new MongoSensorsListModel { };

			var all = GetSensorsData();
			string count = "";
			if (all.Count < 10) { count = "00" + (all.Count + 1).ToString(); }
			else if (all.Count < 100) { count = "0" + (all.Count + 1).ToString(); }

			insertList.roomId = postData.roomId;
			insertList.sensorId = postData.Sensortype + count;
			//insertList.location = postData.location;
			insertList.pos_x = postData.pos_x;
			insertList.pos_y = postData.pos_y;
			insertList.desc = postData.desc;
			insertList.latest_checking_time = DateTime.UtcNow;
			insertList.total_run_time = 0;

			collection.InsertOneAsync(insertList);
			return RedirectToAction("Sensors");
		}
		[Route("Sensors/DropSensorsData")]
		[HttpPost]
		public ActionResult DropSensorsData(SensorsListModel postData)//post
		{
			getdb();
			var collection = database.GetCollection<MongoSensorsListModel>("SENSOR_LIST");

			var DeleteResult = collection.DeleteOne(Builders<MongoSensorsListModel>.Filter.Eq("sensorId", postData.sensorId));

			return RedirectToAction("Sensors");
		}
		[Route("Sensors/DropSensors/{id}")]
		public ActionResult DropSensors(string id)//display Drop sensors form
		{
			getdb();
			List<SensorsListModel> list = GetSensorsData();
			list = list.Where(x => x.sensorId.Contains(id)).ToList();

			ViewData["EditSensorsListModel"] = list;
			ViewBag.viewType = "Drop";
			ViewBag.action = "DropSensorsData";

			return PartialView("_AddSensors", list);
		}
		public List<SensorsListModel> FindSensors(List<SensorsListModel> SensorsDataList)
		{
			List<SensorsListModel> EndDataList = new List<SensorsListModel> { };
			List<SensorsListModel> FDataList = new List<SensorsListModel> { };
			List<SensorsListModel> roomSensorsDataList = new List<SensorsListModel> { };


			foreach (String key in Request.Query.Keys)
			{
				string skey = key;
				string keyValue = Request.Query[key];

				switch (skey)
				{
					case "roomId":
						roomSensorsDataList = SensorsDataList.Where(x => x.roomId.Contains(keyValue)).ToList();
						break;
					case "TS":
					case "LS":
					case "HS":
						FDataList = SensorsDataList.Where(x => x.sensorId.Contains(skey)).ToList();
						break;
					default:
						break;
				}
				if (skey != "sortOrder")
				{
					EndDataList.AddRange(FDataList);//B list add in A list

					EndDataList = EndDataList.Distinct().ToList();//delet double data

				}
			}

			//get B & A list Intersect data
			if (roomSensorsDataList.Count > 0)
			{
				SensorsDataList = roomSensorsDataList.Intersect(EndDataList).ToList();
			}
			else if (this.PageRoomId.Length > 0)
			{//Sensors/{id}
				roomSensorsDataList = SensorsDataList;
				SensorsDataList = roomSensorsDataList.Intersect(EndDataList).ToList();
			}

			return SensorsDataList;
		}
		public List<SensorsListModel> SortList(List<SensorsListModel> DataList)
		{
			string sortOrder = Request.Query["sortOrder"];
			sortOrder = ChangeSortLink(sortOrder);

			if (String.IsNullOrEmpty(sortOrder))
			{
				ViewBag.sortIMG = "sort.png";
			}
			else if (sortOrder.Contains("Desc"))
			{
				DataList = DataList.OrderByDescending(item => item.roomId).ToList();
				//.Sort.Descending(sortOrder[0..^5]);
				ViewBag.sortIMG = "sort_desc.png";

			}
			else
			{
				ViewBag.sortIMG = "sort.png";

				DataList = DataList.OrderBy(item => item.roomId).ToList();
			}
			return DataList;
		}
		public List<List<SensorsListModel>> Setgroup(List<SensorsListModel> SensorsDataList)
		{
			var groupedList = SensorsDataList.GroupBy(s => s.roomId)
				.Select(grp => grp.ToList())
				.ToList();
			return groupedList;
		}
		public List<SensorsListModel> GetSensorsData()
		{

			List<SensorsListModel> SensorsDataList = new List<SensorsListModel> { };
			IMongoCollection<SensorsListModel> collection;

			//db collection
			collection = database.GetCollection<SensorsListModel>("SENSOR_LIST");
			IQueryable<SensorsListModel> query;
			if (PageRoomId.Length == 0)
			{
				query = from c in collection.AsQueryable<SensorsListModel>() select c;
			}
			else
			{//Sensors/{id}
				query = from c in collection.AsQueryable<SensorsListModel>() where c.roomId.Contains(PageRoomId) select c;

			}

			foreach (SensorsListModel set in query)
			{

				var data = new SensorsListModel()
				{
					roomId = set.roomId,
					sensorId = set.sensorId,
					pos_x=set.pos_x,
					pos_y=set.pos_y,
					desc = set.desc,
					latest_checking_time = set.latest_checking_time,
					total_run_time = set.total_run_time,
					current_Value = Convert.ToDouble(getSensorCurrentValue(set.sensorId)),
					current_Time = Convert.ToDateTime(getSensorCurrentDate(set.sensorId)),
					typeImg = getType(set.sensorId),
					typeUnit = getunit(set.sensorId)
				};
				SensorsDataList.Add(data);
			}

			try
			{
				int count = Request.Query.Count;
				if (count != 0)
				{
					SensorsDataList = FindSensors(SensorsDataList);
					SensorsDataList = SortList(SensorsDataList);

				}
				else
				{
					SensorsDataList = SortList(SensorsDataList);

				}
			}
			catch (NullReferenceException)
			{
				SensorsDataList = SortList(SensorsDataList);

			}


			return SensorsDataList;
		}
		public List<RoomsListModel> GetRoomData()
		{
			getdb();
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
		public List<RoomsListModel> GetRoomData(string id)
		{
			var RoomDataList = new List<RoomsListModel> { };

			var data = new RoomsListModel()
			{
				roomId = id,
			};
			RoomDataList.Add(data);


			return RoomDataList;
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

			if (!(count > 1) && String.IsNullOrEmpty(sortOrder))
			{
				ViewBag.roomIdSortParm = "?sortOrder=roomId";
			}
			else if ((count > 1) && String.IsNullOrEmpty(sortOrder))
			{

				var roomidbutton = "?sortOrder=roomId_Desc" + "&" + rs;
				ViewBag.roomIdSortParm = roomidbutton;
			}
			else if (!(count > 1) && !String.IsNullOrEmpty(sortOrder))
			{
				string viewsortOrderroomId = sortOrder == "roomId" ? "roomId_Desc" : "roomId";
				ViewBag.roomIdSortParm = "?sortOrder=" + viewsortOrderroomId;

			}
			else if ((count > 1) && !String.IsNullOrEmpty(sortOrder))
			{
				string viewsortOrderroomId = sortOrder == "roomId" ? "roomId_Desc" : "roomId";
				ViewBag.roomIdSortParm = "?sortOrder=" + viewsortOrderroomId + "&" + rs;

			}
			//end change sorting link


			return sortOrder;
		}
		public string getunit(string sensorId)
		{
			string type = "";
			switch (sensorId.Substring(0, 2))
			{
				case "TS":
					type = "℃";
					break;
				case "LS":
					type = "lm";
					break;
				case "HS":
					type = "%";
					break;
				default:
					break;
			}
			return type;
		}
		public string getType(string sensorId)
		{
			string type = "";
			switch (sensorId.Substring(0, 2))
			{
				case "TS":
					type = "temp.png";
					break;
				case "LS":
					type = "light.png";
					break;
				case "HS":
					type = "humidity.png";
					break;
				default:
					break;
			}
			return type;
		}
		public List<CurrentDataModel> getSensorIDCurrentList(string sensorId)
		{
			string tableName = "";
			List<CurrentDataModel> List = new List<CurrentDataModel>();
			IMongoCollection<CurrentDataModel> collection;
			switch (sensorId.Substring(0, 2))
			{
				case "TS":
					tableName = "TMP_SENSOR";
					break;
				case "LS":
					tableName = "LIGHT_SENSOR";
					break;
				case "HS":
					tableName = "HUM_SENSOR";
					break;
				default:
					break;
			}
			//db collection
			collection = database.GetCollection<CurrentDataModel>(tableName);
			IQueryable<CurrentDataModel> query;
			query = from c in collection.AsQueryable<CurrentDataModel>() orderby c.latest_checking_time descending where c.sensorId.Contains(sensorId) select c;

			return query.ToList();
		}
		public double getSensorCurrentValue(string sensorId)
		{
			double value = 0;

			IMongoCollection<BsonDocument> collection;
			FilterDefinition<BsonDocument> filter;
			var dbStr = "";
			switch (sensorId.Substring(0, 2))
			{
				case "TS":
					dbStr = "TMP_SENSOR";
					value = getCurrentValueByidBytable(sensorId);
					break;
				case "LS":
					dbStr = "LIGHT_SENSOR";
					value = getCurrentValueByidBytable(sensorId);
					break;
				case "HS":
					dbStr = "HUM_SENSOR";
					value = getCurrentValueByidBytable(sensorId);
					break;
				default:
					break;
			}
			return value;

		}

		public DateTime getSensorCurrentDate(string sensorId)
		{
			DateTime value = new DateTime();

			IMongoCollection<BsonDocument> collection;
			FilterDefinition<BsonDocument> filter;
			var dbStr = "";
			switch (sensorId.Substring(0, 2))
			{
				case "TS":
					value = getCurrentDateeByidBytable(sensorId);

					break;
				case "LS":
					value = getCurrentDateeByidBytable(sensorId);

					break;
				case "HS":
					value = getCurrentDateeByidBytable(sensorId);

					break;
				default:
					break;
			}
			return value;

		}
		public double getCurrentValueByidBytable(string sensorId)
		{

			List<CurrentDataModel> SensorsDataList = new List<CurrentDataModel>();
			SensorsDataList = getSensorIDCurrentList(sensorId);
			//Debug.WriteLine(SensorsDataList.ToJson().ToString());
			if (SensorsDataList.Count > 1)
			{
				return Convert.ToDouble(SensorsDataList.First().current);
			}
			else
			{
				return 0;
			}
		}
		public DateTime getCurrentDateeByidBytable(string sensorId)
		{
			DateTime value = DateTime.Now;

			List<CurrentDataModel> SensorsDataList = new List<CurrentDataModel>();
			IMongoCollection<CurrentDataModel> collection;

			SensorsDataList = getSensorIDCurrentList(sensorId);
			//Debug.WriteLine(SensorsDataList.ToJson().ToString());
			if (SensorsDataList.Count > 1)
			{
				return Convert.ToDateTime(SensorsDataList.First().latest_checking_time);
			}
			else
			{
				return value;
			}

		}
		public string chartData(List<SensorsListModel> SensorsDataList, string type)
		{
			switch (type)
			{
				case "TS":
					Debug.WriteLine("type TS");
					SensorsDataList = SensorsDataList.Where(x => x.sensorId.Contains("TS")).ToList();
					ViewBag.unit = " ";
					ViewBag.unitName = "Temperature";
					break;
				case "LS":
					SensorsDataList = SensorsDataList.Where(x => x.sensorId.Contains("LS")).ToList();
					ViewBag.unit = " lm";
					ViewBag.unitName = "Luminosity";
					break;
				case "HS":
					SensorsDataList = SensorsDataList.Where(x => x.sensorId.Contains("HS")).ToList();
					ViewBag.unit = " %";
					ViewBag.unitName = "Humidity";
					break;
				default:
					break;
			}
			return getChartData(SensorsDataList);
		}

		public string getChartData(List<SensorsListModel> SensorsDataList)
		{
			ChartController chart = new ChartController();

			//chart color
			List<string> Color = new List<string>();
			//sensor log


			List<CurrentDataModel> SensorsCurrentList = new List<CurrentDataModel>();
			//only ts

			DateTime today = DateTime.Now;


			//end set time
			List<string> labelss = new List<string>();
			List<double> data = new List<double>();

			List<object> datasets = new List<object>();
			List<object> datas = new List<object>();

			foreach (SensorsListModel get in SensorsDataList)
			{
				Color.Add(chart.getRandomColor());
				labelss.Add(get.sensorId);
				SensorsCurrentList = getSensorIDCurrentList(get.sensorId).Where(x => x.latest_checking_time > today.AddDays(-1)).OrderBy(x => x.latest_checking_time).ToList();
				bool countfirst = true;
				//Debug.WriteLine("Line 641:" + SensorsCurrentList.ToJson().ToString());
				//var first = SensorsCurrentList[0];
				DateTime ca = today;
				TimeSpan catime = ca - ca.AddDays(-1);

				//int counttime = Convert.ToInt32(catime.TotalMinutes / 5) -1- SensorsCurrentList.Count();
				int counttime = Convert.ToInt32(catime.TotalMinutes / 5);


				for (int x = 0; x <= counttime; x++)
				{
					data.Add(0);

				}
				if (SensorsCurrentList.Count() != 0)
				{
					foreach (CurrentDataModel getCurrent in SensorsCurrentList)
					{
						var value = Convert.ToDouble(Convert.ToDouble(getCurrent.current).ToString("0.00"));
						
						ca = DateTime.Now.AddDays(-1);

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
					Debug.WriteLine("chart current null");
				}
				datas.Add(data.ToArray());
			}

			for (int i = 0; i < SensorsDataList.Count; i++)
			{
				labelss.Add(SensorsDataList[i].sensorId);
			}

			return chart.Chart(SensorsDataList.Count, labelss, datas );
		}
	
	}
}