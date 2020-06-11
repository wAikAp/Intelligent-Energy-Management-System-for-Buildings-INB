using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FYP_WEB_APP.Models.MongoModels
{
    public class MongoIndoorTempSetting_Model
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string settingID { get; set; }
        public double acDefaultTemp { get; set; }
        public double tempRangeHighest { get; set; }
        public double checkDegRange { get; set; }
        public double eachTempRange { get; set; }
        public double acLowestTemp { get; set; }
        public string checkTimeMinutes { get; set; }
    }
}
