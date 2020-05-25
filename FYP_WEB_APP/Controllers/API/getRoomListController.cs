using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP_APP.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace FYP_WEB_APP.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class getRoomListController : ControllerBase
    {
        /*
         [{ "roomName" : "Lab 341", "roomId" : "341" },
         { "roomName" : "Lab Fake 348", "roomId" : "F348" }, ...
         
             */
        // GET: api/getRoomList
        [HttpGet]
        public ActionResult Get()
        {
            List<getRoomListModel> roomList = new List<getRoomListModel>();
            foreach (var get in new RoomsController().getRoomListFromDB()) {
                var data = new getRoomListModel
                {
                    roomName=get.roomName,roomId=get.roomId
                };
                roomList.Add(data);
            }
                return Ok(roomList);
        }

    }
    public class getRoomListModel {
        public String roomName { get; set; }
        public String roomId { get; set; }
    }
}
