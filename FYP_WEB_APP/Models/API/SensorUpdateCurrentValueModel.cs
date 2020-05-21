using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_WEB_APP.Models.API
{
    public class SensorUpdateCurrentValueModel
    {
        public string sensorId { get; set; }
        public string value { get; set; }
        public double[] values { get; set; }
        public double C_CO { get; set; }
        public double C_CO2 { get; set; }
        public double C_CI2 { get; set; }
        public double C_CH20 { get; set; }
        public double C_H2 { get; set; }
        public double C_CH4 { get; set; }
        public double C_H2S { get; set; }
        public double C_NO2{ get; set; }
        public double C_O3 { get; set; }
        public double C_C2CI4 { get; set; }
        public double C_SO2 { get; set; }
        public double C_VOC { get; set; }
        public double C_AVG_PM25 { get; set; }


    }
}
