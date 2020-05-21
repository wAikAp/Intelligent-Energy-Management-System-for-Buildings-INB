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

namespace FYP_APP.Controllers
{
    public class RoomsController : Controller
    {
        public IActionResult Rooms()
        {
			//DBManger
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
            return View();
        }
    }
}