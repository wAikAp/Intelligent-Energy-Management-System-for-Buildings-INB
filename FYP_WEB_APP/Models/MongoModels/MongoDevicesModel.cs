using System;
using MongoDB.Bson;

namespace FYP_WEB_APP.Models.MongoModels
{
    public class MongoDevicesModel
    {
        public ObjectId _id { get; set; }
        public string deviceId { get; set; }
        public double current { get; set; }        
        public DateTime latest_checking_time { get; set; }
        public string base_cam_img { get; set; }
    }
}
