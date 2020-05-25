using System;
using System.Collections.Generic;
using System.Diagnostics;
using FYP_APP.Controllers;

namespace FYP_WEB_APP.Models.LogicModels
{
	public class IntelligentControlDeviceUnit
	{
		static ApparenTemperatureUtil apUtil = new ApparenTemperatureUtil();
		public void IntelligentControlDevice()
		{

			//ApparenTemperatureUtil apparenTemperatureUtil = new ApparenTemperatureUtil();
			List<String> strList = apUtil.getRoomList();
			foreach (String roomid in strList) {
				//apparenTemperatureUtil.getAvgLig("F348");
				double T = apUtil.getAvgTemp(roomid);//room temp
				double H = apUtil.getAvgHum(roomid);//room hum
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
				apUtil.setAcCurrent(roomid, AC1);
				Debug.WriteLine("Room:"+roomid+"feels like:" + cT + "AC temp:" + AC1);
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
				if (sm.EventType.Equals("Lab"))
				{
                    sdate = sdate.AddMinutes(-5);
				}
				else if (sm.EventType.Equals("Lecture")) {
                    sdate = sdate.AddMinutes(-8);
				}
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
