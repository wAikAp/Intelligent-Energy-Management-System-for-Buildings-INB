using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
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
		private string PageRoomId="";
		private bool isUpdated;
		public void getdb() { 
		ConnectDB conn = new ConnectDB();
			this.database = conn.Conn();
		}
		[Route("Sensors/")]
		[Route("Sensors/Sensors")]
		public ActionResult Sensors()
		{

			ViewBag.SearchRoomIdENorDisable = "";
			getdb();
			ViewData["SensorsListModel"] = Setgroup(GetSensorsData());

			ViewData["RoomListModel"] = GetRoomData();

			chartData(GetSensorsData());

			return View();
		}
		[Route("Sensors/Sensors/{id}")]
		public ActionResult Sensors(string id)
		{
			ViewBag.roomID =this.PageRoomId = id;
			ViewBag.SearchRoomIdENorDisable = "disabled";
			getdb();

			ViewData["SensorsListModel"] = Setgroup(GetSensorsData());

			ViewData["RoomListModel"] = GetRoomData(id);


			chartData(GetSensorsData());

			return View();
		}
		[Route("Sensors/EditSensors/{id}")]
		public ActionResult EditSensors(string id)
		{
			getdb();
			List<SensorsListModel> list = GetSensorsData();
			list = list.Where(x => x.sensorId.Contains(id)).ToList();
			ViewData["EditSensorsListModel"] = list;
			ViewBag.sid = id;
			ViewBag.viewType = "Edit";
			ViewBag.action = "UpdateSensors";
			return PartialView("_AddSensors", list);
		}
		[Route("Sensors/UpdateSensors")]
		[HttpPost]
		public ActionResult UpdateSensors(MongoSensorsListModel postData)
		{

			getdb();

			var collection=database.GetCollection<MongoSensorsListModel>("SENSOR_LIST");
			var filter = Builders<MongoSensorsListModel>.Filter.Eq("sensorId", postData.sensorId);

			var type = postData.GetType();
			var props = type.GetProperties();

			foreach (var property in props)
			{
				if (!property.Name.Equals("_id") ) {
					if (property.GetValue(postData) != null) {
						UpdateDefinition<MongoSensorsListModel> up;
						if (property.Name=="latest_checking_time")
						{
							up = Builders<MongoSensorsListModel>.Update.Set(property.Name.ToString(), DateTime.UtcNow);

						}
						else {
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
		[Route("Sensors/AddSensorsData")][HttpPost]
		public ActionResult AddSensorsData(SensorsListModel postData)//post
		{
			getdb();
			var collection = database.GetCollection<MongoSensorsListModel>("SENSOR_LIST");

			string sensortype = "";

			MongoSensorsListModel insertList = new MongoSensorsListModel { } ;

			var all=GetSensorsData();
			string count = "";
			if (all.Count < 10) { count = "00" + all.Count.ToString(); }
			else if (all.Count < 100) { count = "0" + all.Count.ToString(); }

			sensortype = postData.Sensortype;
			insertList.roomId = postData.roomId;
			insertList.sensorId = postData.Sensortype+ count;
			insertList.location = postData.location;
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
			ViewBag.sid = id;
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
					if (skey !="sortOrder")
					{
						EndDataList.AddRange(FDataList);//B list add in A list

					EndDataList = EndDataList.Distinct().ToList();//delet double data

				}
			}

			//get B & A list Intersect data
			if (roomSensorsDataList.Count >0)
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
		public List<SensorsListModel> SortList(List<SensorsListModel>DataList)
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
				.Select(grp =>  grp.ToList() )
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
			if (PageRoomId.Length==0) {
				 query = from c in collection.AsQueryable<SensorsListModel>() select c;
			}
			else
			{//Sensors/{id}
				query = from c in collection.AsQueryable<SensorsListModel>()where c.roomId.Contains(PageRoomId) select c;
			}

			foreach (SensorsListModel set in query)
				{
				var data = new SensorsListModel()
				{
					roomId = set.roomId,
					sensorId = set.sensorId,
					location=set.location,
					desc=set.desc,
					latest_checking_time = set.latest_checking_time,
					total_run_time = set.total_run_time,
					current_Value = getSensorCurrentValue(set.sensorId),
					typeImg = getType(set.sensorId),
					typeUnit = getunit(set.sensorId)
					};
					SensorsDataList.Add(data);

				
			}
				try
				{
				int count = Request.Query.Count;
				if (count != null)
				{
					SensorsDataList = FindSensors(SensorsDataList);
					SensorsDataList = SortList(SensorsDataList);
				}
				else
				{
					SensorsDataList = SortList(SensorsDataList);
				}
			}
				catch (NullReferenceException e)
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
				rs = Request.QueryString.ToString().Substring(1);


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
		public string getType(string sensorId) {
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
		public double getSensorCurrentValue(string sensorId) {
			
			IMongoCollection<BsonDocument> collection;
			FilterDefinition<BsonDocument> filter;
			double value = 0;
			var dbStr ="";
			switch (sensorId.Substring(0, 2))
			{
				case "TS":
					dbStr = "TMP_SENSOR";
					break;
				case "LS":
					dbStr = "LIGHT_SENSOR";
					break;
				case "HS":
					dbStr = "HUM_SENSOR";
					break;
				default:
					break;
			}
			collection = database.GetCollection<BsonDocument>(dbStr);
			filter = Builders<BsonDocument>.Filter.Eq("sensorId", sensorId);

			var json = collection.Find(filter).FirstOrDefault();
			if (json != null)
			{
				switch (sensorId.Substring(0, 2))
			{
				case "TS":
						value = Convert.ToDouble(json["current_tmp"]);
						break;
				case "LS":
						value = Convert.ToDouble(json["current_lum"]);
						break;
				case "HS":
						value= Convert.ToDouble(json["current_hum"]);
						break;
					default:
						break;
				}			
			}
			return value;
		}
		//chart js code
		List<string> Color = new List<string>();

		public void chartData(List<SensorsListModel> SensorsDataList) {
			List<string> labelss = new List<string>();
			List<string> datas = new List<string>();
			List<JObject> datasets = new List<JObject>();
			int[] x1 = { 65, 59, 80, 81, 56, 55, 40, 50, 60, 55, 30, 78 };

			int[] x2 = { 10, 20, 60, 95, 64, 78, 90, 80, 70, 40, 70, 89 };

			int[] x3 = { 65, 59, 80, 81, 56, 55, 40, 50, 60, 55, 30, 78 };

			int[] x4 = { 50, 59, 70, 71, 56, 55, 45, 55, 60, 50, 30, 50 };
			int[] x5 = { 50, 59, 70, 71, 56, 55, 45, 55, 60, 50, 30, 50 };
			int[] x6 = { 50, 59, 70, 71, 56, 55, 45, 55, 60, 50, 30, 50 };

			datas.Add(x1.ToJson());
			datas.Add(x2.ToJson());
			datas.Add(x3.ToJson());
			datas.Add(x4.ToJson());
			datas.Add(x5.ToJson());
			datas.Add(x6.ToJson());



			ArrayList day = new ArrayList();

			for (int i = 0; i< 30; i++) {
                day.Add(i+1);
            }

			for (int i = 0; i < SensorsDataList.Count; i++)
			{	
				getRandomColor();
				

				labelss.Add(SensorsDataList[i].sensorId);
			}

			for (int i = 0; i < SensorsDataList.Count; i++)
			{
			 var json = "{ 'label':'"+ labelss[i]+
					  "','borderColor':'" +  Color[i]+
					  "','fill': false,'spanGaps': false," +
					  "'data':"+  datas[i]+"}" ;
				JObject jObj = JObject.Parse(json);

				datasets.Add(jObj);
			}
			ViewBag.datasets = JsonConvert.SerializeObject(datasets);
		}
		public void getRandomColor()
		{
			var random = new Random();
			var rmcolor = String.Format("#{0:X6}", random.Next(0x1000000));
			Color.Add(rmcolor);
		}
		public void alert(int type,string title,string str) {
			switch (type) {
				case -1:
					ViewBag.alert_type = "alert-danger";
					break;
				case 0:
					ViewBag.alert_type = "alert-warning";
					break;
				case 1:
					ViewBag.alert_type = "alert-success";
					break;

			}
			ViewBag.title = title;
			ViewBag.str = str;
		} 
	}
}