using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FYP_APP.Models;
using FYP_APP.Models.MongoModels;
using FYP_APP.Models.LogicModels;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using FYP_APP.Extensions;
using System.Linq;
using FYP_WEB_APP.Controllers;
using System.IO;
using MongoDB.Bson;
using FYP_WEB_APP.Models.LogicModels;
using Newtonsoft.Json.Linq;
using Hangfire;

namespace FYP_APP.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			if (HttpContext.Session.Get<MongoUserModel>("user")!= null)
			{
				return RedirectToAction("Home", "Home");
			}

			DevicesPowerUseInputUtil devicesPowerUseInputUtil = new DevicesPowerUseInputUtil();

			RecurringJob.AddOrUpdate(() => devicesPowerUseInputUtil.updateRoomPower(), "* * * * *");

			return View();
		}
		
		[HttpPost]
		public IActionResult Index(string loginUserName, string loginPass)
		{
			
			UserLogic userLogic = new UserLogic();
			MongoUserModel user = userLogic.login(loginUserName, loginPass);

			if(user != null)
			{
				HttpContext.Session.Set<MongoUserModel>("user", user);
				HttpContext.Session.SetString("userName", user.lName);
				//Debug.WriteLine("json " + HttpContext.Session.Get<MongoUserModel>("user"));
				return RedirectToAction("Dashboard", "Home");
			}
			else
			{
				ViewData["message"] = "Wrong user name or password!";
				
			}
			

			return View();
		}


		public IActionResult Dashboard()
		{

			DevicesPowerUseOutputUtil powerUseOutputUtil = new DevicesPowerUseOutputUtil();

			double TotalSavings = 0;
			double TotalUsage = Math.Round(powerUseOutputUtil.getTotalPowerUse(), 2);
			double ACpower = Math.Round(powerUseOutputUtil.getACPowerUse(),2);
			double LTpower = Math.Round(powerUseOutputUtil.getLPowerUse(),2);
			double HDpower = Math.Round(powerUseOutputUtil.getHUMPowerUse(),2);
			double EFpower = Math.Round(powerUseOutputUtil.getEXHFPowerUse(),2);
		
			ViewBag.TotalUsage = TotalUsage;
			ViewBag.TotalSavings = TotalSavings;
			ViewBag.ACpower =  ACpower ;
			ViewBag.LTpower =  LTpower ;
			ViewBag.HDpower =  HDpower ;
			ViewBag.EFpower =  EFpower ;

			ViewBag.trendJsonString = powerUseOutputUtil.getRoomPowerTrend();
		
			return View();
		}
		
		[Route("Home/DoughnutChart")]
		public ActionResult DoughnutChart()
		{

			Debug.WriteLine(Request.Query["title"]);
			Debug.WriteLine(Request.Query["chartType"]);
			Debug.WriteLine(Request.Query["position"]);

			ViewBag.charttitle = Request.Query["title"];
			ViewBag.chartType = Request.Query["chartType"];
			ViewBag.position = Request.Query["position"];

			List<string> label = new List<string>();
			DevicesController device = new DevicesController();
			List<double> data = new List<double>();

			DevicesPowerUseOutputUtil powerUseOutputUtil = new DevicesPowerUseOutputUtil();
			double ACpower = Math.Round(powerUseOutputUtil.getACPowerUse(), 2);
			double LTpower = Math.Round(powerUseOutputUtil.getLPowerUse(), 2);
			double HDpower = Math.Round(powerUseOutputUtil.getHUMPowerUse(), 2);
			double EFpower = Math.Round(powerUseOutputUtil.getEXHFPowerUse(), 2);
			double[] myNum = { ACpower, LTpower, HDpower, EFpower };
			var i = 0;
			foreach (string getDeviceTypeName in device.typeName) {
				label.Add(getDeviceTypeName);
				//data.Add(device.GetDeviceCount(getDeviceTypeName));
				data.Add(myNum[i]);
				i++;
			}
			ViewBag.divId = GetRandomDivId();
			ChartController chart = new ChartController();
			ViewBag.datasets = chart.DoughnutChart(label, data);
			ViewData["devices"] = label.ToJson();

			return PartialView("_DoughnutChart");
		}

		[Route("Home/DoughnutUseTimeChart")]
		public ActionResult DoughnutUseTimeChart()
		{
			ViewBag.charttitle = Request.Query["title"];
			ViewBag.chartType = Request.Query["chartType"];
			ViewBag.position = Request.Query["position"];

			List<string> label = new List<string>();
			DevicesController device = new DevicesController();
			List<double> data = new List<double>();

			DevicesPowerUseOutputUtil powerUseOutputUtil = new DevicesPowerUseOutputUtil();

			double ACpower = Math.Round((powerUseOutputUtil.getACPowerUseTime()/3600), 2);
			double LTpower = Math.Round((powerUseOutputUtil.getLPowerUseTime() / 3600), 2);
			double HDpower = Math.Round((powerUseOutputUtil.getHUMPowerUseTime() / 3600), 2);
			double EFpower = Math.Round((powerUseOutputUtil.getEXHFPowerUseTime() / 3600), 2);

			double[] myNum = { ACpower, LTpower, HDpower, EFpower };
			var i = 0;
			foreach (string getDeviceTypeName in device.typeName)
			{
				label.Add(getDeviceTypeName);
				//data.Add(device.GetDeviceCount(getDeviceTypeName));
				data.Add(myNum[i]);
				i++;
			}
			ViewBag.divId = GetRandomDivId();
			ChartController chart = new ChartController();
			ViewBag.datasets = chart.DoughnutChart(label, data);
			ViewData["devices"] = label.ToJson();

			return PartialView("_DoughnutChart");
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
		public IActionResult UserSetting()
		{
			MongoUserModel user = HttpContext.Session.Get<MongoUserModel>("user");

			

			return View();
		}

		[Route("Home/MonthlyReport")]
		public void MonthlyReport()
		{
			try
			{
				Debug.WriteLine("Running sample 8");
				var output = PrintOutInExcel.Run();
				Debug.WriteLine("Sample 8 created: {0}", output);
				Debug.WriteLine("");

			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: {0}", ex.Message);
			}
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
