using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP_WEB_APP.Models.API;
using FYP_WEB_APP.Models.LogicModels;
using FYP_WEB_APP.Models.MongoModels;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FYP_WEB_APP.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceOnOffAPIController : ControllerBase
    {
        /*
         * post json
         [
          {
            "deviceId": "LT003",
            "status":true,
            "set_value":"25.5"
          } 
        ]
             */


        [HttpPost]
        public string Post(object PostJson)
        {
            bool isdone = false;
            StatusModel returnModel = new StatusModel();
            List<MongoDevicesListModel> DList = new List<MongoDevicesListModel>();
            try { 
            var json = System.Text.Json.JsonSerializer.Serialize(PostJson);
            var data = JsonConvert.DeserializeObject<List<StatusModel>>(json);

                if (!string.IsNullOrEmpty(data.First().deviceId))
                {
                    System.Diagnostics.Debug.WriteLine(">"+data.First().deviceId+ "<");
                    DList = new FYP_WEB_APP.Models.DBManger().DataBase.GetCollection<MongoDevicesListModel>("DEVICES_LIST").Find(d => d.devicesId.Contains(data.First().deviceId)).ToList();
                    if (DList.Count() > 0)
                    {
                        var up = Builders<FYP_WEB_APP.Models.MongoModels.MongoDevicesListModel>.Update.Set(x => x.status, data.First().status);
                    var UpdateResult = new Models.DBManger().DataBase.GetCollection<MongoDevicesListModel>("DEVICES_LIST").FindOneAndUpdateAsync(u => u.devicesId == data.First().deviceId, up);
                    if (data.First().deviceId.Contains("AC"))
                    {
                        up = Builders<FYP_WEB_APP.Models.MongoModels.MongoDevicesListModel>.Update.Set(x => x.set_value, data.First().set_value);
                        new Models.DBManger().DataBase.GetCollection<MongoDevicesListModel>("DEVICES_LIST").FindOneAndUpdateAsync(u => u.devicesId == data.First().deviceId, up);
                    }
                    DateTime nowTime = DateTime.UtcNow.AddHours(8);
                    if (data.First().status)//true
                    {
                        up = Builders<FYP_WEB_APP.Models.MongoModels.MongoDevicesListModel>.Update.Set(x => x.turn_on_time, nowTime);
                        new Models.DBManger().DataBase.GetCollection<MongoDevicesListModel>("DEVICES_LIST").FindOneAndUpdateAsync(u => u.devicesId == data.First().deviceId, up);
                        isdone = true;

                    }
                    else{
                            if (!new DevicesPowerUseInputUtil().insertDevicesPowerUse(data.First().deviceId))
                        {
                            throw new System.ArgumentException("insertDevicesPowerUse error", data.First().deviceId + " insert Devices Power Use");
                        }
                    }

                        System.Diagnostics.Debug.WriteLine(DList.Count());
                        returnModel = new StatusModel()
                        {
                            deviceId = DList.First().devicesId,
                            set_value = DList.First().set_value,
                            status = DList.First().status,
                            lastest_checking_time = DList.First().lastest_checking_time
                        };

                    }
                    else
                    {
                        throw new System.ArgumentException("DevicesId error", " DevicesId not found");

                    }

                }
                else {
                    throw new System.ArgumentException("input error", " DevicesId not found");

                }

            }
            catch (Exception e) {
                return e.Message.ToString();
            }
            return returnModel.ToJson();
        }
    }

}
