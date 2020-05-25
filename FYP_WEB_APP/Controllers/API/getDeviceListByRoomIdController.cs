using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP_WEB_APP.Models;
using FYP_WEB_APP.Models.API;
using FYP_WEB_APP.Models.MongoModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FYP_WEB_APP.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class getDeviceListByRoomIdController : ControllerBase
    {

        // GET: api/getDeviceListByRoomId/5
        [HttpGet("{id}", Name = "Get")]
        public ActionResult Get(string id)
        {
            List<StatusModel> Statuslist = new List<StatusModel>();
           var filter = Builders<MongoDevicesListModel>.Filter.Eq("roomId", id);
            var getlist= new DBManger().DataBase.GetCollection<MongoDevicesListModel>("DEVICES_LIST").Find(filter).ToList();
            foreach (var get in getlist) {
                var data = new StatusModel {
                    deviceId = get.devicesId,
                    status = get.status.ToString(),
                    set_value = get.set_value
                };
                Statuslist.Add(data);
                  }
            return Ok(Statuslist);
        }

  
    }
    
}
