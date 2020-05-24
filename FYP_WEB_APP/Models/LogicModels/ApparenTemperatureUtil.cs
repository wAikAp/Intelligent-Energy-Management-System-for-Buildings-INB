using System;
using System.Diagnostics;

namespace FYP_WEB_APP.Models.LogicModels
{
    public class ApparenTemperatureUtil
    {
        public double calHeatIndex(double T, double RH)
        {
            //heat index for out door indoor 
            T = 1.8 * T + 32;
            var HI = 0.5 * (T + 61 + (T - 68) * 1.2 + RH * 0.094);
            if (HI >= 80)
            {
                HI = -42.379 + 2.04901523 * T + 10.14333127 * RH - .22475541 * T * RH
                    - .00683783 * T * T - .05481717 * RH * RH + .00122874 * T * T * RH
                    + .00085282 * T * RH * RH - .00000199 * T * T * RH * RH;
                if (RH < 13 && 80 < T && T < 112)
                {
                    var ADJUSTMENT = (13 - RH) / 4 * Math.Sqrt((17 - Math.Abs(T - 95)) / 17);
                    HI -= ADJUSTMENT;
                }
                else if (RH > 85 && 80 < T && T < 87)
                {
                    var ADJUSTMENT = (RH - 85) * (87 - T) / 50;
                    HI += ADJUSTMENT;
                }
            }
            HI = Math.Round((HI - 32) / 1.8, 2);
            Debug.WriteLine("HeatIndex = " + HI);
            return HI;
            
        }

        public double calApparenTemperature(double T, double RH, double V)
        {
            //Where AT is somatosensory temperature (° C), T is air temperature (° C),
            //e is water pressure (hPa), V is wind speed (m / sec), RH is relative humidity (%)
            var e = RH / 100 * 6.105 * Math.Exp((17.26 * T) / (237.7 + T));
            var AT = 1.07 * T + 0.2 * e - 0.65 * V - 2.7;
            AT = Math.Round(AT, 2);
            Debug.WriteLine("calApparenTemperature = " + AT);
            return AT;
        }

        public string getUIColor(double AT) {
            
            var ATbgCol = "badge-secondary";
            if (AT > 25.5 && AT < 30)
            {
                ATbgCol = "badge-warning";
            }
            else if (AT > 30)
            {
                ATbgCol = "badge-danger";
            }
            else if (AT <= 25.5 && AT > 21)
            {
                ATbgCol = "badge-primary";
            }
            else if (AT <= 21 && AT > 0)
            {
                ATbgCol = "badge-info";
            }
            else {
                ATbgCol = "badge-secondary";
            }
            return ATbgCol;
        }

    }
}
