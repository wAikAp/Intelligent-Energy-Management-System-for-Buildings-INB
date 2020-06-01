using System;
using System.Collections.Generic;
using System.Diagnostics;
using FYP_APP.Controllers;
using FYP_WEB_APP.Models.MongoModels;

namespace FYP_WEB_APP.Models.LogicModels
{
	public class IntelligentControlDeviceUnit
	{
		static ApparenTemperatureUtil apUtil = new ApparenTemperatureUtil();

        public MongoIndoorTempSetting_Model mongoIndoor;

        public void IntelligentControlDevice()
		{

			//ApparenTemperatureUtil apparenTemperatureUtil = new ApparenTemperatureUtil();
			List<String> strList = apUtil.getRoomList();
			foreach (String roomid in strList) {
				//apparenTemperatureUtil.getAvgLig("F348");
				double T = apUtil.getAvgTemp(roomid);//room temp
				double H = apUtil.getAvgHum(roomid);//room hum
				double W = 0.5;//room wind 0.05 ~ 0.3m/s

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

				AC1 = calGoodTemp(roomid);
				apUtil.setAcCurrent(roomid, AC1);
				Debug.WriteLine("Room:"+roomid+"feels like:" + cT + " AC temp:" + AC1);
			}
			
		}

		public void scheduledControl() {
			//GetScheduleList
			SchedulesController schedulesController = new SchedulesController();
			List<ScheduleModel> scheduleModels = schedulesController.GetScheduleList();
			foreach(ScheduleModel sm in scheduleModels){
				var roomID = sm.Location;
				DateTime sdate = DateTime.Parse(sm.Date + " " + sm.Time);

				var dftmp = calGoodTemp(roomID);
				
				//"Lab","Lecture"
                /*
				if (sm.EventType.Equals("Lab"))
				{
                    sdate = sdate.AddMinutes(-5);
				}
				else if (sm.EventType.Equals("Lecture")) {
                    sdate = sdate.AddMinutes(-8);
				}*/
				sdate = sdate.AddMinutes(-8);
				String scheduleTime = sdate.ToString("dd/MM/yyyy HH:mm");
				Debug.WriteLine("scheduleTime: " + scheduleTime);
				String now = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
				Debug.WriteLine("DateTime.Now: " + now);
				//TimeSpan timeSpan = now - sdate;
				if (now == scheduleTime) 
				{
					Boolean success = apUtil.setAcCurrentAndTurnON(roomID, dftmp);
					if (success)
					{
						Debug.WriteLine("scheduledControl: ready to have a booking: roomID = " + roomID);
					}
					else {
						Debug.WriteLine("scheduledControl: booked room turn on fail = " + roomID);
					}

				}
			}
		}

		public double calGoodTemp(String roomId)
		{
			double T = apUtil.getAvgTemp(roomId);//room temp
			double H = apUtil.getAvgHum(roomId);//room hum
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

			double AC1 = 25.5;//AC default temp
			double tempRangeHighest = 30;//AC highest tmp
			double checkDegRange = 2; //temperature range eg. 25-27
			double eachTempRange = 3; //temperature range
			double ACLowestTemp = 18;//the AC lowest temperature
			if (this.mongoIndoor != null)
			{
				AC1 = this.mongoIndoor.acDefaultTemp;
				tempRangeHighest = this.mongoIndoor.tempRangeHighest;
				checkDegRange = this.mongoIndoor.checkDegRange; 
				eachTempRange = this.mongoIndoor.eachTempRange; 
				ACLowestTemp = this.mongoIndoor.acLowestTemp;
			}
			
			/*
                30 -2 ,30
                27 -2,27 -3 
                24 -2,24 -6 
                21 -2,21 -9
                18 -2,18 -12

                30C - 16C do 5 times,each time decrease 3C
             */
			for (int i = 0; i <= 5; i++) {

                var calTempRangeH = tempRangeHighest - (eachTempRange*i);
				var calTempRangeL = tempRangeHighest - checkDegRange;
				if (calTempRangeL <= cT && cT <= calTempRangeH) {
					/*
                        16,+3*0
                        19,+3*1
                        22,+3*2
                        25,+3*3
                        27,+3*4
                        30 +3*5
                     */
					AC1 = ACLowestTemp + checkDegRange * i;
				}
            }

            /*
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
				AC1 = 22;
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
            */

			//Debug.WriteLine("roomID: " + roomId +" good temp = "+AC1);
			return AC1;
		}
		public void scheduledRoom()
		{
			//GetScheduleList
			SchedulesController schedulesController = new SchedulesController();
			List<ScheduleModel> scheduleModels = schedulesController.GetScheduleList();
			List<String> scheduledRoomList = new List<string>();
			foreach (ScheduleModel sm in scheduleModels)
			{

				Debug.WriteLine("sm.Location:" + sm.Location);
				
			}
			
		}
	}
}
