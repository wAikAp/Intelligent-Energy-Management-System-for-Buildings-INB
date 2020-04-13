using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using FYP_APP.Models.MongoModels;
using FYP_WEB_APP.Controllers.Mongodb;
using FYP_WEB_APP.Models.MongoModels;
using FYP_WEB_APP.Models;

namespace FYP_APP.Controllers
{
	public class DevicesController : Controller
	{
		public List<MongoDevicesListModel> MongoDevicesList = new List<MongoDevicesListModel> { };

		public IActionResult Devices()
		{
			getAllDevices();
			   ViewData["MongoDevicesListModel"] = this.MongoDevicesList;


			return View();
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
			getAllDevices();

			//List<MongoDevicesListModel> list = new List<MongoDevicesListModel>{ };
			ViewData["EditDevicesListModel"] = MongoDevicesList;
			ViewBag.sid = id;
			ViewBag.viewType = "Edit";
			ViewBag.action = "UpdateDevicesData";
			return PartialView("_AddDevices");
		}
		[Route("Devices/DropDevices/{id}")]
		public ActionResult DropDevices(string id)//display Drop sensors form
		{
			//List<MongoDevicesListModel> list = new List<MongoDevicesListModel> { };
			getAllDevices();

			ViewData["EditDevicesListModel"] = MongoDevicesList;
			System.Diagnostics.Debug.WriteLine(MongoDevicesList.ToJson().ToString());
			ViewBag.sid = id;
			ViewBag.viewType = "Drop";
			ViewBag.action = "DropDevicesData";


			return PartialView("_AddDevices");
		}
		[Route("Devices/UpdateDevices")]
		[HttpPost]
		public ActionResult UpdateDevices(MongoDevicesListModel postData)
		{
			return RedirectToAction("Devices");
		}
		
		[Route("Devices/AddDevicesData")]
		[HttpPost]
		public ActionResult AddDevicesData(MongoDevicesListModel postData)//post
		{
			
			return RedirectToAction("Devices");
		}
		[Route("Devices/DropDevicesData")]
		[HttpPost]
		public ActionResult DropDevicesData(MongoDevicesListModel postData)//post
		{

			//var DeleteResult = getMDLMconn().DeleteOne(Builders<MongoDevicesListModel>.Filter.Eq("DevicesId", postData.devicesId));

			return RedirectToAction("Devices");
		}


		public IMongoCollection<MongoDevicesListModel> getMDLMconn() {
			ConnectDB conn = new ConnectDB();
			var database = conn.Conn();
			var collection = database.GetCollection<MongoDevicesListModel>("DEVICES_LIST");
			return collection;
		}
		public void getAllDevices() {
			var collection = getMDLMconn();
			IQueryable<MongoDevicesListModel> query = from d in collection.AsQueryable<MongoDevicesListModel>() select d;
			MongoDevicesList = query.ToList();
			/*foreach (MongoDevicesListModel set in )
			{
				//Debug.WriteLine(ll.lastest_checking_time);
				//Debug.WriteLine(ll.roomId);
				var data = new MongoDevicesListModel()
				{					
					roomId = set.roomId,
					devicesId=set.devicesId,
					devices_Name= set.devices_Name,
					power = set.power,
					lastest_checking_time = set.lastest_checking_time,
					total_run_time = set.total_run_time
				};
				this.MongoDevicesList.Add(data);
		}*/
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