using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_WEB_APP.Models
{
    public class SensorsListModel
    {
        public ObjectId _id { get; set; }

        public string roomId { get; set; }
        public string sensorId { get; set; }
        public string sensor_name { get; set; }

        public double pos_x { get; set; }
        public double pos_y { get; set; }
        
        public string desc { get; set; }
        public DateTime latest_checking_time { get; set; }
        public DateTime total_run_time { get; set; }
        public string typeImg { get; set; }
        public string typeUnit { get; set; }
        public string Sensortype { get; set; }
        public double current_Value { get; set; }
        public DateTime current_Time { get; set; }

    }
}
