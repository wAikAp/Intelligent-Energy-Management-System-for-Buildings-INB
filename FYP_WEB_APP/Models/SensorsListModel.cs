﻿using MongoDB.Bson;
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
        public string location { get; set; }
        public string desc { get; set; }
        public DateTime latest_checking_time { get; set; }
        public int total_run_time { get; set; }
    }
}
