using System;
namespace FYP_WEB_APP.Models.LogicModels
{
    public class IntelligentControlDeviceUnit
    {
        static ApparenTemperatureUtil apUtil = new ApparenTemperatureUtil();
        public void IntelligentControlDevice() {

            var T = 0;//room temp
            var H = 0;//room hum
            var W = 0.15;//room wind 0.05 ~ 0.3m/s

            var cT = 0;
            if (T >= 26.7)
            {//heat index
                cT = apUtil.calHeatIndex(T, H);
            }
            else
            {
                cT = apUtil.calApparenTemperature(T, H, W);
           }
            var AC1 = 25.5;
            var AC2 = 25.5;
            //3C 
            if (28 =< cT && cT <= 30)
            {
                AC1 = AC2 = 16;
            }
            else if (24 <= cT && cT <= 27)
            {
                AC1 = AC2 = 19;
            }
            else if (21 <= cT && cT <= 23)
            {
                AC1 = AC2 = 22;
            }
            else if (18 <= cT && cT <= 20)
            {
                AC1 = AC2 = 25;
            }
            else if (15 <= cT && cT <= 17)
            {
                AC1 = AC2 = 27;
            }
            else if (10 <= cT && cT <= 14)
            {
                AC1 = AC2 = 30;
            }

        }
    }
}
