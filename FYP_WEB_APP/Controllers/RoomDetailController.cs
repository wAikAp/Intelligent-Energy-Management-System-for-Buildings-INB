using System;
using System.Collections.Generic;
using System.Diagnostics;
using FYP_APP.Controllers;
using FYP_WEB_APP.Models;
using FYP_WEB_APP.Models.MongoModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FYP_WEB_APP.Controllers
{
    public class RoomDetailController : Controller
    {
        //[Route("RoomDetail/RoomDetail/{roomID}")]
        public IActionResult RoomDetail(String roomID)
        {
            ViewData["roomID"] = roomID;
            SensorsController sensorsController = new SensorsController();
            List<SensorsListModel> sensorsList = sensorsController.GetSensorsListByRoomid(roomID);
            DevicesController DevicesController = new DevicesController();
            List<DevicesListModel> devicesList = DevicesController.GetDevicesListByRoomid(roomID);

            //Debug.WriteLine("sensor list = "+sensorsList.ToJson().ToString());
            //Debug.WriteLine("Devices list = " + deviceslist.ToJson().ToString());
            ViewData["sensorsList"] = sensorsList;
            ViewData["devicesList"] = devicesList;

            return View();
        }

        [HttpPost]
        public IActionResult AddDragButton(IFormCollection post) {
            Debug.WriteLine("sensorID = " + post["sensor_type"] + post["sensorNo"]);
            ViewData["roomID"] = post["roomID"];
            Debug.WriteLine("AddDragButton room id = " + ViewData["roomID"]);
            return Redirect("RoomDetail?roomID="+ ViewData["roomID"]+"#floorPlan");
        }

        [Route("RoomDetail/UpdateSensorDevicePosition")]
        [HttpPost]
		public static string UpdateSensorDevicePosition(string userdata)
		{
			
			Debug.WriteLine("post!");
			//getdb();

			//var collection = database.GetCollection<MongoSensorsListModel>("SENSOR_LIST");
			/*
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

						}
						var Updated = SensorsCollection.UpdateOne(filter, up);
						this.isUpdated = Updated.IsAcknowledged;
					}
				}
			}
            */
			return "posted";
		}

	}
}
