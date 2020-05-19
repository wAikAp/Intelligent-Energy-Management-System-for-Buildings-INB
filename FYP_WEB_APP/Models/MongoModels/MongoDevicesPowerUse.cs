using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace FYP_WEB_APP.Models.MongoModels
{
	public class MongoDevicesPowerUse
	{
        public ObjectId _id { get; set; }
        public string devicesId { get; set; }
        public string roomId { get; set; }
        public DateTime recorded_time { get; set; }
        public int recorded_used_time { get; set; }// store in s
        public double power_used { get; set; }//kwh
    }
}
