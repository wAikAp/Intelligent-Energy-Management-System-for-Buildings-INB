using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FYP_APP.Controllers;
using FYP_WEB_APP.Models;
using FYP_WEB_APP.Models.chart;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace FYP_WEB_APP.Controllers
{
    public class ChartController : Controller
    {
		private int timeSpacing;//minute ~ point to point of spacing
		private int time;//minute ~ how long display
		/*
		 * link this funcation
		 * chart/chart?title=test chart&chartType=line&position=top&download=true&time=10&timeSpacing=30&type=TS
		 * 
		 * you can by roomid to view
		 * chart/chart?roomId=348&title=test chart&chartType=line&position=top&download=true&time=10&timeSpacing=30&type=TS
		 * 
		 */
		[HttpGet]
		public ActionResult chart() {

			ViewBag.charttitle = Request.Query["title"];//chart title
			ViewBag.chartType = Request.Query["chartType"];//chart type
			ViewBag.position = Request.Query["position"];//the lable position
			ViewBag.download = Convert.ToBoolean(Request.Query["download"]);//true:display download
			string type = Request.Query["type"];
			string iid = "";
			if (type.Length > 2)
			{
				iid = type;
				type = type.Substring(0, 2);
			}
			else { iid = type; }
			try {
				this.time = Convert.ToInt32(Request.Query["time"]);
				this.timeSpacing = Convert.ToInt32(Request.Query["timeSpacing"]);
				Debug.WriteLine(timeSpacing);


				string roomId = Request.Query["roomId"];
				if (roomId == null) {
					roomId = "";
				}
				switch (type) {
				case "TS"://Temp sensor
				case "LS"://Light sensor
				case "HS"://Hum sensor
					ViewBag.datasets=SensorsCurrectLineChart(iid, roomId);
					break;
				case "AC"://Ac devices
				case "LT"://Light devices
				case "HD"://Hum devices
				case "EF"://Fan devices
					ViewBag.datasets=DeviceCurrectLineChart(iid, roomId);
					break;
				}
			}
			catch (Exception e) {
				return Content(e.Message);
			}
			//ViewBag.datasets = ChartData(lists, Request.Query["sensorType"]);

			ViewBag.day = GetChartTime(time,timeSpacing);
			ViewBag.divId = GetRandomDivId();
			//ViewBag.unit
			//ViewBag.unitName

			return PartialView("_LineChart");

		}
		public string SensorsCurrectLineChart(string type,string roomId)
		{
			Debug.WriteLine("\n\n Chart strat ");
			FYP_APP.Controllers.SensorsController SensorsControl = new FYP_APP.Controllers.SensorsController();
			if (string.IsNullOrEmpty(roomId))
			{
				if (Request.Cookies.TryGetValue("returnUrl", out string url))
				{
					string host = HttpContext.Request.Host.Value;

					string UrlroomId = HttpUtility.ParseQueryString(new Uri(host + url).Query).Get("roomId");
					if (!string.IsNullOrEmpty(UrlroomId))
					{
						roomId = UrlroomId;
					}
				}
			}
			
			var SensorsDataList = SensorsControl.GetSensorsData().Where(s => s.roomId.Contains(roomId)).ToList();
			

			var iid = type;
			if (type.Length > 2) {

				type = "true";
			}

			switch (type)
			{
				case "TS":
					SensorsDataList = SensorsDataList.Where(x => x.sensorId.Contains("TS")).ToList();
					ViewBag.unit = " ";
					ViewBag.unitName = "Temperature";
					break;
				case "LS":
					SensorsDataList = SensorsDataList.Where(x => x.sensorId.Contains("LS")).ToList();
					ViewBag.unit = " lm";
					ViewBag.unitName = "Luminosity";
					break;
				case "HS":
					SensorsDataList = SensorsDataList.Where(x => x.sensorId.Contains("HS")).ToList();
					ViewBag.unit = " %";
					ViewBag.unitName = "Humidity";
					break;
				case "true":
					SensorsDataList = SensorsDataList.Where(x => x.sensorId.Contains(iid)).ToList();
					switch (iid.Substring(0,2))
					{
						case "TS":
							ViewBag.unit = " ";
							ViewBag.unitName = "Temperature";
							break;
						case "LS":
							ViewBag.unit = " lm";
							ViewBag.unitName = "Luminosity";
							break;
						case "HS":
							ViewBag.unit = " %";
							ViewBag.unitName = "Humidity";
							break;
					}
					break;
			}

			List<CurrentDataModel> CurrentList = new List<CurrentDataModel>();
			//only ts

			DateTime today = DateTime.Now;


			//end set time
			List<string> labelss = new List<string>();

			List<object> datasets = new List<object>();
			List<object> datas = new List<object>();

			foreach (SensorsListModel get in SensorsDataList)
			{
				labelss.Add(get.sensorId);
				CurrentList = SensorsControl.GetSensorIDCurrentList(get.sensorId).Where(x => x.latest_checking_time > today.AddDays(-1)).OrderBy(x => x.latest_checking_time).ToList();
				
				datas.Add(MangeData(CurrentList).ToArray());
			}

			/*for (int i = 0; i < SensorsDataList.Count; i++)
			{
				labelss.Add(SensorsDataList[i].sensorId);
			}*/
			return LineChart(SensorsDataList.Count, labelss, datas);

		}
		
		public string DeviceCurrectLineChart(string type, string roomId)
		{

			DevicesController DevicesControl = new DevicesController();
			if (string.IsNullOrEmpty(roomId)) { 
			if (Request.Cookies.TryGetValue("returnUrl", out string url))
			{
				string host = HttpContext.Request.Host.Value;

				string UrlroomId = HttpUtility.ParseQueryString(new Uri(host + url).Query).Get("roomId");
				if (!string.IsNullOrEmpty(UrlroomId))
				{
					roomId = UrlroomId;
				}
			}
			}
			List<DevicesListModel> DevicesDataList = DevicesControl.GetAllDevices().Where(d => d.roomId.Contains(roomId)).ToList(); 
			switch (type)
			{
				case "AC":
					DevicesDataList = DevicesDataList.Where(x => x.devicesId.Contains("AC")).ToList();
					ViewBag.unit = " ";
					ViewBag.unitName = "Air Conditioning";
					break;
				case "LT":
					DevicesDataList = DevicesDataList.Where(x => x.devicesId.Contains("LT")).ToList();
					ViewBag.unit = " lm";
					ViewBag.unitName = "Light";
					break;
				case "HD":
					DevicesDataList = DevicesDataList.Where(x => x.devicesId.Contains("HD")).ToList();
					ViewBag.unit = " %";
					ViewBag.unitName = "Humidifier";
					break;
				case "EF":
					DevicesDataList = DevicesDataList.Where(x => x.devicesId.Contains("EF")).ToList();
					ViewBag.unit = " rpm";
					ViewBag.unitName = "FAN";
					break;
				default:

					break;
			}

			List<CurrentDataModel> CurrentList = new List<CurrentDataModel>();

			DateTime today = DateTime.Now;


			//end set time
			List<string> labelss = new List<string>();

			List<object> datas = new List<object>();

			foreach (DevicesListModel get in DevicesDataList)
			{
				labelss.Add(get.devicesId);
				CurrentList = DevicesControl.GetChartDataList(get.devicesId).Where(x => x.latest_checking_time > today.AddDays(-1)).OrderBy(x => x.latest_checking_time).ToList();

				datas.Add(MangeData(CurrentList).ToArray());
			}

		/*	for (int i = 0; i < DevicesDataList.Count; i++)
			{
				labelss.Add(DevicesDataList[i].devicesId);
			}*/

			/*foreach (var get in datas)
			{
				Debug.WriteLine(get.ToJson());

			}*/
			return LineChart(DevicesDataList.Count, labelss, datas); 
		}

		public string DoughnutChart(List<string> label, List<double> data)
		{
			if (label.Count()<1 &  data.Count() < 1)
			{
				throw new System.InvalidOperationException("The List and labels and data .count Not match !! ");
			}
			else
			{
				List<string> Color = new List<string>();

				List<object> datasets = new List<object>();

				for (int i = 0; i < label.Count(); i++)
				{
					Color.Add(GetRandomColor());
				}


					var chartdata = new DoughnutChartModel()
					{
						label= label.ToArray(),
						data= data.ToArray(),
						backgroundColor= Color.ToArray(),
						borderWidth= 1,
						borderColor= "#777",
						hoverBorderWidth= 3,
						hoverBorderColor= "#000"
					};

					datasets.Add(chartdata);			

				return JsonConvert.SerializeObject(datasets);
			}
		}
		public string LineChart(int count, List<string> label, List<object> data)
        {
			if (count != label.Count() & count != data.Count())
			{
				throw new System.InvalidOperationException("The List and labels and data .count Not match !! ");
			}
			else { 
			List<string> Color = new List<string>();

			List<object> datasets = new List<object>();


			for (int i = 0; i < count; i++)
			{

				var chartdata = new LineChartModel()
				{
					fill = false,
					spanGaps = false,
					label = label[i],
					borderColor = GetRandomColor(),
					data = data[i]
				};

				datasets.Add(chartdata);
			}

			return JsonConvert.SerializeObject(datasets);
			}
		}

		public string GetRandomColor()
		{
			var random = new Random();
			var rmcolor = String.Format("#{0:X6}", random.Next(0x1000000));
			return rmcolor;
		}
		public string GetRandomDivId()
		{
			var random = new Random();
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, 10)
			  .Select(s => s[random.Next(s.Length)]).ToArray());
			//return rmcolor;
		}
		public string GetChartTime(int ctime,int timeSpacing)
		{
			DateTime today = DateTime.Now;
			int hour = today.Hour;
			int Minute = today.Minute;

			int oMinute = today.Minute;
			//set time
			int ix = 0;
			List<string> time = new List<string>();

			if ((hour - ctime) > 0)
			{
				for (int i = hour - ctime; i <= today.Hour; i++)
				{
					for (ix = Minute; ix < 60; ix += timeSpacing)
					{
						if (i == today.Hour && ix > today.Minute)
						{

						}
						else
						{
							time.Add(i.ToString() + ":" + ix.ToString());
						}
						if (ix > 55)
						{
							Minute = ix + timeSpacing;
							Minute %= 60;
						}
						else if (ix == 55)
						{
							Minute = 0;
						}

					}
				}
			}
			else {
				for (int i =24+(hour - ctime); i <24; i++)
				{
					for (ix = Minute; ix < 60; ix += timeSpacing)
					{						
							time.Add(i.ToString() + ":" + ix.ToString());
						if (ix > 55)
						{
							Minute = ix + timeSpacing;
							Minute %= 60;
						}
						else if (ix == 55)
						{
							Minute = 0;
						}

					}
				}
				for (int i = 0; i <= today.Hour; i++)
				{
					for (ix = Minute; ix < 60; ix += timeSpacing)
					{
						if (i == today.Hour && ix > today.Minute)
						{

						}
						else
						{
							time.Add(i.ToString() + ":" + ix.ToString());
						}
						if (ix > 55)
						{
							Minute = ix + timeSpacing;
							Minute %= 60;
						}
						else if (ix == 55)
						{
							Minute = 0;
						}

					}
				}
			}
			return time.ToJson();

		}
		public List<double> MangeData(List<CurrentDataModel> CurrentList)
		{
			DateTime today = DateTime.Now;
			List<double> data = new List<double>();

			DateTime ca = today;
			TimeSpan catime = ca - ca.AddHours(time * -1);
			int counttime = 0;

				counttime = Convert.ToInt32(catime.TotalMinutes / timeSpacing);


				for (int x = 0; x <= counttime; x++)
				{
					data.Add(0);

				}
				if (CurrentList.Count() != 0)
				{
					foreach (CurrentDataModel getCurrent in CurrentList)
					{
						var value = Convert.ToDouble(Convert.ToDouble(getCurrent.current).ToString("0.00"));

						ca = DateTime.Now.AddHours(time * -1);

						for (int x = 0; x <= counttime; x++)
						{
							var bo = getCurrent.latest_checking_time >= ca && getCurrent.latest_checking_time <= ca.AddMinutes(timeSpacing);

							ca = ca.AddMinutes(timeSpacing);
							if (getCurrent.latest_checking_time > ca && getCurrent.latest_checking_time < ca.AddMinutes(timeSpacing))
							{
								data[x] = value;
							}
						}
					}
				}
				else
				{
				}
			

			return data;
		}


		public string GetChartTime() { return null; }
	}
}