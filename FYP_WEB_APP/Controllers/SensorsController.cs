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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace FYP_APP.Controllers
{
	public class SensorsController : Controller
	{
		private readonly IMongoDatabase database = new DBManger().DataBase;
		private readonly IMongoCollection<MongoSensorsListModel> SensorsCollection = new DBManger().DataBase.GetCollection<MongoSensorsListModel>("SENSOR_LIST");
		private readonly IMongoCollection<RoomsListModel> RoomCollection = new DBManger().DataBase.GetCollection<RoomsListModel>("ROOM");

		public List<SensorsListModel> _sensorList;

		private string PageRoomId = "";
		private bool isUpdated;

		[Route("Sensors/")]
		[Route("Sensors/Sensors")]
		public ActionResult Sensors()
		{

			ViewData["NotGroup"] = "false";
			ViewBag.SearchRoomIdENorDisable = "";
			Debug.WriteLine("\n\n sensor Page Start");
			_sensorList = GetSensorsData();
			ViewData["SensorsListModel"] = Setgroup(_sensorList);

			ViewData["RoomListModel"] = GetRoomData();
			if (!string.IsNullOrEmpty(Request.QueryString.ToString()))
			{
				ViewData["queryString"] ="&"+ Request.QueryString.ToString().Substring(1, Request.QueryString.ToString().Length-1);

			}
			else {
				ViewData["queryString"] = "";

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
				return RedirectToAction("Sensors");
			}
		}
		[Route("Sensors/SensorsChart")]
		public ActionResult SensorsChart()
		{
			//string id  = Request.Query["roomID"];

			//List<SensorsListModel> lists = GetSensorsData();

			ViewBag.charttitle = Request.Query["title"];
			ViewBag.chartType = Request.Query["chartType"];
			ViewBag.position = Request.Query["position"];
			ViewBag.download = Request.Query["download"];

			Debug.WriteLine("****"+ _sensorList.ToJson().ToString());
			ViewBag.datasets = ChartData(_sensorList, Request.Query["sensorType"]);

			ChartController chart = new ChartController();

			ViewBag.day = chart.GetChartTime();
			ViewBag.divId = chart.GetRandomDivId();

			return PartialView("_SensorsChart");
		}
		[Route("Sensors/SensorsChartByRoomid")]
		public ActionResult SensorsChartByRoomid()
		{
			string id = "";
			id = Request.Query["roomID"];

			List<SensorsListModel> lists = GetSensorsData().Where(x => x.roomId.Contains(id)).ToList();
		
			ViewBag.charttitle = Request.Query["title"];
			ViewBag.chartType = Request.Query["chartType"];
			ViewBag.position = Request.Query["position"];
			ViewBag.download = Request.Query["download"];
			
			ViewBag.datasets = ChartData(lists, Request.Query["sensorType"]);

			ChartController chart = new ChartController();

			ViewBag.day = chart.GetChartTime();
			ViewBag.divId = chart.GetRandomDivId();

			return PartialView("_SensorsChart");

		}
		[Route("Sensors/SensorsListByRoomid")]
		public ActionResult SensorsListByRoomid()
		{
			ViewData["NotGroup"] = "true";
			string id = "";
			if (!String.IsNullOrEmpty(Request.Query["roomID"]))
			{
				id = Request.Query["roomID"];
				ViewBag.roomid	= id;
				List<SensorsListModel> list = GetAllSensors().Where(x => x.roomId.Contains(id)).ToList();
				//List<SensorsListModel> lists = GetSensorsData().Where(x => x.roomId.Contains(id)).ToList();
				Debug.WriteLine("sensor list by room id" + list.ToJson().ToString());
				ViewData["SensorsListModel"] = Setgroup(list);
			}
			else {
				id = "";
			}

			return PartialView("_SensorsList");
		}
		public List<SensorsListModel> GetSensorsListByRoomid(string id)
		{
			List<SensorsListModel> list = GetAllSensors().Where(x => x.roomId.Contains(id)).ToList();
			return list;
		}

		[Route("Sensors/Sensors/{id}")]
		public ActionResult Sensors(string id)
		{
			ViewData["NotGroup"] = "true";
			ViewBag.roomID = this.PageRoomId = id;
			ViewBag.SearchRoomIdENorDisable = "disabled";
			//getdb();


			return View();
		}
		//form
		[Route("Sensors/EditSensors/{id}")]
		public ActionResult EditSensors(string id)
		{
			//getdb();
			List<SensorsListModel> list = GetSensorsData();
			list = list.Where(x => x.sensorId.Contains(id)).ToList();
			ViewData["EditSensorsListModel"] = list;
			ViewBag.viewType = "Edit";
			ViewBag.action = "UpdateSensors";
			return PartialView("_AddSensors", list);
		}
		[Route("Sensors/UpdateSensors")]
		[HttpPost]
		public ActionResult UpdateSensors(MongoSensorsListModel postData )
		{

			//getdb();

			//var collection = database.GetCollection<MongoSensorsListModel>("SENSOR_LIST");
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
						}
						else
						{
							up = Builders<MongoSensorsListModel>.Update.Set(property.Name.ToString(), property.GetValue(postData).ToString());
							Debug.WriteLine("\n "+ property+" + " + property.GetValue(postData));
						}
						var Updated = SensorsCollection.UpdateOne(filter, up);

						this.isUpdated = Updated.IsAcknowledged;
						Debug.WriteLine("\n update status" + this.isUpdated);

					}
				}
			}

			return ReturnUrl();
		}
		
		[Route("Sensors/AddSensors/{id}")]
		public ActionResult AddSensors(string id)//display add sensors form
		{
			ViewBag.viewType = "Add";
			ViewBag.ChangeType = "readonly";
			ViewData["RoomListModel"] = GetRoomData();
			ViewBag.action = "AddSensorsData";
			ViewBag.CheckRoomid = id;

			return PartialView("_AddSensors");
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
			//getdb();
			//var collection = database.GetCollection<MongoSensorsListModel>("SENSOR_LIST");

			MongoSensorsListModel insertList = new MongoSensorsListModel { };
			
			
			var all = GetSensorsData().Where(c=>c.sensorId.Contains(postData.Sensortype));
			int count = all.Count();
			string id = postData.Sensortype + count;
			all = GetSensorsData().Where(c => c.sensorId == (id));



			while (all.Count()!=0)
				{
				count += 1;
				id = postData.Sensortype + count;
				all = GetSensorsData().Where(c => c.sensorId == (id));
				
				// code block to be executed
			}


			insertList.roomId = postData.roomId;
			insertList.sensorId = postData.Sensortype + count;
			//insertList.location = postData.location;
			insertList.pos_x = postData.pos_x;
			insertList.pos_y = postData.pos_y;
			insertList.desc = postData.desc;
			insertList.latest_checking_time = DateTime.UtcNow;
			insertList.total_run_time = DateTime.UtcNow;
			try
			{
				new DBManger().DataBase.GetCollection<MongoSensorsListModel>("SENSOR_LIST").InsertOneAsync(insertList);
			}
			catch (Exception )
			{

				return ReturnUrl();
			};

			return ReturnUrl();
		}
		[Route("Sensors/DropSensorsData")]
		[HttpPost]
		public ActionResult DropSensorsData(MongoSensorsListModel postData)//post
		{
			var DeleteResult = SensorsCollection.DeleteOne(Builders<MongoSensorsListModel>.Filter.Eq("sensorId", postData.sensorId));
			if (DeleteResult.IsAcknowledged)
			{
				if (DeleteResult.DeletedCount != 1)
				{
					return ReturnUrl();

					throw new Exception(string.Format("Count [value:{0}] after delete is invalid", DeleteResult.DeletedCount));
				
				}
			}
			else {
				return ReturnUrl();

				throw new Exception(string.Format("Delete for [_id:{0}] was not acknowledged", postData.sensorId.ToString()));

			}
			return ReturnUrl();
		}
		[Route("Sensors/DropSensors/{id}")]
		public ActionResult DropSensors(string id)//display Drop sensors form
		{
			//getdb();
			List<SensorsListModel> list = GetSensorsData();
			list = list.Where(x => x.sensorId.Contains(id)).ToList();

			ViewData["EditSensorsListModel"] = list;
			ViewBag.viewType = "Drop";
			ViewBag.action = "DropSensorsData";

			return PartialView("_AddSensors", list);
		}
		//form
		public List<SensorsListModel> FindSensors(List<SensorsListModel> sensorsDataList)
		{

			//Debug.WriteLine("FindSensors.ViewData[roomID] = " + ViewData["roomID"]);
			//List<SensorsListModel> sensorsDataList = SensorsDataList;
			List<SensorsListModel> newSearchedlist = new List<SensorsListModel> { };
			//sensorsDataList = SensorsDataList;// database.GetCollection<SensorsListModel>("SENSOR_LIST").Find(_ => true).ToList();
			Debug.WriteLine("sensorsDataList! = " + sensorsDataList.ToJson().ToString());

				if (Request.Query.Keys.Count() > 0 ) {//search 
												 //search sensor
				var roomid = ""; //get room id if have
				if (Request.Query.ContainsKey("roomId"))
				{
					if (Request.Query["roomId"] != string.Empty || Request.Query["roomId"] != "")
					{
						roomid = Request.Query["roomId"];
					}
				}
				var sen_id = "";//get sensor id if have
				if (Request.Query.ContainsKey("sensorId"))
				{
					if (Request.Query["sensorId"] != string.Empty || Request.Query["sensorId"] != "")
					{
						sen_id = Request.Query["sensorId"];
					}
				}


				//get what type need to search if have
				List<string> typeList = new List<string>();
				//record the check box
				string chkStr = "checked";
				List<string> typeCheckedList = new List<string>() { "", "", "", "" };//last one is all checked
				Debug.WriteLine("Request.Query.Keys.Count() = " + Request.Query.Keys.Count());
				if (Request.Query.Keys.Count() > 2)
				{
					foreach (string key in Request.Query.Keys)
					{
						Debug.WriteLine("key! = " + key);
						switch (key)
						{
							case "TS":
								typeCheckedList[0] = chkStr;
								typeList.Add(key);
								break;
							case "HS":
								typeCheckedList[1] = chkStr;
								typeList.Add(key);
								break;
							case "LS":
								typeCheckedList[2] = chkStr;
								typeList.Add(key);
								break;
							default:
								break;
						}
					}
				}

				//var filter = Builders<SensorsListModel>.Filter.Regex("roomId", new BsonRegularExpression(".*"+ roomid+".*"));
				if (!Request.Query.ContainsKey("All"))//all sensor type
				{

					//if not all type need check which type the user selected.
					foreach (string stype in typeList)
					{
						newSearchedlist.AddRange(sensorsDataList.Where(ls => ls.sensorId.Contains(stype)).ToList());
					}
					Debug.WriteLine("Not all! = " + newSearchedlist.ToJson().ToString());
				} else {
					typeCheckedList[3] = chkStr;
					//if all type, then get all sensor type
					newSearchedlist = sensorsDataList;
				}

				if (!roomid.Equals(" ") || roomid.Length > 0)
				{
					//search by room id
					newSearchedlist = newSearchedlist.Where(ls => ls.roomId.Contains(roomid)).ToList();
				}

				if (!sen_id.Equals(" ") || sen_id.Length > 0)
				{
					//search by sensor id
					newSearchedlist = newSearchedlist.Where(ls => ls.sensorId.Contains(sen_id)).ToList();
				}

				ViewData["searchedRoomId"] = roomid;
				ViewData["searchedSensorId"] = sen_id;
				ViewData["typeCheckedList"] = typeCheckedList;
			}
			else {//not search 
				  // senosrs page 1st load 
				Debug.WriteLine("only 1st load");
				newSearchedlist = sensorsDataList;
			}
				
			

			return newSearchedlist;
		}


        public List<SensorsListModel> SortList(List<SensorsListModel> DataList)
		{
			if (HttpContext!=null) { 
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
		public List<SensorsListModel> GetAllSensors()
		{
			List<SensorsListModel> SensorsDataList = new List<SensorsListModel> { };

			IQueryable<MongoSensorsListModel> query= from c in SensorsCollection.AsQueryable<MongoSensorsListModel>() select c;
			if (PageRoomId.Length == 0)
			{
				query = from c in SensorsCollection.AsQueryable<MongoSensorsListModel>() select c;
			}
			else
			{//Sensors/{id}
				query = from c in SensorsCollection.AsQueryable<MongoSensorsListModel>() where c.roomId.Contains(PageRoomId) select c;

			}
			if (query.Count()>0) { 
				foreach (var set in query.ToList())
				{
					var data = new SensorsListModel()
					{
						roomId = set.roomId,
						sensorId = set.sensorId,
						sensor_name = set.sensor_name,
						pos_x = set.pos_x,
						pos_y = set.pos_y,
						desc = set.desc,
						latest_checking_time = set.latest_checking_time,
						total_run_time = set.total_run_time,
						current_Value = Convert.ToDouble(GetSensorCurrentValue(set.sensorId)),
						current_Time = Convert.ToDateTime(GetSensorCurrentDate(set.sensorId)),
						typeImg = GetType(set.sensorId),
						typeUnit = Getunit(set.sensorId),
						Exception = set.Exception,
						status=set.status
	};
					SensorsDataList.Add(data);
				}
			}
			return SensorsDataList;
		}
		public List<SensorsListModel> GetSensorsData()
		{
			List<SensorsListModel> SensorsDataList = GetAllSensors();
			if (Request != null) {
				Debug.WriteLine("***f*"+ Request.QueryString);
				SensorsDataList = FindSensors(SensorsDataList);
			}
			SensorsDataList = SortList(SensorsDataList);

				
			//Debug.WriteLine("\n\n     last list ");
			//Debug.WriteLine(SensorsDataList.ToJson().ToString()+" \n\n");

			return SensorsDataList;
		}
		public List<RoomsListModel> GetRoomData()
		{
			//getdb();
			var RoomDataList = new List<RoomsListModel> { };
			//IMongoCollection<RoomsListModel> collection = database.GetCollection<RoomsListModel>("ROOM");

			// sorting

			var sort = Builders<RoomsListModel>.Sort.Ascending("roomId");

			//end sorting

			var RoomsDocuments = RoomCollection.Find(new BsonDocument()).Sort(sort);

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
		public string Getunit(string sensorId)
		{
			string type = "";
			switch (sensorId.Substring(0, 2))
			{
				case "TS":
					type = "°C";
					break;
				case "LS":
					type = "";
					break;
				case "HS":
					type = "%";
					break;
				case "AS":
					type = "µm";
					break;
				default:
					break;
			}
			return type;
		}
		public string GetType(string sensorId)
		{
			string type = "";
			switch (sensorId.Substring(0, 2))
			{
				case "TS":
					type = "temperature96.png";
					break;
				case "LS":
					type = "lightSensor96.png";
					break;
				case "HS":
					type = "humidity96.png";
					break;
				case "AS":
					type = "aqSensor96.png";
					break;
				default:
					type = "light-off96";
					break;
			}
			return type;
		}
		public List<CurrentDataModel> GetSensorIDCurrentList(string sensorId)
		{
			string tableName = "";
			List<CurrentDataModel> List = new List<CurrentDataModel>();
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
			List<CurrentDataModel> query = new List<CurrentDataModel>();
			if (!string.IsNullOrEmpty(tableName))
			{
				var filter = Builders<CurrentDataModel>.Filter.Eq(x => x.sensorId, sensorId);

				query = database.GetCollection<CurrentDataModel>(tableName).Find(filter).Limit(1).Sort(Builders<CurrentDataModel>.Sort.Descending("latest_checking_time")).ToList();
			}
			//IQueryable<CurrentDataModel> query;
			//query = from c in collection.AsQueryable<CurrentDataModel>() orderby c.latest_checking_time descending where c.sensorId.Contains(sensorId) select c;
			return query;
		}
		public CurrentDataModel GetSensorIDCurrent(string sensorId)
		{
			string tableName = "";
			List<CurrentDataModel> List = new List<CurrentDataModel>();
			CurrentDataModel query = new CurrentDataModel();

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
				case "AS":
					tableName = "";
					var filter = Builders<MongoASModel>.Filter.Eq(x => x.sensorId, sensorId);
					MongoASModel asquery = new MongoASModel();

					asquery = new DBManger().DataBase.GetCollection<MongoASModel>("AS_SENSOR").Find(filter).Limit(1).Sort(Builders<MongoASModel>.Sort.Descending("latest_checking_time")).FirstOrDefault();
					query = new CurrentDataModel {
                        sensorId= asquery.sensorId,
                        current= asquery.C_AVG_PM25,
                        latest_checking_time=asquery.latest_checking_time
					};
					break;
				default:
					tableName = "";
					break;
			}
			Debug.WriteLine("^^^^^"+tableName);
			//db collection
			if (!string.IsNullOrEmpty(tableName) && tableName!="AS") {
				var filter = Builders<CurrentDataModel>.Filter.Eq(x => x.sensorId, sensorId);

				query = database.GetCollection<CurrentDataModel>(tableName).Find(filter).Limit(1).Sort(Builders<CurrentDataModel>.Sort.Descending("latest_checking_time")).FirstOrDefault();
			}
			//IQ ueryable<CurrentDataModel> query;
			//query = from c in collection.AsQueryable<CurrentDataModel>() orderby c.latest_checking_time descending where c.sensorId.Contains(sensorId) select c;
			return query;
		}
		public double GetSensorCurrentValue(string sensorId)
		{
			double value = GetCurrentValueByidBytable(sensorId);
				
			return value;
		}

		public DateTime GetSensorCurrentDate(string sensorId)
		{
			DateTime value = new DateTime();


					value = GetCurrentDateByidBytable(sensorId);

			return value;

		}
		public double GetCurrentValueByidBytable(string sensorId)
		{

			var SensorsDataList = GetSensorIDCurrent(sensorId);
			if (SensorsDataList!=null)
			{
				return Convert.ToDouble(SensorsDataList.current);
			}
			else
			{
				return 0;
			}
		}
		public DateTime GetCurrentDateByidBytable(string sensorId)
		{
			CurrentDataModel SensorsDataList = GetSensorIDCurrent(sensorId);
			if (SensorsDataList==null)
			{
				return default;
			}
			else { 
			return Convert.ToDateTime(SensorsDataList.latest_checking_time);
			}
		}
		public string ChartData(List<SensorsListModel> SensorsDataList, string type)
		{
			switch (type)
			{
				case "TS":
					SensorsDataList = SensorsDataList.Where(x => x.sensorId.Contains("TS")).ToList();
					ViewBag.unit = " C";
					ViewBag.unitName = "Temperature";
					break;
				case "LS":
					SensorsDataList = SensorsDataList.Where(x => x.sensorId.Contains("LS")).ToList();
					ViewBag.unit = " ";
					ViewBag.unitName = "Luminosity";
					break;
				case "HS":
					SensorsDataList = SensorsDataList.Where(x => x.sensorId.Contains("HS")).ToList();
					ViewBag.unit = " %";
					ViewBag.unitName = "Humidity";
					break;
				case "AS":
					ViewBag.unit = " µm";

					
					break;
				default:
					break;
			}
			return GetChartData(SensorsDataList);
		}

		public string GetChartData(List<SensorsListModel> SensorsDataList)
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
				Color.Add(chart.GetRandomColor());
				labelss.Add(get.sensorId);
				SensorsCurrentList = GetSensorIDCurrentList(get.sensorId).Where(x => x.latest_checking_time > today.AddDays(-1)).OrderBy(x => x.latest_checking_time).ToList();

				DateTime ca = today;
				TimeSpan catime = ca - ca.AddHours(-6);

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
						
						ca = DateTime.Now.AddHours(-6);

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

			for (int i = 0; i < SensorsDataList.Count; i++)
			{
				labelss.Add(SensorsDataList[i].sensorId);
			}

			return chart.LineChart(SensorsDataList.Count, labelss, datas );
		}
	
	}
}