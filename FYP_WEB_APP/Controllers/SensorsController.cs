using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
		public ActionResult Sensors()
		{
			ConnectDB conn = new ConnectDB();
			IMongoDatabase database = conn.Conn();

			ViewData["SensorsListModel"] = GetSensorsData(database);
			ViewData["RoomListModel"] = GetRoomData(database);
			chartData(GetSensorsData(database));

			return View();
		}
		public FilterDefinition<SensorsListModel> SearchSensors()
		{
			FilterDefinition<SensorsListModel> filter = null;
			bool isFirstFilter = false;
			if (!string.IsNullOrWhiteSpace(Request.ToString()))
			{
				foreach (String key in Request.Query.Keys)
				{
					string rk = Request.Query[key];
					if (!rk.Equals("false") && !key.Equals("sortOrder") && !key.Equals("All"))
					{
						string skey = key;
						string keyValue = Request.Query[key];
						if (isFirstFilter == false)
						{
							filter = Builders<SensorsListModel>.Filter.Eq(key, Request.Query[key]);
						}
						else
						{
							filter &= Builders<SensorsListModel>.Filter.Eq(key, Request.Query[key]);
						}
					}
				}
			}
			return filter;
		}
		public SortDefinition<SensorsListModel> SortList()
		{
			SortDefinition<SensorsListModel> sort = Builders<SensorsListModel>.Sort.Descending("latest_checking_time");
			string sortOrder = Request.Query["sortOrder"];
			sortOrder = ChangeSortLink(sortOrder);

			if (String.IsNullOrEmpty(sortOrder)) { }
			else if (sortOrder.Contains("Desc"))
			{
				sort = Builders<SensorsListModel>.Sort.Descending(sortOrder[0..^5]);
			}
			else
			{
				sort = Builders<SensorsListModel>.Sort.Ascending(sortOrder);
			}

			return sort;
		}

		public List<SensorsListModel> GetSensorsData(IMongoDatabase database)
		{

			List<SensorsListModel> SensorsDataList = new List<SensorsListModel> { };
			IFindFluent<SensorsListModel, SensorsListModel> FindSensorsDocuments;
			IMongoCollection<SensorsListModel> collection;
			FilterDefinition<SensorsListModel> filter;
			SortDefinition<SensorsListModel> sort;

			//db collection
			collection = database.GetCollection<SensorsListModel>("SENSOR_LIST");

			// sorting data

			sort = SortList();

			//end sorting

			//filter & find data

			filter = SearchSensors();

			//end filter & find data


			if (filter == null)
			{
				//not filter or find data using this get documents
				FindSensorsDocuments = collection.Find(new BsonDocument()).Sort(sort);
			}
			else
			{
				FindSensorsDocuments = collection.Find(filter).Sort(sort);
			}
			//push the data in the model
			foreach (SensorsListModel set in FindSensorsDocuments.ToList())
			{
				var data = new SensorsListModel()
				{
					roomId = set.roomId,
					sensorId = set.sensorId,
					latest_checking_time = set.latest_checking_time
				};
				SensorsDataList.Add(data);
			}

			return SensorsDataList;
		}
		public List<RoomsListModel> GetRoomData(IMongoDatabase database)
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
				ViewBag.latest_checking_timeSortParm = "?sortOrder=latest_checking_time";
				ViewBag.roomIdSortParm = "?sortOrder=roomId";
			}
			else if ((count > 1) && String.IsNullOrEmpty(sortOrder))
			{

				var latest_checking_timeSortParmButton = "?sortOrder=latest_checking_time_Desc" + "&" + rs;
				ViewBag.latest_checking_timeSortParm = latest_checking_timeSortParmButton;
				var roomidbutton = "?sortOrder=roomId_Desc" + "&" + rs;
				ViewBag.roomIdSortParm = roomidbutton;

			}
			else if (!(count > 1) && !String.IsNullOrEmpty(sortOrder))
			{
				rs = Request.QueryString.ToString().Substring(1);

				string viewsortOrderlatest = sortOrder == "latest_checking_time" ? "latest_checking_time_Desc" : "latest_checking_time";
				ViewBag.latest_checking_timeSortParm = "?sortOrder=" + viewsortOrderlatest;

				string viewsortOrderroomId = sortOrder == "roomId" ? "roomId_Desc" : "roomId";
				ViewBag.roomIdSortParm = "?sortOrder=" + viewsortOrderroomId;
			}
			else if ((count > 1) && !String.IsNullOrEmpty(sortOrder))
			{
				string viewsortOrderlatest = sortOrder == "latest_checking_time" ? "latest_checking_time_Desc" : "latest_checking_time";
				ViewBag.latest_checking_timeSortParm = "?sortOrder=" + viewsortOrderlatest + "&" + rs;

				string viewsortOrderroomId = sortOrder == "roomId" ? "roomId_Desc" : "roomId";
				ViewBag.roomIdSortParm = "?sortOrder=" + viewsortOrderroomId + "&" + rs;
			}
			//end change sorting link


			return sortOrder;
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
			datas.Add(x1.ToJson());
			datas.Add(x2.ToJson());
			datas.Add(x3.ToJson());
			datas.Add(x4.ToJson());

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
			 var json = "{ 'label':"+ labelss[i]+
					  ",'borderColor':'" +  Color[i]+
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