using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FYP_APP.Controllers;
using FYP_WEB_APP.Models;
using FYP_WEB_APP.Models.chart;
using FYP_WEB_APP.Models.MongoModels;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace FYP_WEB_APP.Controllers
{
    public class ChartController : Controller
    {
		DateTime today = DateTime.UtcNow.AddHours(8);

		private int timeSpacing;//minute ~ point to point of spacing
		private int time;//minute ~ how long display
		/*
		 * link this funcation
		 * chart/chart?title=test chart&chartType=line&position=top&download=true&hour=5&timeSpacing=30&type=TS
		 * 
		 * you can by roomid to view
		 * chart/chart?roomId=348&title=test chart&chartType=line&position=top&download=true&time=10&timeSpacing=30&type=TS
		 * 
		 */
		[Route("Chart/Log")]
		[HttpGet]
		public IEnumerable<CurrentDataModel> Log()
		{
			DateTime dtoday = DateTime.UtcNow.AddHours(8);
			DateTime start = dtoday.AddHours(-1);
			DateTime end = dtoday;
			var filterStartDateTime = Builders<CurrentDataModel>.Filter.Gte(x => x.latest_checking_time, start);

			var filterEndDateTime = Builders<CurrentDataModel>.Filter.Lte(x => x.latest_checking_time, end);
			List<CurrentDataModel> current = new List<CurrentDataModel>();

			List<CurrentDataModel> TS = new List<CurrentDataModel>();
			List<CurrentDataModel> HS = new List<CurrentDataModel>();
			List<CurrentDataModel> LS = new List<CurrentDataModel>();
			List<CurrentDataModel> AS = new List<CurrentDataModel>();
			string[] tableArr= { "TMP_SENSOR", "LIGHT_SENSOR", "HUM_SENSOR" };
			foreach (var get in tableArr) {

			current.AddRange(new DBManger().DataBase.GetCollection<CurrentDataModel>(get).Find(new BsonDocument()).Limit(50).Sort(Builders<CurrentDataModel>.Sort.Descending(s => s.latest_checking_time)).ToList());//B list add in A list
				Debug.WriteLine(current.ToJson().ToString());

			}
			current.OrderByDescending(x=>x.latest_checking_time);
			foreach (var p in current) {
				yield return p;

			}
		}
		[Route("Chart/Chart")]
		[HttpGet]
		public ActionResult Chart() {
			//getallcurrent();
			if (Request.QueryString.HasValue)
			{
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
				try
				{
					this.time = Convert.ToInt32(Request.Query["time"]);
					this.timeSpacing = Convert.ToInt32(Request.Query["timeSpacing"]);
					string roomId="";
					if (!string.IsNullOrEmpty(Request.Query["roomId"].ToString()))
					{
						roomId = Request.Query["roomId"];
					}
					
					switch (type)
					{
						case "TS"://Temp sensor
						case "LS"://Light sensor
						case "HS"://Hum sensor
							ViewBag.datasets = SensorsCurrectLineChart(iid, roomId);
							break;
						case "AC"://Ac devices
						case "LT"://Light devices
						case "HD"://Hum devices
						case "EF"://Fan devices
							ViewBag.datasets = DeviceCurrectLineChart(iid, roomId);
							break;
					}
				}
				catch (Exception e)
				{
						return Content(e.Message );
				}
				//ViewBag.datasets = ChartData(lists, Request.Query["sensorType"]);

				ViewBag.day = GetChartTime(time, timeSpacing);
				ViewBag.divId = GetRandomDivId();
				//ViewBag.unit
				//ViewBag.unitName
			}
			else {
				ViewBag.charttitle = "";
				ViewBag.chartType = "";
				ViewBag.position = "";
				ViewBag.download = false;

				ViewBag.datasets = "";

				ViewBag.datasets = "";

				ViewBag.day = "";
				ViewBag.divId = "";
				//ViewBag.unit
				//ViewBag.unitName
			}
			return PartialView("_LineChart");

		}
		public string SensorsCurrectLineChart(string type,string roomId)
		{
			SensorsController SensorsControl = new SensorsController();
            //var SensorsDataList = SensorsControl.GetSensorsData().Where(s => s.roomId.Contains(roomId)).ToList();
			var SensorsDataList = SensorsControl.GetAllSensors().Where(s => s.roomId.Contains(roomId)).ToList();

			string tableName = "";

			var iid = type;
			if (type.Length > 2) {

				type = "true";
			}

			switch (type)
			{
				case "TS":
					SensorsDataList = SensorsDataList.Where(x => x.sensorId.Contains("TS")).ToList();
					ViewBag.unit = "℃";
					ViewBag.unitName = "Temperature";
					tableName = "TMP_SENSOR";

					break;
				case "LS":
					SensorsDataList = SensorsDataList.Where(x => x.sensorId.Contains("LS")).ToList();
					ViewBag.unit = " lm";
					ViewBag.unitName = "Luminosity";
					tableName = "LIGHT_SENSOR";

					break;
				case "HS":
					SensorsDataList = SensorsDataList.Where(x => x.sensorId.Contains("HS")).ToList();
					ViewBag.unit = " %";
					tableName = "HUM_SENSOR";
					ViewBag.unitName = "Humidity";
					break;
				case "true":
					SensorsDataList = SensorsDataList.Where(x => x.sensorId.Contains(iid)).ToList();
					switch (iid.Substring(0,2))
					{
						case "TS":
							ViewBag.unit = "℃";
							ViewBag.unitName = "Temperature";
							tableName = "TMP_SENSOR";

							break;
						case "LS":
							ViewBag.unit = " lm";
							ViewBag.unitName = "Luminosity";
							tableName = "LIGHT_SENSOR";

							break;
						case "HS":
							ViewBag.unit = " %";
							ViewBag.unitName = "Humidity";
					tableName = "HUM_SENSOR";
							break;
					}
					break;
			}

			List<CurrentDataModel> CurrentList = new List<CurrentDataModel>();
			//only ts

			var start = today.AddHours(time*-1);
			//var start = today.AddHours(-3);
			var end = today;
			//end set time
			List<string> labelss = new List<string>();

			List<object> datasets = new List<object>();
			List<object> datas = new List<object>();
			List<CurrentDataModel> searchResultOfCurrentDataList = new List<CurrentDataModel>();

			var filterStartDateTime = Builders<CurrentDataModel>.Filter.Gte(x => x.latest_checking_time, start);

			var filterEndDateTime = Builders<CurrentDataModel>.Filter.Lte(x => x.latest_checking_time, end);

			foreach (SensorsListModel get in SensorsDataList)
			{
				var filterid = Builders<CurrentDataModel>.Filter.Eq(x => x.sensorId,get.sensorId );

				labelss.Add(get.sensorId);
				searchResultOfCurrentDataList = new DBManger().DataBase.GetCollection<CurrentDataModel>(tableName).Find(filterid&filterStartDateTime & filterEndDateTime).Sort(Builders<CurrentDataModel>.Sort.Descending(s=>s.latest_checking_time)).ToList();

				datas.Add(MangeData(searchResultOfCurrentDataList).ToArray());
			}

			return LineChart(SensorsDataList.Count, labelss, datas);

		}
		
		public string DeviceCurrectLineChart(string type, string roomId)
		{

			DevicesController DevicesControl = new DevicesController();
		
			string tableName = "";
			List<DevicesListModel> DevicesDataList = DevicesControl.GetAllDevices().Where(d => d.roomId.Contains(roomId)).ToList(); 
			switch (type)
			{
				case "AC":
					DevicesDataList = DevicesDataList.Where(x => x.devicesId.Contains("AC")).ToList();
					ViewBag.unit = " C";
					ViewBag.unitName = "Air Conditioner Temperature";
					tableName = "AC";

					break;
				case "LT":
					DevicesDataList = DevicesDataList.Where(x => x.devicesId.Contains("LT")).ToList();
					ViewBag.unit = " lm";
					ViewBag.unitName = "Light Luminosity";
					tableName = "LIGHT";

					break;
				case "HD":
					DevicesDataList = DevicesDataList.Where(x => x.devicesId.Contains("HD")).ToList();
					ViewBag.unit = " %";
					ViewBag.unitName = "Humidifier Humidity"; 
					tableName = "HUM";

					break;
				case "EF":
					DevicesDataList = DevicesDataList.Where(x => x.devicesId.Contains("EF")).ToList();
					ViewBag.unit = " rpm";
					ViewBag.unitName = "FAN Revolution(s)"; 
					tableName = "EXH_FAN";

					break;
				case "AS":
					DevicesDataList = DevicesDataList.Where(x => x.devicesId.Contains("AS")).ToList();
					ViewBag.unit = " ";
					ViewBag.unitName = "AS";
					tableName = "AS_SENSOR";

					break;
				default:

					break;
			}

			List<CurrentDataModel> CurrentList = new List<CurrentDataModel>();

			DateTime today = DateTime.Now;


			//end set time
			List<string> labelss = new List<string>();

			List<object> datas = new List<object>();

			var start = today.AddHours(time * -1);
			//var start = today.AddHours(-3);
			var end = today;
			//end set time
			List<CurrentDataModel> searchResultOfCurrentDataList = new List<CurrentDataModel>();
			var filterStartDateTime = Builders<CurrentDataModel>.Filter.Gte(x => x.latest_checking_time, start);

			var filterEndDateTime = Builders<CurrentDataModel>.Filter.Lte(x => x.latest_checking_time, end);
			if (DevicesDataList.Count()>0) { 
			foreach (var get in DevicesDataList)
			{
			var filterId = Builders<CurrentDataModel>.Filter.Eq(x => x.devicesId, get.devicesId);
					
				labelss.Add(get.devicesId);
				searchResultOfCurrentDataList = new DBManger().DataBase.GetCollection<CurrentDataModel>(tableName).Find(filterId&filterStartDateTime & filterEndDateTime).Sort(Builders<CurrentDataModel>.Sort.Descending(s => s.latest_checking_time)).ToList();
				datas.Add(MangeData(searchResultOfCurrentDataList).ToArray());
			}
			}

			return LineChart(DevicesDataList.Count, labelss, datas); 
		}

		public string DoughnutChart(List<string> label, List<double> data)
		{
			if (label.Count()<1 &  data.Count() < 1)
			{
				throw new System.InvalidOperationException("The List, labels and data .count Not match !! ");
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
			//DateTime today = DateTime.Now;
		///	int hour = today.Hour;
		//	int Minute = today.Minute;

		//	int oMinute = today.Minute;
			//set time
		//	int ix = 0;
			List<string> datetime = new List<string>();
			var CountStart = today.AddHours(time * -1);
			//var start = today.AddHours(-3);
			var CountEnd = today;
			DateTime ca = CountStart;

			while (ca <= CountEnd) {
				datetime.Add(ca.ToString("HH:mm"));
				ca=ca.AddMinutes(timeSpacing);
			}
			/*	if ((hour - ctime) > 0)
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
				}*/
			return datetime.ToJson();

		}
		public List<string> MangeData(List<CurrentDataModel> CurrentList)
		{
			List<string> data = new List<string>();

			if (CurrentList.Count > 0) { 

			var MaStart = today.AddHours(time * -1);
			var MaEnd = today;

	
			DateTime ca = MaStart;
			TimeSpan catime = today - today.AddHours(time * -1);

			int counttime = 0;
			// time-hour to now time get data
			//miss spacking for no record.
				counttime = Convert.ToInt32(catime.TotalMinutes / timeSpacing); int zx = 0;

			string back ="";
			while (ca<= MaEnd)
			{
				var checkList = new List<CurrentDataModel>();

				zx++;	
				try {
					if (CurrentList.Count() > 0)
					{
						foreach (CurrentDataModel getCurrent in CurrentList)//in the time spaceing all record
						{

							var both = getCurrent.latest_checking_time >= ca && getCurrent.latest_checking_time <= ca.AddMinutes(timeSpacing);
							if (both)
							{
								checkList.Add(getCurrent);
							}
						}
						if (checkList.Count() > 0)
						{
							/*value = Convert.ToDouble(Convert.ToDouble(checkList.First().current).ToString("0.00"));*/
							data.Add(checkList.First().current.ToString());//get the time spacing first approaching the time point
							back = checkList.First().latest_checking_time.ToString();
						}
						else
						{
								//missing point
								//Debug.WriteLine("miss point =>> " + checkList.First().sensorId + " + " + back);
								data.Add("");
						}

					}
					else {
						//Debug.WriteLine("line 520 : CurrentList NOT > 0" );

					}

				//	Debug.WriteLine("*****************************************************");
            }catch (Exception e) {
						error("ChartController-Error-line 552:" + e.Message);
				}
				ca = ca.AddMinutes(timeSpacing);

			}
				
			}
			return data;
		}

		public ActionResult error(string str)
		{ return Content(str); }

			public string GetChartTime() { return null; }
		public void getallcurrent()
		{
			var start = today.AddHours(-3);
			var end = today;
			var filter1 = Builders<CurrentDataModel>.Filter.Gte(x => x.latest_checking_time, start);

			var filter = Builders<CurrentDataModel>.Filter.Lte(x => x.latest_checking_time, end);

			var cursor = new DBManger().DataBase.GetCollection<CurrentDataModel>("TMP_SENSOR").Find(filter1&filter).ToList();


		/*	foreach (var get in query)
			{
				x++;
				Debug.WriteLine(x + " => " + get.latest_checking_time);
			}*/
		}
	}
}