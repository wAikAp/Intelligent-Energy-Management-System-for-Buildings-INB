using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_WEB_APP.Models
{
    public class ScheduleViewModel
    {
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string EventName { get; set; }
        public string EventType { get; set; }
        public string section_id { get; set; }//Location
        public string UsagePlan { get; set; }
        public string User { get; set; }
        public string Room { get; set; }
    }
}
