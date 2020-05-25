using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_WEB_APP.Models.MongoModels
{
    public class MongoDevicesListModel
    {
        public ObjectId _id { get; set; }
        public string roomId { get; set; }
        public string devicesId { get; set; }
        public string devices_Name { get; set; }
        public double power { get; set; }
        public DateTime lastest_checking_time { get; set; }
        public DateTime turn_on_time { get; set; }
        public double pos_x { get; set; }
        public double pos_y { get; set; }
        public string desc { get; set; }
        public bool status { get; set; }
        public double set_value { get; set; }



    }
}
