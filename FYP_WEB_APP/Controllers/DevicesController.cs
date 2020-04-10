using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using FYP_APP.Models.MongoModels;
using FYP_WEB_APP.Controllers.Mongodb;
using FYP_WEB_APP.Models.MongoModels;

namespace FYP_APP.Controllers
{
	public class DevicesController : Controller
	{
		public List<MongoDeivcesListModel> MongoDeivcesList = new List<MongoDeivcesListModel> { };

		public IActionResult Devices()
		{
			getAlldeivces();
			   ViewData["MongoDeivcesListModel"] = this.MongoDeivcesList;


			return View();
		}
		public IMongoCollection<MongoDeivcesListModel> getMDLMconn() {
			ConnectDB conn = new ConnectDB();
			var database = conn.Conn();
			var collection = database.GetCollection<MongoDeivcesListModel>("DEVICES_LIST");
			return collection;
		}
		public void getAlldeivces() {
			var collection = getMDLMconn();
			IQueryable<MongoDeivcesListModel> query = from d in collection.AsQueryable<MongoDeivcesListModel>() select d;

			foreach (MongoDeivcesListModel set in query.ToList())
			{
				//Debug.WriteLine(ll.lastest_checking_time);
				//Debug.WriteLine(ll.roomId);
				var data = new MongoDeivcesListModel()
				{					
					roomId = set.roomId,
					devicesId=set.devicesId,
					devices_Name= set.devices_Name,
					power = set.power,
					lastest_checking_time = set.lastest_checking_time,
					total_run_time = set.total_run_time
				};
				this.MongoDeivcesList.Add(data);
			}
		}

	}
}