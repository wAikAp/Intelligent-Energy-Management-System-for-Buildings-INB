using System;
using System.Collections.Generic;
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

            var data = JsonConvert.DeserializeObject<List<pi_cam_model>>(json);
            foreach (var get in data) {
                var deviceFilter = Builders<MongoDevicesListModel>.Filter.Eq(x => x.devicesId, get.deviceId);
                var PiFilter = Builders<pi_cam_model>.Filter.Eq(x => x.deviceId, get.deviceId);

                var check =new DBManger().DataBase.GetCollection<MongoDevicesListModel>("DEVICES_LIST").Find(deviceFilter).ToList();
                System.Diagnostics.Debug.WriteLine(get.deviceId +"has "+check.Count());
                try
                {
                    if (check.Count() != 0)
                    {

                        var picheck = new DBManger().DataBase.GetCollection<pi_cam_model>("PI_CAM").Find(PiFilter).ToList();
                        if (picheck.Count() != 0)
                        {
                            var up = Builders<pi_cam_model>.Update.Set(x=>x.base_cam_img, get.base_cam_img);
                            var UpdateResult = new DBManger().DataBase.GetCollection<pi_cam_model>("PI_CAM").FindOneAndUpdateAsync(u => u.deviceId == get.deviceId, up);
                             up = Builders<pi_cam_model>.Update.Set(x=>x.latest_checking_time, DateTime.UtcNow.AddHours(8));
                             UpdateResult = new DBManger().DataBase.GetCollection<pi_cam_model>("PI_CAM").FindOneAndUpdateAsync(u => u.deviceId == get.deviceId, up);


                            if (UpdateResult == null)
                            {
                                //error 
                                //return ;
                                //Debug.WriteLine("error: line 306 =>> "+ property.Name);
                            }
                        }
                        else
                        {

                            var insert =new pi_cam_model()
                            {
                                roomId = check.FirstOrDefault().roomId,
                                deviceId = get.deviceId,
                                latest_checking_time = DateTime.UtcNow.AddHours(8),
                                 base_cam_img = get.base_cam_img,
                                current = 0
                        };

                            new DBManger().DataBase.GetCollection<pi_cam_model>("PI_CAM").InsertOneAsync(insert);

                        }

                    }
                    else {
                        throw new System.ArgumentException("No device error", "original");

                    }
                }
                catch (Exception e)
                {

                    return e.Message.ToString();
                };
              
            }
            return "";

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
