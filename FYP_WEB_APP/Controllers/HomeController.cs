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

namespace FYP_APP.Controllers
{
	public class HomeController : Controller
	{



		[HttpGet]
		public IActionResult Index()
		{
			if (HttpContext.Session.Get<MongoUserModel>("user")!= null)
			{
				return RedirectToAction("Home", "Home");
			}
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
				return RedirectToAction("Home", "Home");
			}
			else
			{
				ViewData["message"] = "Wrong user name or password!";
				
			}
			

			return View();
		}


		public IActionResult dashboard()
		{
			//MongoUserModel user = HttpContext.Session.Get<MongoUserModel>("user");
			return View();
		}
		
		[Route("Home/DoughnutChart")]
		public ActionResult DoughnutChart()
		{
			//string id = "";
			//id = Request.Query["roomID"];

			//getdb();
			//List<SensorsListModel> lists = GetSensorsData();
			//Debug.WriteLine(lists.ToJson().ToString());
			Debug.WriteLine(Request.Query["title"]);
			Debug.WriteLine(Request.Query["chartType"]);
			Debug.WriteLine(Request.Query["position"]);
			//Debug.WriteLine(Request.Query["sensorType"]);
			//Debug.WriteLine(Setgroup(lists).ToJson().ToString());
			ViewBag.charttitle = Request.Query["title"];
			ViewBag.chartType = Request.Query["chartType"];
			ViewBag.position = Request.Query["position"];
			//ViewBag.download = Request.Query["download"];

			//ViewBag.day = getChartTime();
			//ViewBag.datasets = chartData(lists, Request.Query["sensorType"]);
			ViewBag.divId = getRandomDivId();
			
			return PartialView("_DoughnutChart");
		}
		public string getRandomColor()
		{
			var random = new Random();
			var rmcolor = String.Format("#{0:X6}", random.Next(0x1000000));
			return rmcolor;
		}
		public string getRandomDivId()
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



		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
