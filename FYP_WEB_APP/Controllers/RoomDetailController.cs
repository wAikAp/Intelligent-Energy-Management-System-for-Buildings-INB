using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using FYP_APP.Controllers;
using FYP_WEB_APP.Models;
using FYP_WEB_APP.Models.MongoModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FYP_WEB_APP.Controllers
{
    public class RoomDetailController : Controller
    {
        private static DBManger dBManger = new DBManger();
        private readonly IMongoCollection<BsonDocument> SENSORCOLLECTION = dBManger.DataBase.GetCollection<BsonDocument>("SENSOR_LIST");
        private readonly IMongoCollection<BsonDocument> DEVICECOLLECTION = dBManger.DataBase.GetCollection<BsonDocument>("DEVICES_LIST");

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
			Debug.WriteLine("AddDragButton");
            //Debug.WriteLine("sensorID = " + post["sensor_type"] + post["sensorNo"]);
            ViewData["roomID"] = post["roomID"];
            //Debug.WriteLine("AddDragButton room id = " + ViewData["roomID"]);
            return Redirect("RoomDetail?roomID="+ ViewData["roomID"]+"#floorPlan");
        }



        [Route("RoomDetail/UpdateSensorDevicePosition")]
        [HttpPost]
		public JsonResult UpdateSensorDevicePosition(IFormCollection postFrom)
		{
            
            try
            {
                //data sample
                //[{"id":"HS001","pos_x":100.09375,"pos_y":100.1875},{"id":"TS001","pos_x":1,"pos_y":1},{"id":"TS002","pos_x":1,"pos_y":1},{"id":"LS001","pos_x":10,"pos_y":10},{"id":"TS003","pos_x":0,"pos_y":0}]
                Dictionary<string, string>[] sensorJSONObjectList = JsonConvert.DeserializeObject<Dictionary<string, string>[]>(postFrom["sPositionList"]);
                Dictionary<string, string>[] deviceJSONObjectList = JsonConvert.DeserializeObject<Dictionary<string, string>[]>(postFrom["dPositionList"]);
                string pos_x = "pos_x";
                string pos_y = "pos_y";

                //update sensor position to sensor list table
                for (int i = 0; i < sensorJSONObjectList.Length; i++)
                {
                    //Debug.WriteLine(sensorJSONObject[i]["id"] + "/" + sensorJSONObject[i]["pos_x"] + "," + sensorJSONObject[i]["pos_y"]);
                    var filter = Builders<BsonDocument>.Filter.Eq("sensorId", sensorJSONObjectList[i]["id"]);
                    UpdateDefinition<BsonDocument> updteFields = Builders<BsonDocument>.Update
                        .Set(pos_y, double.Parse(sensorJSONObjectList[i]["pos_y"]))
                        .Set(pos_x, double.Parse(sensorJSONObjectList[i]["pos_x"]));
                    var result = SENSORCOLLECTION.FindOneAndUpdate(filter, updteFields);

                    Debug.WriteLine("update result: " + result);
                }

                //update device position to device list table
                for (int i = 0; i < deviceJSONObjectList.Length; i++)
                {
                    //Debug.WriteLine(sensorJSONObject[i]["id"] + "/" + sensorJSONObject[i]["pos_x"] + "," + sensorJSONObject[i]["pos_y"]);
                    var filter = Builders<BsonDocument>.Filter.Eq("devicesId", deviceJSONObjectList[i]["id"]);
                    UpdateDefinition<BsonDocument> updteFields = Builders<BsonDocument>.Update
                        .Set(pos_y, double.Parse(deviceJSONObjectList[i]["pos_y"]))
                        .Set(pos_x, double.Parse(deviceJSONObjectList[i]["pos_x"]));
                    var result = DEVICECOLLECTION.FindOneAndUpdate(filter, updteFields);

                    Debug.WriteLine("device update result: " + result);
                }

            }
            catch (Exception ex) {
                return Json("Error:"+ex);
            }
			

            

			return Json("Success");
		}

	}
}
