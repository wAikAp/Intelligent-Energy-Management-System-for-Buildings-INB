using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP_WEB_APP.Controllers.Mongodb;
using FYP_WEB_APP.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FYP_APP.Controllers
{
    public class SensorsController : Controller
    {
		public ActionResult Sensors(string sortOrder)
		{
			if (sortOrder is null)
			{
				sortOrder = "";
			}

			ConnectDB conn = new ConnectDB();
			IMongoDatabase database = conn.Conn();

			ViewData["SensorsListModel"] = GetSensorsData(database);
			ViewData["RoomListModel"] = GetRoomData(database);

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
					if (!rk.Equals("false") && !key.Equals("sortOrder"))
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
			sortOrder = ChangeLink(sortOrder);

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
		public string ChangeLink(string sortOrder)
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
	}
}