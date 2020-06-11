using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FYP_WEB_APP.Models.MongoModels
{
    public class MongoPICamModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string roomId { get; set; }
        public string deviceId { get; set; }
        public string base_cam_img { get; set; }
        public DateTime latest_checking_time { get; set; }
        public double current { get; set; }
    }
}
