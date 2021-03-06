﻿using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_WEB_APP.Models
{
    public class DevicesListModel
    {
        public ObjectId _id { get; set; }
        public string roomId { get; set; }
        public string devicesId { get; set; }
        public string devices_Name { get; set; }
        public double power { get; set; }
        public DateTime lastest_checking_time { get; set; }
        public DateTime turn_on_time { get; set; }//device turned on date
        public double pos_x { get; set; }
        public double pos_y { get; set; }
        public string desc { get; set; }
        public double current { get; set; }
        public bool powerOnOff { get; set; }
        public double avgPower { get; set; }
        public bool status { get; set; }
        public double set_value { get; set; }
        public double currentMonthUsage { get; set; }
        public double currentMonthTotalUseTime { get; set; }
        public double turnedOnUsage { get; set; }
        public double turnedOnTime { get; set; }//turned on time seconds
    }
}
