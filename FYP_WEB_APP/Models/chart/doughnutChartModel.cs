using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_WEB_APP.Models.chart
{
    public class doughnutChartModel
    {

        public Array label { get; set; }
        public Array data { get; set; }
        public Array backgroundColor { get; set; }
        public int borderWidth { get; set; }
        public string borderColor { get; set; }
        public int hoverBorderWidth { get; set; }
        public string hoverBorderColor { get; set; }
    }
}
