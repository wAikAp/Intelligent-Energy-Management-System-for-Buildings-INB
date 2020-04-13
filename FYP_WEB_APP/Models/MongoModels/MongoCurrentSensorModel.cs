using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_WEB_APP.Models.MongoModels
{
    public class MongoCurrentSensorModel
    {
        public ObjectId _id { get; set; }
        public string sensorId { get; set; }
        public string current_tmp { get; set; }
        public string current_lum { get; set; }
        public string current_hum { get; set; }
        public DateTime latest_checking_time { get; set; }
    }
}
