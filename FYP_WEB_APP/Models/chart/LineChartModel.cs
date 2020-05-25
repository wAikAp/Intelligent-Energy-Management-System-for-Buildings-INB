using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_WEB_APP.Models
{
    public class LineChartModel
    {
        public string label { get; set; }
        public string borderColor { get; set; }
        public bool fill { get; set; }
        public bool spanGaps { get; set; }
        public object data { get; set; }
    }
}
