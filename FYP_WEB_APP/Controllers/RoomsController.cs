using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FYP_WEB_APP.Models;
using FYP_APP.Models.MongoModels;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.AspNetCore.Http;

namespace FYP_APP.Controllers
{
    public class RoomsController : Controller
    {
        private static DBManger dBManger = new DBManger();
        private readonly IMongoCollection<MongoRoomModel> ROOMCOLLECTION = dBManger.DataBase.GetCollection<MongoRoomModel>("ROOM");

        public List<MongoRoomModel> getRoomListFromDB() {//get all room from db
            List<MongoRoomModel> roomLs =  ROOMCOLLECTION.Find(_ => true).ToList();
            return roomLs;
        }

        public IActionResult Rooms()
        {
            //DBManger
            /*
            var dbManger = new DBManger();
            var collection = dbManger.DataBase.GetCollection<MongoRoomModel>("ROOM");

            var documents = collection.Find(new BsonDocument()).ToList();
            var roomsDatalist = new List<MongoRoomModel> { };

            foreach (MongoRoomModel roomModel in documents)
            {
                roomsDatalist.Add(roomModel);
            }
            //return data
            ViewData["roomsDatalist"] = roomsDatalist;
            */
            List<MongoRoomModel> roomLs = getRoomListFromDB();
            ViewData["roomsDatalist"] = roomLs;
            return View();
        }

        public IActionResult AddRoom(IFormCollection postFrom)
        {


            var roomid = postFrom["roomID"];
            var roomType = postFrom["roomType"];
            var floor = postFrom["floor"];
            var roomName = postFrom["roomName"];
            var desc = "";
            if (desc != null)
            {
                desc = postFrom["desc"];
            }


            MongoRoomModel roomModel = new MongoRoomModel();
            roomModel.roomId = roomid;
            roomModel.roomName = roomName;
            roomModel.type = roomType;
            roomModel.desc = desc;
            roomModel.floor = floor;
            roomModel.power = 0.0;
            roomModel.temp = "0";
            roomModel.hum = "0";
            roomModel.lig = "0";
            roomModel.lightListId = "0";
            roomModel.acListId = "0";
            roomModel.humListId = "0";
            roomModel.floorPlanImg = "0";
            //add to db
            try
            {
                ROOMCOLLECTION.InsertOneAsync(roomModel);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Insert new room to db error: " + ex);
                return Redirect("Rooms");
            }

            return Redirect("Rooms");

        }

        public IActionResult EditRoom(IFormCollection postFrom)
        {


            var roomid = postFrom["roomID"];
            var roomType = postFrom["roomType"];
            var floor = postFrom["floor"];
            var roomName = postFrom["roomName"];
            var desc = "";
            if (desc != null)
            {
                desc = postFrom["desc"];
            }
            if (desc != null)
            {
                desc = postFrom["desc"];
            }
            try
            {

                var filter = Builders<MongoRoomModel>.Filter.Eq("roomId", roomid.ToString());
                UpdateDefinition<MongoRoomModel> updteFields = Builders<MongoRoomModel>.Update
                    .Set("type", roomType.ToString())
                    .Set("floor", floor.ToString())
                    .Set("desc", desc.ToString())
                    .Set("roomName", roomName.ToString());
                var result = ROOMCOLLECTION.FindOneAndUpdate(filter, updteFields);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Edit room data to db error: " + ex);
                return Redirect("Rooms");
            }

            return Redirect("Rooms");

        }
        public IActionResult dropRoom(IFormCollection postFrom)
        {
            var roomid = postFrom["roomID"];
            try
            {
                var DeleteResult = ROOMCOLLECTION.DeleteOne(Builders<MongoRoomModel>.Filter.Eq("roomId", roomid));
                if (DeleteResult.IsAcknowledged)
                {
                    if (DeleteResult.DeletedCount != 1)
                    {
                        return Redirect("Rooms");
                        throw new Exception(string.Format("Count [value:{0}] after delete is invalid", DeleteResult.DeletedCount));
                    }
                }
                else
                {
                    return Redirect("Rooms");
                    throw new Exception(string.Format("Delete for [_id:{0}] was not acknowledged", roomid));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Drop room fail: " + ex);
            }
            return Redirect("Rooms");
        }

        public IActionResult searchRoom(IFormCollection postFrom)
        {
            List<MongoRoomModel> roomLs = getRoomListFromDB();
            
            var searchRoomID = postFrom["searchRoomID"];
            ViewData["searchedID"] = searchRoomID;
            if (searchRoomID.Equals("")) {
                ViewData["roomsDatalist"] = roomLs;
                return Redirect("Rooms");
            }

            roomLs = roomLs.Where(ls => ls.roomId.Contains(searchRoomID)).ToList();
            Debug.WriteLine("roomLs!! = " + roomLs.ToJson().ToString());
            ViewData["roomsDatalist"] = roomLs;
            //Session["searchedID"] = searchRoomID;
            return View("Rooms");
        }
    }
}