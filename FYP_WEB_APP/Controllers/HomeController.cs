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


		public IActionResult Home()
		{
			//MongoUserModel user = HttpContext.Session.Get<MongoUserModel>("user");
			return View();
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
