using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP_WEB_APP.Models.API;
using FYP_WEB_APP.Models.MongoModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace FYP_WEB_APP.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class getDeviceStatusController : ControllerBase
    {
        // GET api/getDeviceStatus/{id}
        [HttpGet("{id}")]
        public string Get(string id)
        {
            StatusModel returnString = new StatusModel();

            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    List<FYP_WEB_APP.Models.MongoModels.MongoDevicesListModel> DList = new List<FYP_WEB_APP.Models.MongoModels.MongoDevicesListModel>();
                    DList = new FYP_WEB_APP.Models.DBManger().DataBase.GetCollection<MongoDevicesListModel>("DEVICES_LIST").Find(d => d.devicesId.Contains(id)).ToList();
                    if (DList.Count() == 1)
                    {
                        returnString = new StatusModel()
                        {
                            deviceId = DList.First().devicesId,
                            status = DList.First().status,
                            set_value = DList.First().set_value,
                            lastest_checking_time = DList.First().lastest_checking_time
                        };
                    }
                }
                else
                {
                    throw new System.ArgumentException("ID is incorrect format !!", id + " id no found");
                }
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }
            var json = System.Text.Json.JsonSerializer.Serialize(returnString);//to json

            return json;

        }
    }
}
