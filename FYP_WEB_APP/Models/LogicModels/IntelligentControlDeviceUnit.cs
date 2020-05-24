using System;
using System.Diagnostics;

namespace FYP_WEB_APP.Models.LogicModels
{
    public class IntelligentControlDeviceUnit
    {
        static ApparenTemperatureUtil apUtil = new ApparenTemperatureUtil();
        public void IntelligentControlDevice() {

            ApparenTemperatureUtil apparenTemperatureUtil = new ApparenTemperatureUtil();
            //apparenTemperatureUtil.getAvgLig("F348");
            double T = apparenTemperatureUtil.getAvgTemp("F348");//room temp
            double H = apparenTemperatureUtil.getAvgHum("F348");//room hum
            double W = 0.15;//room wind 0.05 ~ 0.3m/s
            
            double cT = 0;//Calculation the indoor feels like degree 
            if (T >= 26.7)
            {//heat index
                cT = apUtil.calHeatIndex(T, H);
            }
            else
            {//Apparent Temperature
                cT = apUtil.calApparenTemperature(T, H, W);
            }

            double AC1 = 25.5;

            //check the temp each 3'C 
            if (28 <= cT && cT <= 30)
            {
                AC1 = 16;
            }
            else if (24 <= cT && cT <= 27)
            {
                AC1 = 19;
            }
            else if (21 <= cT && cT <= 23)
            {
                AC1= 22;
            }
            else if (18 <= cT && cT <= 20)
            {
                AC1 = 25;
            }
            else if (15 <= cT && cT <= 17)
            {
                AC1 = 27;
            }
            else if (10 <= cT && cT <= 14)
            {
                AC1 = 30;
            }
            //apparenTemperatureUtil.setAcCurrent('F348',AC1);
            Debug.WriteLine("feels like:" + cT + "AC temp:"+AC1);
        }
    }
}
