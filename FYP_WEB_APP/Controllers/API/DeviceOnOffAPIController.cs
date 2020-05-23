using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        // GET api/<LightAPIController>/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            OnOffModel returnString = new OnOffModel();
             
            try {
                if (!string.IsNullOrEmpty(id)) {
                    List<FYP_WEB_APP.Models.MongoModels.MongoDevicesListModel> DList = new List<FYP_WEB_APP.Models.MongoModels.MongoDevicesListModel>();
                    DList = new FYP_WEB_APP.Models.DBManger().DataBase.GetCollection<MongoDevicesListModel>("DEVICE_LIST").Find(d => d.devicesId.Contains(id)).ToList();
                    if (DList.Count() == 1) {
                        returnString = new OnOffModel()
                        {
                            deviceId = DList.First().devicesId,
                            status = DList.First().status,
                            set_value = DList.First().set_value,
                            lastest_checking_time = DList.First().lastest_checking_time
                        };
                    }
                }
                else {
                    throw new System.ArgumentException("ID is incorrect format !!", id+" id no found");
                }
            }
            catch (Exception e) {
                return e.Message.ToString();
            }
            var json = System.Text.Json.JsonSerializer.Serialize(returnString);//to json

            return json;

        }


        [HttpPost]
        public string Post(object PostJson)
        {
            bool isdone = false;
            OnOffModel retrunModel = new OnOffModel();
            try { 
            var json = System.Text.Json.JsonSerializer.Serialize(PostJson);
            var data = JsonConvert.DeserializeObject<List<OnOffModel>>(json);

            foreach (var get in data)
            {

                var up = Builders<FYP_WEB_APP.Models.MongoModels.MongoDevicesListModel>.Update.Set(x => x.status, get.status);
                var UpdateResult = new Models.DBManger().DataBase.GetCollection<MongoDevicesListModel>("DEVICES_LIST").FindOneAndUpdateAsync(u => u.devicesId == get.deviceId, up);
                if (get.deviceId.Contains("AC"))
                {
                    up = Builders<FYP_WEB_APP.Models.MongoModels.MongoDevicesListModel>.Update.Set(x => x.set_value, get.set_value);
                     new Models.DBManger().DataBase.GetCollection<MongoDevicesListModel>("DEVICES_LIST").FindOneAndUpdateAsync(u => u.devicesId == get.deviceId, up);
                 }
                DateTime nowTime = DateTime.UtcNow.AddHours(8);
                if (get.status)//true
                {
                    up = Builders<FYP_WEB_APP.Models.MongoModels.MongoDevicesListModel>.Update.Set(x => x.turn_on_time,nowTime);
                    new Models.DBManger().DataBase.GetCollection<MongoDevicesListModel>("DEVICES_LIST").FindOneAndUpdateAsync(u => u.devicesId == get.deviceId, up);
                    isdone= true;
                        retrunModel = get;
                }
                else {
                        if (!new DevicesPowerUseInputUtil().insertDevicesPowerUse(get.deviceId))
                        {
                            throw new System.ArgumentException("insertDevicesPowerUse error", get.deviceId + " insert Devices Power Use");
                        }
                        else {
                            retrunModel = get;
                        }
                    }
            }
            }
            catch (Exception e) {
                return e.Message.ToString();
            }
            return retrunModel.ToJson();
        }
    }
    public class OnOffModel
    {
        public string deviceId { get; set; }
        public bool status { get; set; }
        public double set_value { get; set; }
        public DateTime lastest_checking_time { get; set; }
    }
}
