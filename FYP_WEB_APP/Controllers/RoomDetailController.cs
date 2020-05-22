using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using FYP_APP.Controllers;
using FYP_APP.Models.MongoModels;
using FYP_WEB_APP.Models;
using FYP_WEB_APP.Models.LogicModels;
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
        private readonly IMongoCollection<BsonDocument> ROOMCOLLECTION = dBManger.DataBase.GetCollection<BsonDocument>("ROOM");

        //[Route("RoomDetail/RoomDetail/{roomID}")]
        public IActionResult RoomDetail(String roomID)
        {
            ViewData["roomID"] = roomID;
            //get sensor and device list
            SensorsController sensorsController = new SensorsController();
            List<SensorsListModel> sensorsList = sensorsController.GetSensorsListByRoomid(roomID);
            DevicesController DevicesController = new DevicesController();
            List<DevicesListModel> devicesList = DevicesController.GetDevicesListByRoomid(roomID);

            //power list
            DevicesPowerUseOutputUtil devicesPowerUseOutputUtil = new DevicesPowerUseOutputUtil();
            List<MongoDevicesPowerUse> powerUsageList = devicesPowerUseOutputUtil.getPowerUseList();

            double powerSum = 0;//sum power usage of this room
            foreach (MongoDevicesPowerUse mpu in powerUsageList) {
                powerSum += mpu.power_used;
            }

            //get room model
            var collection = dBManger.DataBase.GetCollection<MongoRoomModel>("ROOM");
            var roomList = collection.Find(x => x.roomId == roomID).ToList();

            //Debug.WriteLine("roomList list = " + roomList.ToJson().ToString());
            if (roomList.Count > 0)
            {
                ViewData["roomModel"] = roomList[0];
            }
            else {
                ViewData["roomModel"] = new MongoRoomModel();
            }
            
            
            //Debug.WriteLine("Devices list = " + deviceslist.ToJson().ToString());
            ViewData["sensorsList"] = sensorsList;
            ViewData["devicesList"] = devicesList;
            ViewData["powerUsageList"] = powerUsageList;
            ViewData["sumPowerUsage"] = powerSum;
            Debug.WriteLine("powerUsageList list = " + powerUsageList.ToJson().ToString());
            return View();
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

                    //Debug.WriteLine("update result: " + result);
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

                    //Debug.WriteLine("device update result: " + result);
                }

            }
            catch (Exception ex) {
                return Json("Error:"+ex);
            }
		
			return Json("Success");
		}
        public IActionResult uploadFloorPlan(IFormCollection postFrom) {
            ViewData["roomID"] = postFrom["roomID"];
            string floorPlanBase64 = postFrom["floorPlanBase64"];
            try
            {
                //base 64 img save to db
                //ROOMCOLLECTION
                var filter = Builders<BsonDocument>.Filter.Eq("roomId", postFrom["roomId"]);
                UpdateDefinition<BsonDocument> updteFields = Builders<BsonDocument>.Update.Set("floorPlanImg", floorPlanBase64);
                var result = ROOMCOLLECTION.FindOneAndUpdate(filter, updteFields);
                if (result != null)
                {
                    ViewData["uploadStatus"] = true;
                }
                else {
                    ViewData["uploadStatus"] = false;
                }
            }
            catch (Exception ex) {
                ViewData["uploadStatus"] = false;
                Debug.WriteLine("upload floor plan exception:"+ ex);
            }

            
            return Redirect("RoomDetail?roomID=" + ViewData["roomID"] + "#floorPlan");
        }


    }
}
