using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using FYP_WEB_APP.Controllers.Mongodb;
using FYP_WEB_APP.Models;
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
		private string PageRoomId;

		public ActionResult Sensors()
		{
			PageRoomId = "";
			ViewBag.SearchRoomIdENorDisable = "";
			ConnectDB conn = new ConnectDB();
			this.database = conn.Conn();
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
			ConnectDB conn = new ConnectDB();
			this.database = conn.Conn();
			ViewData["SensorsListModel"] = Setgroup(GetSensorsData());

			ViewData["RoomListModel"] = GetRoomData(id);


			chartData(GetSensorsData());

			return View();
		}
		[Route("Sensors/EditSensors")]
		public ActionResult EditSensors()
		{

			return PartialView("_AddSensors");
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
						latest_checking_time = set.latest_checking_time,
						current_Value = getSensorCurrentValue(set.sensorId),
						typeImg = getType(set.sensorId),
						typeUnit = getunit(set.sensorId)
					};
					SensorsDataList.Add(data);

				
			}
			if (Request.Query.Count > 0)
			{

				SensorsDataList = FindSensors(SensorsDataList);
				SensorsDataList = SortList(SensorsDataList);
			}
			else
			{
				SensorsDataList = SortList(SensorsDataList);
			}

			return SensorsDataList;
		}
		public List<RoomsListModel> GetRoomData()
		{
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
			IFindFluent<BsonDocument, BsonDocument> FindSensorsDocuments;
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
			string sdatasets = "";
			int[] x1 = { 65, 59, 80, 81, 56, 55, 40, 50, 60, 55, 30, 78 };

			int[] x2 = { 10, 20, 60, 95, 64, 78, 90, 80, 70, 40, 70, 89 };

			int[] x3 = { 65, 59, 80, 81, 56, 55, 40, 50, 60, 55, 30, 78 };

			int[] x4 = { 50, 59, 70, 71, 56, 55, 45, 55, 60, 50, 30, 50 };
			int[] x5 = { 50, 59, 70, 71, 56, 55, 45, 55, 60, 50, 30, 50 };

			datas.Add(x1.ToJson());
			datas.Add(x2.ToJson());
			datas.Add(x3.ToJson());
			datas.Add(x4.ToJson());
			datas.Add(x5.ToJson());


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
	}
}