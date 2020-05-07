using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_WEB_APP.Models
{
    public class DevicesListModel
    {
        public ObjectId _id { get; set; }
        public string listId { get; set; }
        public string roomId { get; set; }
        public string devicesId { get; set; }
        public string devices_Name { get; set; }
        public int power { get; set; }
        public DateTime lastest_checking_time { get; set; }
        public DateTime total_run_time { get; set; }
        public string location { get; set; }
        public string desc { get; set; }
        public double current { get; set; }
        public bool powerOnOff { get; set; }
        public double avgPower { get; set; }

    }
}
