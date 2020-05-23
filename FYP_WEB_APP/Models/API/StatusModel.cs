using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_WEB_APP.Models.API
{
    public class StatusModel
    {
        public string deviceId { get; set; }
        public bool status { get; set; }
        public double set_value { get; set; }
        public DateTime lastest_checking_time { get; set; }
    }
}
