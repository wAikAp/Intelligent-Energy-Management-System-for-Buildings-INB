using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FYP_WEB_APP.Models;
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
    public class PIcamController : ControllerBase
    {
        // GET: api/<PIcamController>
       /* [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PIcamController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
       */
        // POST api/<PIcamController>
        [HttpPost]
        public string Post(object imgJson)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(imgJson);
            var piJsonModel = JsonConvert.DeserializeObject<pi_cam_model>(json);
            
            var deviceFilter = Builders<MongoDevicesListModel>.Filter.Eq(x => x.devicesId, piJsonModel.deviceId);
            var PiFilter = Builders<pi_cam_model>.Filter.Eq(x => x.deviceId, piJsonModel.deviceId);

            var deviceList =new DBManger().DataBase.GetCollection<MongoDevicesListModel>("DEVICES_LIST").Find(deviceFilter).ToList();
            //Debug.WriteLine(get.deviceId +"has "+check.Count());
            try
            {
                if (deviceList.Count() != 0)
                {//user need added to devicesList first
                    var piCamList = new DBManger().DataBase.GetCollection<pi_cam_model>("PI_CAM").Find(PiFilter).ToList();
                    if (piCamList.Count() != 0)
                    {
                        var up = Builders<pi_cam_model>.Update
                            .Set(x => x.base_cam_img, piJsonModel.base_cam_img)
                            .Set(x => x.latest_checking_time, DateTime.UtcNow.AddHours(8));

                        var UpdateResult = new DBManger().DataBase.GetCollection<pi_cam_model>("PI_CAM").FindOneAndUpdateAsync(u => u.deviceId == piJsonModel.deviceId, up);
                        if (UpdateResult == null)
                        {
                            Debug.WriteLine("Can not Update PI CAM Result =>> " + UpdateResult);
                            throw new System.ArgumentException("Update PI cam to db fail", "original");
                        }
                    }
                    else
                    {//first upload data
                        var newCamModelTodb =new pi_cam_model()
                        {
                            roomId = deviceList.FirstOrDefault().roomId,
                            deviceId = piJsonModel.deviceId,
                            latest_checking_time = DateTime.UtcNow.AddHours(8),
                            base_cam_img = piJsonModel.base_cam_img,
                            current = 0
                    };
                        new DBManger().DataBase.GetCollection<pi_cam_model>("PI_CAM").InsertOneAsync(newCamModelTodb);
                    }

                }
                else {// didn't add to the devicesList
                    throw new System.ArgumentException("Please add the device to db on UI", "original");
                }
            }catch (Exception e){
                return "{ status: false,"+ "msg:" + e.Message.ToString()+"}";
            };

            return "{ status: true," + "msg: Success, Pi cam updated}";

        }
        /*
        device id
        roomid
        Base64

         
         */
        /*   // PUT api/<PIcamController>/5
           [HttpPut("{id}")]
           public void Put(int id, [FromBody] string value)
           {
           }

           // DELETE api/<PIcamController>/5
           [HttpDelete("{id}")]
           public void Delete(int id)
           {
           }*/
    }
    public class pi_cam_model {
        public ObjectId _id { get; set; }
        public string roomId{ get; set; }
        public string deviceId { get; set; }
        public string base_cam_img { get; set; }
        public DateTime latest_checking_time { get; set; }
        public double current { get; set; }
        
    }
}
